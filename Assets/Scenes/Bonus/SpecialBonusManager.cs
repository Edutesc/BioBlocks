using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Linq;
using Firebase.Firestore;


public class SpecialBonusManager
{
    private const string COLLECTION_NAME = "UserBonus";
    private const int BONUS_ACTIVATION_THRESHOLD = 5;
    private FirebaseFirestore db;
    
    public SpecialBonusManager()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    
    // Método para incrementar o contador de um bônus
    public async Task IncrementBonusCounter(string userId, string bonusName)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("SpecialBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            // Obter lista atual de bônus
            List<BonusType> bonusList = await GetUserBonuses(userId);
            
            // Procurar o bônus específico
            BonusType targetBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
            
            if (targetBonus != null)
            {
                // Incrementar contador
                targetBonus.BonusCount++;
                
                // Verificar se atingiu o limite para ativação
                if (targetBonus.BonusCount >= BONUS_ACTIVATION_THRESHOLD)
                {
                    targetBonus.IsBonusActive = true;
                    Debug.Log($"SpecialBonusManager: Bônus {bonusName} ativado para o usuário {userId} após atingir {BONUS_ACTIVATION_THRESHOLD} conquistas");
                }
            }
            else
            {
                // Criar novo bônus com contador 1
                targetBonus = new BonusType(bonusName, 1, false, 0, false);
                bonusList.Add(targetBonus);
            }
            
            // Salvar alterações
            await SaveBonusList(userId, bonusList);
            
            Debug.Log($"SpecialBonusManager: Contador do bônus {bonusName} incrementado para {targetBonus.BonusCount}");
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusManager: Erro ao incrementar contador: {e.Message}");
        }
    }
    
    // Método para ativar um bônus (quando o usuário decide usá-lo)
    public async Task ActivateBonus(string userId, string bonusName, float durationInSeconds)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("SpecialBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            // Obter lista atual de bônus
            List<BonusType> bonusList = await GetUserBonuses(userId);
            
            // Procurar o bônus específico
            BonusType targetBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
            
            if (targetBonus != null && targetBonus.IsBonusActive)
            {
                // Desativar o direito ao bônus
                targetBonus.IsBonusActive = false;
                
                // Criar um bônus ativo temporário
                string activeBonusName = $"active_{bonusName}";
                BonusType activeBonus = bonusList.FirstOrDefault(b => b.BonusName == activeBonusName);
                
                if (activeBonus == null)
                {
                    activeBonus = new BonusType(activeBonusName, 0, true, 0, true);
                    bonusList.Add(activeBonus);
                }
                else
                {
                    activeBonus.IsBonusActive = true;
                    activeBonus.IsPersistent = true;
                }
                
                // Configurar duração
                activeBonus.SetExpirationFromDuration(durationInSeconds);
                
                // Salvar alterações
                await SaveBonusList(userId, bonusList);
                
                Debug.Log($"SpecialBonusManager: Bônus {bonusName} ativado por {durationInSeconds} segundos");
            }
            else
            {
                Debug.LogWarning($"SpecialBonusManager: Bônus {bonusName} não está disponível para ativação");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusManager: Erro ao ativar bônus: {e.Message}");
        }
    }
    
    // Método para obter a lista de bônus do usuário
    public async Task<List<BonusType>> GetUserBonuses(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("SpecialBonusManager: UserId é nulo ou vazio");
            return new List<BonusType>();
        }
        
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            List<BonusType> bonusList = new List<BonusType>();
            
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                
                if (data.ContainsKey("BonusList"))
                {
                    List<object> bonusListData = data["BonusList"] as List<object>;
                    
                    if (bonusListData != null)
                    {
                        foreach (object bonusObj in bonusListData)
                        {
                            Dictionary<string, object> bonusDict = bonusObj as Dictionary<string, object>;
                            if (bonusDict != null)
                            {
                                BonusType bonus = new BonusType(
                                    bonusDict.ContainsKey("BonusName") ? bonusDict["BonusName"].ToString() : "",
                                    bonusDict.ContainsKey("BonusCount") ? Convert.ToInt32(bonusDict["BonusCount"]) : 0,
                                    bonusDict.ContainsKey("IsBonusActive") ? Convert.ToBoolean(bonusDict["IsBonusActive"]) : false,
                                    bonusDict.ContainsKey("ExpirationTimestamp") ? Convert.ToInt64(bonusDict["ExpirationTimestamp"]) : 0,
                                    bonusDict.ContainsKey("IsPersistent") ? Convert.ToBoolean(bonusDict["IsPersistent"]) : false
                                );
                                
                                // Verificar se o bônus expirou
                                if (bonus.IsBonusActive && bonus.IsExpired())
                                {
                                    bonus.IsBonusActive = false;
                                }
                                
                                bonusList.Add(bonus);
                            }
                        }
                    }
                }
                
                // Verificar e atualizar bônus expirados
                List<BonusType> expiredBonuses = bonusList.Where(b => b.IsBonusActive && b.IsExpired()).ToList();
                if (expiredBonuses.Any())
                {
                    foreach (var expiredBonus in expiredBonuses)
                    {
                        expiredBonus.IsBonusActive = false;
                        Debug.Log($"SpecialBonusManager: Bônus {expiredBonus.BonusName} expirado");
                    }
                    
                    await SaveBonusList(userId, bonusList);
                }
            }
            
            return bonusList;
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusManager: Erro ao obter bônus do usuário: {e.Message}");
            return new List<BonusType>();
        }
    }
    
    // Método para salvar a lista de bônus
    public async Task SaveBonusList(string userId, List<BonusType> bonusList)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("SpecialBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "BonusList", bonusList.Select(b => b.ToDictionary()).ToList() },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };
            
            await docRef.SetAsync(data);
            
            Debug.Log($"SpecialBonusManager: Lista de bônus salva para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusManager: Erro ao salvar lista de bônus: {e.Message}");
        }
    }
}
