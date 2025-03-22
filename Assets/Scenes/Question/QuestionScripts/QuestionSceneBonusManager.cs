using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Firebase.Firestore;
using System.Linq;

public class QuestionSceneBonusManager
{
    private const string COLLECTION_NAME = "QuestionSceneBonus";
    private const string ACTIVE_BONUSES_FIELD = "ActiveBonuses";
    private FirebaseFirestore db;

    public QuestionSceneBonusManager()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    // Ativa um bônus para o usuário
    public async Task ActivateBonus(string userId, string bonusType, float durationInSeconds = 600f, int multiplier = 2)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("QuestionSceneBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            // Verificar bônus existentes
            List<Dictionary<string, object>> activeBonuses = await GetActiveBonuses(userId);

            // Remover qualquer bônus existente do mesmo tipo
            activeBonuses.RemoveAll(b => b.ContainsKey("BonusType") && b["BonusType"].ToString() == bonusType);

            // Criar novo bônus
            long expirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(durationInSeconds).ToUnixTimeSeconds();
            Dictionary<string, object> newBonus = new Dictionary<string, object>
            {
                { "BonusType", bonusType },
                { "BonusMultiplier", multiplier },
                { "ExpirationTimestamp", expirationTimestamp }
            };

            // Adicionar à lista
            activeBonuses.Add(newBonus);

            // Salvar todos os bônus
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { ACTIVE_BONUSES_FIELD, activeBonuses },
                { "IsActive", true },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };

            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);

            // Se for o bônus de resposta correta, incrementar o contador do bônus especial
            if (bonusType == "correctAnswerBonus")
            {
                SpecialBonusManager specialBonusManager = new SpecialBonusManager();
                await specialBonusManager.IncrementBonusCounter(userId, "specialBonus");
            }

            Debug.Log($"QuestionSceneBonusManager: Bônus {bonusType} ativado com multiplicador {multiplier}");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao ativar bônus: {e.Message}");
        }
    }

    // Obtém a lista de bônus ativos (não expirados)
    public async Task<List<Dictionary<string, object>>> GetActiveBonuses(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("QuestionSceneBonusManager: UserId é nulo ou vazio");
            return new List<Dictionary<string, object>>();
        }

        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return new List<Dictionary<string, object>>();
            }

            Dictionary<string, object> data = snapshot.ToDictionary();

            if (!data.ContainsKey(ACTIVE_BONUSES_FIELD))
            {
                return new List<Dictionary<string, object>>();
            }

            // Converter o objeto para lista
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (data[ACTIVE_BONUSES_FIELD] is List<object> bonusList)
            {
                foreach (object bonusObj in bonusList)
                {
                    if (bonusObj is Dictionary<string, object> bonusDict)
                    {
                        // Verificar se não está expirado
                        if (bonusDict.ContainsKey("ExpirationTimestamp"))
                        {
                            long expirationTimestamp = Convert.ToInt64(bonusDict["ExpirationTimestamp"]);

                            if (currentTimestamp < expirationTimestamp)
                            {
                                result.Add(bonusDict);
                            }
                        }
                    }
                }
            }

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao obter bônus ativos: {e.Message}");
            return new List<Dictionary<string, object>>();
        }
    }

    // Calcula o multiplicador combinado de todos os bônus ativos
    public async Task<int> GetCombinedMultiplier(string userId)
    {
        try
        {
            List<Dictionary<string, object>> activeBonuses = await GetActiveBonuses(userId);

            if (activeBonuses.Count == 0)
            {
                return 1; // Sem bônus
            }

            // Multiplicar todos os multiplicadores
            int combinedMultiplier = 1;

            foreach (var bonus in activeBonuses)
            {
                if (bonus.ContainsKey("BonusMultiplier"))
                {
                    int multiplier = Convert.ToInt32(bonus["BonusMultiplier"]);
                    combinedMultiplier *= multiplier;
                }
            }

            return combinedMultiplier;
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao calcular multiplicador: {e.Message}");
            return 1;
        }
    }

    // Desativa todos os bônus
    public async Task DeactivateAllBonuses(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("QuestionSceneBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { ACTIVE_BONUSES_FIELD, new List<object>() },
                { "IsActive", false },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };

            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);

            Debug.Log($"QuestionSceneBonusManager: Todos os bônus desativados para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao desativar bônus: {e.Message}");
        }
    }

    // Desativa um tipo específico de bônus
    public async Task DeactivateBonus(string userId, string bonusType)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("QuestionSceneBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<Dictionary<string, object>> activeBonuses = await GetActiveBonuses(userId);

            // Remover o bônus específico
            activeBonuses.RemoveAll(b => b.ContainsKey("BonusType") && b["BonusType"].ToString() == bonusType);

            // Salvar a lista atualizada
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { ACTIVE_BONUSES_FIELD, activeBonuses },
                { "IsActive", activeBonuses.Count > 0 },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };

            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);

            Debug.Log($"QuestionSceneBonusManager: Bônus {bonusType} desativado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao desativar bônus específico: {e.Message}");
        }
    }

    public async Task UpdateExpirationTimestamp(string userId, float remainingSeconds)
    {
        await UpdateBonusExpirations(userId, remainingSeconds);
    }

    public async Task UpdateBonusExpirations(string userId, float remainingSeconds)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("QuestionSceneBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<Dictionary<string, object>> activeBonuses = await GetActiveBonuses(userId);

            if (activeBonuses.Count == 0)
            {
                return;
            }

            // Definir a nova expiração
            long newExpirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(remainingSeconds).ToUnixTimeSeconds();

            // Atualizar todos os bônus
            foreach (var bonus in activeBonuses)
            {
                bonus["ExpirationTimestamp"] = newExpirationTimestamp;
            }

            // Salvar a lista atualizada
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { ACTIVE_BONUSES_FIELD, activeBonuses },
                { "UpdatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };

            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            await docRef.SetAsync(data, SetOptions.MergeAll);

            Debug.Log($"QuestionSceneBonusManager: Expiração atualizada para {remainingSeconds} segundos");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao atualizar expiração: {e.Message}");
        }
    }

    // Verifica se há algum bônus ativo
    public async Task<bool> HasAnyActiveBonus(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return false;
        }

        try
        {
            List<Dictionary<string, object>> activeBonuses = await GetActiveBonuses(userId);
            return activeBonuses.Count > 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao verificar bônus ativos: {e.Message}");
            return false;
        }
    }

    // Obtém o bônus que expira primeiro (útil para o timer)
    public async Task<Dictionary<string, object>> GetEarliestExpiringBonus(string userId)
    {
        try
        {
            List<Dictionary<string, object>> activeBonuses = await GetActiveBonuses(userId);

            if (activeBonuses.Count == 0)
            {
                return null;
            }

            Dictionary<string, object> earliestBonus = null;
            long earliestExpiration = long.MaxValue;

            foreach (var bonus in activeBonuses)
            {
                if (bonus.ContainsKey("ExpirationTimestamp"))
                {
                    long expiration = Convert.ToInt64(bonus["ExpirationTimestamp"]);

                    if (expiration < earliestExpiration)
                    {
                        earliestExpiration = expiration;
                        earliestBonus = bonus;
                    }
                }
            }

            return earliestBonus;
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao obter bônus mais próximo de expirar: {e.Message}");
            return null;
        }
    }

    // Obtém o tempo restante para o primeiro bônus expirar
    public async Task<float> GetRemainingTime(string userId)
    {
        try
        {
            Dictionary<string, object> earliestBonus = await GetEarliestExpiringBonus(userId);

            if (earliestBonus == null || !earliestBonus.ContainsKey("ExpirationTimestamp"))
            {
                return 0;
            }

            long expirationTimestamp = Convert.ToInt64(earliestBonus["ExpirationTimestamp"]);
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return Math.Max(0, expirationTimestamp - currentTimestamp);
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao obter tempo restante: {e.Message}");
            return 0;
        }
    }

    public async Task<bool> IsBonusActive(string userId, string bonusType = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("QuestionSceneBonusManager: UserId é nulo ou vazio");
            return false;
        }

        try
        {
            List<Dictionary<string, object>> activeBonuses = await GetActiveBonuses(userId);

            if (bonusType != null)
            {
                // Verificar um tipo específico de bônus
                return activeBonuses.Any(b =>
                    b.ContainsKey("BonusType") &&
                    b["BonusType"].ToString() == bonusType);
            }
            else
            {
                // Verificar se há qualquer bônus ativo
                return activeBonuses.Count > 0;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao verificar bônus ativo: {e.Message}");
            return false;
        }
    }
}