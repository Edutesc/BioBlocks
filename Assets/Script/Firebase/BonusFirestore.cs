using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class BonusFirestore
{
    private const string COLLECTION_NAME = "UserBonus";
    private const string CORRECT_ANSWER_BONUS = "correctAnswerBonus";
    private const int BONUS_ACTIVATION_THRESHOLD = 5;

    private FirebaseFirestore db;
    private ListenerRegistration bonusListener;

    public BonusFirestore()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public async Task IncrementCorrectAnswerBonus(string userId, float bonusDuration = 600f)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
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
                                    bonus.ExpirationTimestamp = 0;
                                    Debug.Log($"BonusFirestore: Bônus {bonus.BonusName} expirado para o usuário {userId}");
                                }
                                
                                bonusList.Add(bonus);
                            }
                        }
                    }
                }

                BonusType correctAnswerBonus = bonusList.FirstOrDefault(b => b.BonusName == CORRECT_ANSWER_BONUS);

                if (correctAnswerBonus != null)
                {
                    if (!correctAnswerBonus.IsBonusActive)
                    {
                        correctAnswerBonus.BonusCount++;

                        if (correctAnswerBonus.BonusCount >= BONUS_ACTIVATION_THRESHOLD)
                        {
                            correctAnswerBonus.IsBonusActive = true;
                            correctAnswerBonus.IsPersistent = true;
                            correctAnswerBonus.SetExpirationFromDuration(bonusDuration);
                            Debug.Log($"BonusFirestore: Bônus {CORRECT_ANSWER_BONUS} ativado para o usuário {userId} até {DateTimeOffset.FromUnixTimeSeconds(correctAnswerBonus.ExpirationTimestamp).LocalDateTime}");
                        }
                    }
                }
                else
                {
                    bonusList.Add(new BonusType(CORRECT_ANSWER_BONUS, 1, false, 0, false));
                }
            }
            else
            {
                bonusList.Add(new BonusType(CORRECT_ANSWER_BONUS, 1, false, 0, false));
            }

            await SaveBonusList(userId, bonusList);

            Debug.Log($"BonusFirestore: Bônus {CORRECT_ANSWER_BONUS} incrementado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao incrementar bônus: {e.Message}");
        }
    }
    
    public async Task UpdateBonus(string userId, BonusType bonus)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType existingBonus = bonusList.FirstOrDefault(b => b.BonusName == bonus.BonusName);
            if (existingBonus != null)
            {
                // Atualiza o bônus existente
                existingBonus.IsBonusActive = bonus.IsBonusActive;
                existingBonus.BonusCount = bonus.BonusCount;
                existingBonus.ExpirationTimestamp = bonus.ExpirationTimestamp;
                existingBonus.IsPersistent = bonus.IsPersistent;
            }
            else
            {
                bonusList.Add(bonus);
            }
            
            await SaveBonusList(userId, bonusList);
            Debug.Log($"BonusFirestore: Bônus {bonus.BonusName} atualizado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao atualizar bônus: {e.Message}");
        }
    }
    
    public async Task ActivatePersistentBonus(string userId, string bonusName, float durationInSeconds)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType existingBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
            if (existingBonus != null)
            {
                // Atualiza o bônus existente
                existingBonus.IsBonusActive = true;
                existingBonus.IsPersistent = true;
                existingBonus.SetExpirationFromDuration(durationInSeconds);
            }
            else
            {
                BonusType newBonus = new BonusType(bonusName, BONUS_ACTIVATION_THRESHOLD, true, 0, true);
                newBonus.SetExpirationFromDuration(durationInSeconds);
                bonusList.Add(newBonus);
            }
            
            await SaveBonusList(userId, bonusList);
            Debug.Log($"BonusFirestore: Bônus persistente {bonusName} ativado para o usuário {userId} por {durationInSeconds} segundos");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao ativar bônus persistente: {e.Message}");
        }
    }

    public async Task DeactivateBonus(string userId, string bonusName)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType existingBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
            if (existingBonus != null)
            {
                existingBonus.IsBonusActive = false;
                existingBonus.ExpirationTimestamp = 0;
            }
            
            await SaveBonusList(userId, bonusList);
            Debug.Log($"BonusFirestore: Bônus {bonusName} desativado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao desativar bônus: {e.Message}");
        }
    }

    private async Task SaveBonusList(string userId, List<BonusType> bonusList)
    {
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "BonusList", bonusList.Select(b => b.ToDictionary()).ToList() }
            };

            await docRef.SetAsync(data);
            Debug.Log($"BonusFirestore: Lista de bônus salva para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao salvar lista de bônus: {e.Message}");
        }
    }

    public async Task<List<BonusType>> GetUserBonuses(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
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
                                
                                // Verificar se o bônus está ativo mas expirou
                                if (bonus.IsBonusActive && bonus.IsExpired())
                                {
                                    bonus.IsBonusActive = false;
                                    // Atualizaremos o estado no Firestore após retornar a lista
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
                        Debug.Log($"BonusFirestore: Bônus {expiredBonus.BonusName} expirado para o usuário {userId}");
                    }
                    
                    await SaveBonusList(userId, bonusList);
                }
            }

            return bonusList;
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao obter bônus do usuário: {e.Message}");
            return new List<BonusType>();
        }
    }

    public void ListenForBonusUpdates(string userId, Action<List<BonusType>> onUpdate)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
        }

        try
        {
            StopListeningForBonusUpdates();
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            bonusListener = docRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    List<BonusType> bonusList = new List<BonusType>();
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

                    onUpdate?.Invoke(bonusList);
                }
                else
                {
                    onUpdate?.Invoke(new List<BonusType>());
                }
            });

            Debug.Log($"BonusFirestore: Ouvindo atualizações para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao configurar listener: {e.Message}");
        }
    }

    public void StopListeningForBonusUpdates()
    {
        bonusListener?.Stop();
        bonusListener = null;
        Debug.Log("BonusFirestore: Parou de ouvir atualizações");
    }
}