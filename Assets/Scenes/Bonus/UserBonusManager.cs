using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Firestore;

public class UserBonusManager
{
    private const string COLLECTION_NAME = "UserBonus";
    private const string ACTIVE_BONUS_PREFIX = "active_";
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
                                    bonusDict.ContainsKey("IsPersistent") ? Convert.ToBoolean(bonusDict["IsPersistent"]) : false,
                                    bonusDict.ContainsKey("Multiplier") ? Convert.ToInt32(bonusDict["Multiplier"]) : 1
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

    public async Task<BonusType> GetActiveBonusByName(string userId, string bonusName)
    {
        string activeBonusName = $"{ACTIVE_BONUS_PREFIX}{bonusName}";
        List<BonusType> bonusList = await GetUserBonuses(userId);
        return bonusList.FirstOrDefault(b => b.BonusName == activeBonusName && b.IsBonusActive && !b.IsExpired());
    }

    public async Task<List<BonusType>> GetAllActiveBonuses(string userId)
    {
        List<BonusType> bonusList = await GetUserBonuses(userId);
        return bonusList.Where(b =>
            b.BonusName.StartsWith(ACTIVE_BONUS_PREFIX) &&
            b.IsBonusActive &&
            !b.IsExpired()).ToList();
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

                if (autoActivate || (bonusName == "specialBonus" && targetBonus.BonusCount >= 5))
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
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao incrementar contador: {e.Message}");
        }
    }

    public async Task ActivateBonusInGame(string userId, string bonusName, float durationInSeconds, int multiplier)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType bonusToActivate = bonusList.FirstOrDefault(b => b.BonusName == bonusName);

            if (bonusToActivate != null && bonusToActivate.IsBonusActive)
            {
                bonusToActivate.SetExpirationFromDuration(durationInSeconds);
                bonusToActivate.Multiplier = multiplier;
            }
            else
            {
                if (bonusToActivate == null)
                {
                    bonusToActivate = new BonusType(
                        bonusName,
                        0,
                        true,
                        DateTimeOffset.UtcNow.AddSeconds(durationInSeconds).ToUnixTimeSeconds(),
                        false,
                        multiplier
                    );
                    bonusList.Add(bonusToActivate);
                }
                else
                {
                    bonusToActivate.IsBonusActive = true;
                    bonusToActivate.SetExpirationFromDuration(durationInSeconds);
                    bonusToActivate.Multiplier = multiplier;
                }
            }

            await SaveBonusList(userId, bonusList);
            QuestionSceneBonusManager sceneBonusManager = new QuestionSceneBonusManager();
            await sceneBonusManager.ActivateBonus(userId, bonusName, durationInSeconds, multiplier);
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao ativar bônus: {e.Message}");
            throw;
        }
    }

    public async Task ConsumeBonusAndActivate(string userId, string bonusName, float durationInSeconds, int multiplier)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserBonusManager: UserId é nulo ou vazio");
            return;
        }

        try
        {
            List<BonusType> bonusList = await GetUserBonuses(userId);
            BonusType bonusToConsume = bonusList.FirstOrDefault(b => b.BonusName == bonusName);

            if (bonusToConsume != null && bonusToConsume.BonusCount > 0)
            {
                bonusToConsume.BonusCount--;

                if (!bonusToConsume.IsPersistent && bonusToConsume.BonusCount <= 0)
                {
                    bonusToConsume.IsBonusActive = false;
                }

                await SaveBonusList(userId, bonusList);
                QuestionSceneBonusManager sceneBonusManager = new QuestionSceneBonusManager();
                await sceneBonusManager.ActivateBonus(userId, bonusName, durationInSeconds, multiplier);
            }
            else
            {
                Debug.LogWarning($"UserBonusManager: {bonusName} não está disponível para consumo");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"UserBonusManager: Erro ao consumir e ativar bônus: {e.Message}");
            throw;
        }
    }

    #endregion
}