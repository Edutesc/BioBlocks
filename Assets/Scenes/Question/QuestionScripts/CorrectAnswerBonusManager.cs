using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Firebase.Firestore;

public class CorrectAnswerBonusManager
{
    private const string COLLECTION_NAME = "UsersCorrectAnswer";
    private FirebaseFirestore db;
    
    public CorrectAnswerBonusManager()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    
    public async Task ActivateCorrectAnswerBonus(string userId, float durationInSeconds = 600f)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("CorrectAnswerBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            long expirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(durationInSeconds).ToUnixTimeSeconds();
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "ExpirationTimestamp", expirationTimestamp },
                { "IsActive", true },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };
            
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);
            await IncrementSpecialBonusCounter(userId);
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao ativar bônus: {e.Message}");
        }
    }

    public async Task<bool> IsCorrectAnswerBonusActive(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("CorrectAnswerBonusManager: UserId é nulo ou vazio");
            return false;
        }
        
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                
                if (data.ContainsKey("ExpirationTimestamp") && data.ContainsKey("IsActive"))
                {
                    bool isActive = Convert.ToBoolean(data["IsActive"]);
                    long expirationTimestamp = Convert.ToInt64(data["ExpirationTimestamp"]);
                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    
                    if (isActive && currentTimestamp >= expirationTimestamp)
                    {
                        await DeactivateCorrectAnswerBonus(userId);
                        return false;
                    }
                    
                    return isActive;
                }
            }
            
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao verificar bônus: {e.Message}");
            return false;
        }
    }
    
    public async Task<float> GetRemainingTime(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("CorrectAnswerBonusManager: UserId é nulo ou vazio");
            return 0;
        }
        
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                
                if (data.ContainsKey("ExpirationTimestamp") && data.ContainsKey("IsActive"))
                {
                    bool isActive = Convert.ToBoolean(data["IsActive"]);
                    long expirationTimestamp = Convert.ToInt64(data["ExpirationTimestamp"]);
                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    
                    if (isActive)
                    {
                        return Math.Max(0, expirationTimestamp - currentTimestamp);
                    }
                }
            }
            
            return 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao obter tempo restante: {e.Message}");
            return 0;
        }
    }
    
    public async Task DeactivateCorrectAnswerBonus(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("CorrectAnswerBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "IsActive", false },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };
            
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);
            
            Debug.Log($"CorrectAnswerBonusManager: Bônus desativado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao desativar bônus: {e.Message}");
        }
    }
    
    public async Task UpdateExpirationTimestamp(string userId, float remainingSeconds)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("CorrectAnswerBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            long expirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(remainingSeconds).ToUnixTimeSeconds();
            
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "ExpirationTimestamp", expirationTimestamp },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };
            
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao atualizar timestamp: {e.Message}");
        }
    }
    
    private async Task IncrementSpecialBonusCounter(string userId)
    {
        try
        {
            SpecialBonusManager specialBonusManager = new SpecialBonusManager();
            await specialBonusManager.IncrementBonusCounter(userId, "specialBonus");
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao incrementar contador de bônus especial: {e.Message}");
        }
    }
}