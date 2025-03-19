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
    
    // Método para ativar o bônus de respostas corretas
    public async Task ActivateCorrectAnswerBonus(string userId, float durationInSeconds = 600f)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("CorrectAnswerBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            // Calcular timestamp de expiração
            long expirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(durationInSeconds).ToUnixTimeSeconds();
            
            // Dados a serem salvos
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "ExpirationTimestamp", expirationTimestamp },
                { "IsActive", true },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };
            
            // Salvar no Firestore
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);
            
            Debug.Log($"CorrectAnswerBonusManager: Bônus ativado para o usuário {userId} até {DateTimeOffset.FromUnixTimeSeconds(expirationTimestamp).LocalDateTime}");
            
            // Incrementar contagem no outro sistema
            await IncrementSpecialBonusCounter(userId);
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao ativar bônus: {e.Message}");
        }
    }
    
    // Método para verificar se o bônus está ativo
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
                    
                    // Verificar se o bônus expirou
                    if (isActive && currentTimestamp >= expirationTimestamp)
                    {
                        // Bônus expirou, desativar
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
    
    // Método para obter o tempo restante do bônus em segundos
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
                        // Calcular tempo restante
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
    
    // Método para desativar o bônus
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
    
    // Método para atualizar o timestamp de expiração
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
            
            Debug.Log($"CorrectAnswerBonusManager: Timestamp de expiração atualizado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao atualizar timestamp: {e.Message}");
        }
    }
    
    // Método privado para incrementar o contador no SpecialBonusManager
    private async Task IncrementSpecialBonusCounter(string userId)
    {
        try
        {
            // Criar instância do SpecialBonusManager
            SpecialBonusManager specialBonusManager = new SpecialBonusManager();
            
            // Incrementar contador
            await specialBonusManager.IncrementBonusCounter(userId, "specialBonus");
            
            Debug.Log("CorrectAnswerBonusManager: Contador de bônus especial incrementado");
        }
        catch (Exception e)
        {
            Debug.LogError($"CorrectAnswerBonusManager: Erro ao incrementar contador de bônus especial: {e.Message}");
        }
    }
}