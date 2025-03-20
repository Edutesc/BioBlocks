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
    
    public async Task IncrementBonusCounter(string userId, string bonusName)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("SpecialBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType targetBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
            
            if (targetBonus != null)
            {
                targetBonus.BonusCount++;

                if (targetBonus.BonusCount >= BONUS_ACTIVATION_THRESHOLD)
                {
                    targetBonus.IsBonusActive = true;
                }
            }
            else
            {
                targetBonus = new BonusType(bonusName, 1, false, 0, false);
                bonusList.Add(targetBonus);
            }
            
            await SaveBonusList(userId, bonusList);
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusManager: Erro ao incrementar contador: {e.Message}");
        }
    }
    
    public async Task ActivateBonus(string userId, string bonusName, float durationInSeconds)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("SpecialBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType targetBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
            
            if (targetBonus != null && targetBonus.IsBonusActive)
            {
                targetBonus.IsBonusActive = false;
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
                
                activeBonus.SetExpirationFromDuration(durationInSeconds);
                await SaveBonusList(userId, bonusList);
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
                                
                                if (bonus.IsBonusActive && bonus.IsExpired())
                                {
                                    bonus.IsBonusActive = false;
                                }
                                
                                bonusList.Add(bonus);
                            }
                        }
                    }
                }

                List<BonusType> expiredBonuses = bonusList.Where(b => b.IsBonusActive && b.IsExpired()).ToList();
                if (expiredBonuses.Any())
                {
                    foreach (var expiredBonus in expiredBonuses)
                    {
                        expiredBonus.IsBonusActive = false;
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
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusManager: Erro ao salvar lista de bônus: {e.Message}");
        }
    }
}
