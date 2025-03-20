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

                // Buscar o correctAnswerBonus
                BonusType correctAnswerBonus = bonusList.FirstOrDefault(b => b.BonusName == CORRECT_ANSWER_BONUS);

                // Buscar o bônus especial (Bonus Especial)
                BonusType specialBonus = bonusList.FirstOrDefault(b => b.BonusName == "specialBonus");

                if (correctAnswerBonus != null)
                {
                    // IMPORTANTE: Só incrementar o BonusCount se o bônus não estiver ativo atualmente
                    // Isso impede que o contador aumente várias vezes durante a mesma sessão de bônus
                    if (!correctAnswerBonus.IsBonusActive)
                    {
                        // Incrementar o contador de vezes que o usuário ganhou o correctAnswerBonus
                        correctAnswerBonus.BonusCount++;
                        Debug.Log($"BonusFirestore: Contador de correctAnswerBonus incrementado para {correctAnswerBonus.BonusCount}");
                    }

                    // Sempre ativar o bônus quando chamamos IncrementCorrectAnswerBonus
                    correctAnswerBonus.IsBonusActive = true;
                    correctAnswerBonus.IsPersistent = true;
                    correctAnswerBonus.SetExpirationFromDuration(bonusDuration);
                    Debug.Log($"BonusFirestore: Bônus {CORRECT_ANSWER_BONUS} ativado para o usuário {userId} até {DateTimeOffset.FromUnixTimeSeconds(correctAnswerBonus.ExpirationTimestamp).LocalDateTime}");

                    // Verificar se o bônus especial deve ser ativado
                    if (correctAnswerBonus.BonusCount >= BONUS_ACTIVATION_THRESHOLD)
                    {
                        if (specialBonus == null)
                        {
                            // Criar o bônus especial se não existir
                            specialBonus = new BonusType("specialBonus", 0, true, 0, false);
                            bonusList.Add(specialBonus);
                            Debug.Log($"BonusFirestore: Bônus especial criado e ativado para o usuário {userId}");
                        }
                        else
                        {
                            // Ativar o bônus especial
                            specialBonus.IsBonusActive = true;
                            Debug.Log($"BonusFirestore: Bônus especial ativado para o usuário {userId}");
                        }
                    }
                }
                else
                {
                    // Se o correctAnswerBonus não existir, criá-lo com contador = 1
                    correctAnswerBonus = new BonusType(CORRECT_ANSWER_BONUS, 1, true, 0, true);
                    correctAnswerBonus.SetExpirationFromDuration(bonusDuration);
                    bonusList.Add(correctAnswerBonus);
                    Debug.Log($"BonusFirestore: Novo bônus {CORRECT_ANSWER_BONUS} criado com contador 1 e ativado para o usuário {userId}");
                }

                // Garantir que o bônus especial exista
                if (specialBonus == null)
                {
                    specialBonus = new BonusType("specialBonus", 0, false, 0, false);
                    bonusList.Add(specialBonus);
                    Debug.Log($"BonusFirestore: Bônus especial criado (inativo) para o usuário {userId}");
                }
            }
            else
            {
                // Documento não existe, criar o correctAnswerBonus com contador = 1
                BonusType correctAnswerBonus = new BonusType(CORRECT_ANSWER_BONUS, 1, true, 0, true);
                correctAnswerBonus.SetExpirationFromDuration(bonusDuration);
                bonusList.Add(correctAnswerBonus);

                // Também criar o bônus especial (inativo)
                BonusType specialBonus = new BonusType("specialBonus", 0, false, 0, false);
                bonusList.Add(specialBonus);

                Debug.Log($"BonusFirestore: Novos bônus criados para o usuário {userId}");
            }

            // Salvar a lista atualizada no Firestore
            await SaveBonusList(userId, bonusList);

            Debug.Log($"BonusFirestore: Bônus {CORRECT_ANSWER_BONUS} processado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao incrementar bônus: {e.Message}");
        }
    }

    public async Task ActivateSpecialBonus(string userId, float durationInSeconds)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);

            // Verificar se o usuário tem o bônus especial ativo
            BonusType specialBonus = bonusList.FirstOrDefault(b => b.BonusName == "specialBonus");

            if (specialBonus != null && specialBonus.IsBonusActive)
            {
                // Desativar o direito ao bônus especial
                specialBonus.IsBonusActive = false;

                // Criar um novo bônus temporário para representar o bônus especial em uso
                BonusType activeSpecialBonus = bonusList.FirstOrDefault(b => b.BonusName == "activeSpecialBonus");

                if (activeSpecialBonus == null)
                {
                    activeSpecialBonus = new BonusType("activeSpecialBonus", 0, true, 0, true);
                    bonusList.Add(activeSpecialBonus);
                }
                else
                {
                    activeSpecialBonus.IsBonusActive = true;
                    activeSpecialBonus.IsPersistent = true;
                }

                // Configurar a duração do bônus especial ativo
                activeSpecialBonus.SetExpirationFromDuration(durationInSeconds);

                // Salvar as alterações
                await SaveBonusList(userId, bonusList);

                Debug.Log($"BonusFirestore: Bônus especial consumido e ativado por {durationInSeconds} segundos para o usuário {userId}");
                return;
            }

            Debug.LogWarning($"BonusFirestore: Usuário {userId} tentou ativar o bônus especial, mas não tem direito");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao ativar bônus especial: {e.Message}");
        }
    }

    public async Task<int> CountCorrectAnswerBonusesEarned(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return 0;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType correctAnswerBonus = bonusList.FirstOrDefault(b => b.BonusName == CORRECT_ANSWER_BONUS);

            if (correctAnswerBonus != null)
            {
                return correctAnswerBonus.BonusCount;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao contar bônus: {e.Message}");
        }

        return 0;
    }

    // Método para verificar se o bônus especial está disponível
    public async Task<bool> IsSpecialBonusAvailable(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return false;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType specialBonus = bonusList.FirstOrDefault(b => b.BonusName == "specialBonus");

            return specialBonus != null && specialBonus.IsBonusActive;
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao verificar bônus especial: {e.Message}");
        }

        return false;
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