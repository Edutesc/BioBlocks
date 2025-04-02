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

    public async Task ActivateBonus(string userId, string bonusType, float durationInSeconds, int multiplier)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("QuestionSceneBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            Debug.Log($"QuestionSceneBonusManager: Ativando bônus {bonusType} com multiplicador {multiplier} por {durationInSeconds} segundos");

            // Calcular timestamp de expiração
            long expirationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (long)durationInSeconds;

            // Preparar dados do bônus
            Dictionary<string, object> bonusData = new Dictionary<string, object>
        {
            { "BonusType", bonusType },
            { "BonusMultiplier", multiplier },
            { "ExpirationTimestamp", expirationTimestamp },
            { "ActivatedAt", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
        };

            // Salvar no Firestore
            DocumentReference docRef = FirebaseFirestore.DefaultInstance.Collection("QuestionSceneBonus").Document(userId);

            DocumentSnapshot existing = await docRef.GetSnapshotAsync();
            if (existing.Exists)
            {
                // Atualizar documento existente
                Dictionary<string, object> data = existing.ToDictionary();

                if (data.ContainsKey("ActiveBonuses"))
                {
                    List<object> activeBonuses = data["ActiveBonuses"] as List<object>;
                    if (activeBonuses != null)
                    {
                        // Verificar se já existe um bônus do mesmo tipo
                        bool updated = false;
                        List<Dictionary<string, object>> updatedBonuses = new List<Dictionary<string, object>>();

                        foreach (object bonusObj in activeBonuses)
                        {
                            Dictionary<string, object> existingBonus = bonusObj as Dictionary<string, object>;
                            if (existingBonus != null)
                            {
                                // Se encontrar um bônus do mesmo tipo, atualizá-lo
                                if (existingBonus.ContainsKey("BonusType") && existingBonus["BonusType"].ToString() == bonusType)
                                {
                                    updatedBonuses.Add(bonusData);
                                    updated = true;
                                }
                                else
                                {
                                    updatedBonuses.Add(existingBonus);
                                }
                            }
                        }

                        // Se não encontrou um bônus do mesmo tipo, adicionar um novo
                        if (!updated)
                        {
                            updatedBonuses.Add(bonusData);
                        }

                        // Atualizar o documento
                        await docRef.UpdateAsync(new Dictionary<string, object>
                    {
                        { "ActiveBonuses", updatedBonuses }
                    });

                        Debug.Log($"QuestionSceneBonusManager: Bônus {bonusType} atualizado com sucesso");
                        return;
                    }
                }

                // Se não havia lista de bônus ativa, criar uma nova
                await docRef.UpdateAsync(new Dictionary<string, object>
            {
                { "ActiveBonuses", new List<Dictionary<string, object>> { bonusData } }
            });
            }
            else
            {
                // Criar um novo documento
                await docRef.SetAsync(new Dictionary<string, object>
            {
                { "ActiveBonuses", new List<Dictionary<string, object>> { bonusData } },
                { "UserId", userId }
            });
            }

            Debug.Log($"QuestionSceneBonusManager: Bônus {bonusType} ativado com sucesso");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionSceneBonusManager: Erro ao ativar bônus: {e.Message}\n{e.StackTrace}");
            throw;
        }
    }

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