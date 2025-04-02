using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Firestore;

public class UserBonusManager
{
    private const string COLLECTION_NAME = "UserBonus";
    private const int SPECIAL_BONUS_ACTIVATION_THRESHOLD = 5;
    private FirebaseFirestore db;

    public UserBonusManager()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    #region Bonus Generic Methods

    public async Task<List<BonusType>> GetUserBonuses(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
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
            Debug.LogError($"UserBonusManager: Erro ao obter bônus do usuário: {e.Message}");
            return new List<BonusType>();
        }
    }

    public async Task SaveBonusList(string userId, List<BonusType> bonusList)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
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
            Debug.LogError($"UserBonusManager: Erro ao salvar lista de bônus: {e.Message}");
        }
    }

    public async Task<BonusType> GetBonusByName(string userId, string bonusName)
    {
        List<BonusType> bonusList = await GetUserBonuses(userId);
        return bonusList.FirstOrDefault(b => b.BonusName == bonusName);
    }

    public async Task<bool> IsBonusActive(string userId, string bonusName)
    {
        BonusType bonus = await GetBonusByName(userId, bonusName);
        return bonus != null && bonus.IsBonusActive && !bonus.IsExpired();
    }

    public async Task<int> GetBonusCount(string userId, string bonusName)
    {
        BonusType bonus = await GetBonusByName(userId, bonusName);
        return bonus?.BonusCount ?? 0;
    }

    public async Task IncrementBonusCount(string userId, string bonusName, int incrementAmount = 1, bool autoActivate = false)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType targetBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);

            if (targetBonus != null)
            {
                targetBonus.BonusCount += incrementAmount;

                // Se autoActivate for verdadeiro ou se for o specialBonus com count >= threshold
                if (autoActivate || (bonusName == "specialBonus" && targetBonus.BonusCount >= SPECIAL_BONUS_ACTIVATION_THRESHOLD))
                {
                    targetBonus.IsBonusActive = true;
                }
            }
            else
            {
                targetBonus = new BonusType(bonusName, incrementAmount, autoActivate, 0, false);
                bonusList.Add(targetBonus);
            }

            await SaveBonusList(userId, bonusList);
            Debug.Log($"UserBonusManager: {bonusName} incrementado em {incrementAmount}. Novo valor: {targetBonus.BonusCount}");
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao incrementar contador: {e.Message}");
        }
    }

    public async Task DecrementBonusCount(string userId, string bonusName, int decrementAmount = 1)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType targetBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);

            if (targetBonus != null)
            {
                targetBonus.BonusCount = Math.Max(0, targetBonus.BonusCount - decrementAmount);

                // Desativar o bônus se o contador chegar a zero
                if (targetBonus.BonusCount == 0)
                {
                    targetBonus.IsBonusActive = false;
                }

                await SaveBonusList(userId, bonusList);
                Debug.Log($"UserBonusManager: {bonusName} decrementado em {decrementAmount}. Novo valor: {targetBonus.BonusCount}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao decrementar contador: {e.Message}");
        }
    }

    public async Task ActivateBonusInScene(string userId, string bonusName, float durationInSeconds, int multiplier)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            Debug.Log($"UserBonusManager: Tentando ativar {bonusName} na cena com multiplicador {multiplier} por {durationInSeconds} segundos");

            QuestionSceneBonusManager questionSceneBonusManager = new QuestionSceneBonusManager();
            if (questionSceneBonusManager == null)
            {
                Debug.LogError("UserBonusManager: Falha ao criar QuestionSceneBonusManager");
                return;
            }
            Debug.Log($"Passando pelo ActivateBonus");
            Debug.Log($"UserBonusManager: Chamando questionSceneBonusManager.ActivateBonus para {bonusName}");
            await questionSceneBonusManager.ActivateBonus(userId, bonusName, durationInSeconds, multiplier);
            Debug.Log($"UserBonusManager: {bonusName} ativado com sucesso na cena de perguntas");
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao ativar bônus na cena: {e.Message}\n{e.StackTrace}");
            throw;
        }
    }

    #endregion

    #region Special Bonus Methods

    public async Task IncrementSpecialBonus(string userId)
    {
        await IncrementBonusCount(userId, "specialBonus", 1);
    }

    public async Task<bool> ActivateSpecialBonus(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return false;
        }

        try
        {
            Debug.Log("UserBonusManager: Iniciando ativação do Special Bonus");
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType specialBonus = bonusList.FirstOrDefault(b => b.BonusName == "specialBonus");

            if (specialBonus != null && specialBonus.BonusCount >= SPECIAL_BONUS_ACTIVATION_THRESHOLD)
            {
                // Zerar o contador e desativar o bônus
                specialBonus.BonusCount = 0;
                specialBonus.IsBonusActive = false;

                // Salvar as alterações
                await SaveBonusList(userId, bonusList);

                // Ativar o bônus na cena (3x de multiplicador por 10 minutos)
                await ActivateBonusInScene(userId, "specialBonus", 600f, 3);

                Debug.Log("UserBonusManager: Special Bonus ativado com sucesso");
                return true;
            }
            else
            {
                Debug.LogWarning("UserBonusManager: Special Bonus não está disponível para ativação");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao ativar Special Bonus: {e.Message}");
            throw;
        }
    }

    #endregion

    #region List Completion Bonus Methods

    public async Task IncrementListCompletionBonus(string userId, string databankName)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            // Primeiro, verificar se o databank já foi marcado como completado
            bool isEligible = await CheckIfDatabankEligibleForBonus(userId, databankName);

            if (isEligible)
            {
                // Marcar o databank como completado
                await MarkDatabankAsCompleted(userId, databankName);

                // Incrementar o bônus
                await IncrementBonusCount(userId, "listCompletionBonus", 1, true);

                Debug.Log($"UserBonusManager: Bônus de lista incrementado para o databank {databankName}");
            }
            else
            {
                Debug.Log($"UserBonusManager: Databank {databankName} já foi marcado como completado");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao incrementar List Completion Bonus: {e.Message}");
        }
    }

    public async Task<bool> ActivateListCompletionBonus(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return false;
        }

        try
        {
            Debug.Log("UserBonusManager: Iniciando ativação do List Completion Bonus");
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType listBonus = bonusList.FirstOrDefault(b => b.BonusName == "listCompletionBonus");

            if (listBonus != null && listBonus.BonusCount > 0)
            {
                // Decrementar o contador em 1
                listBonus.BonusCount--;

                // Ajustar o status ativo com base no contador
                listBonus.IsBonusActive = listBonus.BonusCount > 0;

                // Salvar as alterações no UserBonus
                Debug.Log($"UserBonusManager: Salvando alterações do List Completion Bonus na coleção UserBonus");
                await SaveBonusList(userId, bonusList);

                // Ativar o bônus na cena (2x de multiplicador por 10 minutos)
                Debug.Log($"UserBonusManager: Tentando ativar List Completion Bonus na cena");
                await ActivateBonusInScene(userId, "listCompletionBonus", 600f, 2);

                Debug.Log($"UserBonusManager: List Completion Bonus ativado. Bônus restantes: {listBonus.BonusCount}");
                return true;
            }
            else
            {
                Debug.LogWarning("UserBonusManager: List Completion Bonus não está disponível para ativação");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao ativar List Completion Bonus: {e.Message}\n{e.StackTrace}");
            throw;
        }
    }

    public async Task<bool> CheckIfDatabankEligibleForBonus(string userId, string databankName)
    {
        try
        {
            // Verifica se o usuário já ganhou bônus por este databank específico
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();

                // Verificar se já existe um registro de lista completa para este databank
                if (data.ContainsKey("CompletedDatabanks"))
                {
                    List<object> completedDatabanks = data["CompletedDatabanks"] as List<object>;

                    if (completedDatabanks != null && completedDatabanks.Contains(databankName))
                    {
                        Debug.Log($"UserBonusManager: Databank {databankName} já foi marcado como completo");
                        return false;
                    }
                }

                // Se chegou aqui, o databank ainda não foi marcado
                return true;
            }

            // Se o documento não existe, ele é elegível
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao verificar elegibilidade do databank: {e.Message}");
            return false;
        }
    }

    public async Task MarkDatabankAsCompleted(string userId, string databankName)
    {
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            List<string> completedDatabanks = new List<string>();

            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();

                if (data.ContainsKey("CompletedDatabanks"))
                {
                    List<object> existingList = data["CompletedDatabanks"] as List<object>;

                    if (existingList != null)
                    {
                        completedDatabanks = existingList.Select(item => item.ToString()).ToList();
                    }
                }
            }

            // Adicionar o novo databank se ainda não estiver na lista
            if (!completedDatabanks.Contains(databankName))
            {
                completedDatabanks.Add(databankName);

                // Atualizar o documento
                Dictionary<string, object> updateData = new Dictionary<string, object>
                {
                    { "CompletedDatabanks", completedDatabanks }
                };

                await docRef.UpdateAsync(updateData);
                Debug.Log($"UserBonusManager: Databank {databankName} marcado como completo");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao marcar databank como completo: {e.Message}");
        }
    }

    #endregion

    #region Persistence Bonus Methods

    public async Task IncrementPersistenceBonus(string userId)
    {
        await IncrementBonusCount(userId, "persistenceBonus", 1, true);
    }

    public async Task<bool> ActivatePersistenceBonus(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return false;
        }

        try
        {
            Debug.Log("UserBonusManager: Iniciando ativação do Persistence Bonus");
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType persistenceBonus = bonusList.FirstOrDefault(b => b.BonusName == "persistenceBonus");

            if (persistenceBonus != null && persistenceBonus.BonusCount > 0)
            {
                // Decrementar o contador em 1
                persistenceBonus.BonusCount--;

                // Ajustar o status ativo com base no contador
                persistenceBonus.IsBonusActive = persistenceBonus.BonusCount > 0;

                // Salvar as alterações
                await SaveBonusList(userId, bonusList);

                // Ativar o bônus na cena (configuração a definir)
                await ActivateBonusInScene(userId, "persistenceBonus", 600f, 2);

                Debug.Log($"UserBonusManager: Persistence Bonus ativado. Bônus restantes: {persistenceBonus.BonusCount}");
                return true;
            }
            else
            {
                Debug.LogWarning("UserBonusManager: Persistence Bonus não está disponível para ativação");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao ativar Persistence Bonus: {e.Message}");
            throw;
        }
    }

    #endregion

    // Você pode adicionar regiões para os outros bônus seguindo a mesma estrutura
    // #region NewBonus Methods
    // ...
    // #endregion
}