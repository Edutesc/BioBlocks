using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BonusApplicationManager : MonoBehaviour
{
    [Header("Global Settings")]
    [SerializeField] private float updateInterval = 1f;

    [Header("UI Elements")]
    [SerializeField] private GameObject bonusTimerContainer; 
    [SerializeField] private TextMeshProUGUI bonusTimerText; 

    [Header("Bonus Colors")]
    [SerializeField] private Color specialBonusColor = new Color(1f, 1f, 0.7f);
    [SerializeField] private Color listBonusColor = new Color(0.71f, 1f, 0.6f);
    [SerializeField] private Color persistenceBonusColor = new Color(0.7f, 1f, 1f);
    [SerializeField] private Color correctAnswerBonusColor = new Color(0.8f, 0.7f, 0.8f);

    private Dictionary<string, Color> bonusColors = new Dictionary<string, Color>();
    private Dictionary<string, string> bonusDisplayNames = new Dictionary<string, string>();
    private UserBonusManager userBonusManager;
    private QuestionSceneBonusManager questionSceneBonusManager;
    private string userId;
    private float lastFirestoreUpdateTime = 0f;
    private List<BonusInfo> activeBonuses = new List<BonusInfo>();
    public System.Action<int> OnBonusMultiplierUpdated;

    private class BonusInfo
    {
        public string bonusName;
        public float remainingTime;
        public int multiplier;
        public string displayName;
        public Color color;
    }

    private void Awake()
    {
        userBonusManager = new UserBonusManager();
        questionSceneBonusManager = new QuestionSceneBonusManager();

        bonusColors["specialBonus"] = specialBonusColor;
        bonusColors["listCompletionBonus"] = listBonusColor;
        bonusColors["persistenceBonus"] = persistenceBonusColor;
        bonusColors["correctAnswerBonus"] = correctAnswerBonusColor;
        bonusColors["specialBonusPro"] = specialBonusColor;
        bonusColors["listCompletionBonusPro"] = listBonusColor;
        bonusColors["persistenceBonusPro"] = persistenceBonusColor;

        bonusDisplayNames["specialBonus"] = "Bônus XP Triplicada";
        bonusDisplayNames["listCompletionBonus"] = "Bônus XP Triplicada";
        bonusDisplayNames["persistenceBonus"] = "Bônus XP Triplicada";
        bonusDisplayNames["correctAnswerBonus"] = "Bônus XP Dobrada";
        bonusDisplayNames["specialBonusPro"] = "Bônus XP Triplicada";
        bonusDisplayNames["listCompletionBonusPro"] = "Bônus XP Triplicada";
        bonusDisplayNames["persistenceBonusPro"] = "Bônus XP Triplicada";
    }

    private void Start()
    {
        if (bonusTimerText == null)
        {
            bonusTimerText = GameObject.Find("BonusTimerText")?.GetComponent<TextMeshProUGUI>();
        }

        if (bonusTimerContainer == null && bonusTimerText != null)
        {
            bonusTimerContainer = bonusTimerText.transform.parent.gameObject;
        }

        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(false);
        }

        InitializeAndFetchActiveBonuses();
    }

    private void OnEnable()
    {
        if (isInitialized)
        {
            RefreshActiveBonuses();
        }
    }

    private void OnDisable()
    {
        if (isInitialized)
        {
            UpdateBonusTimestampsInFirestore();
        }
    }

    private bool isInitialized = false;

    private async void InitializeAndFetchActiveBonuses()
    {
        if (UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            userId = UserDataStore.CurrentUserData.UserId;
            await FetchAndDisplayAllActiveBonuses();
            StartCoroutine(UpdateTimersCoroutine());
            isInitialized = true;
        }
        else
        {
            Debug.LogWarning("BonusApplicationManager: Usuário não está logado");
        }
    }

    public async void RefreshActiveBonuses()
    {
        if (isInitialized)
        {
            await FetchAndDisplayAllActiveBonuses();
        }
    }

    private async Task FetchAndDisplayAllActiveBonuses()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusApplicationManager: UserId não definido");
            return;
        }

        try
        {
            activeBonuses.Clear();
            List<Dictionary<string, object>> activeSceneBonuses = await questionSceneBonusManager.GetActiveBonuses(userId);
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            foreach (var bonusDict in activeSceneBonuses)
            {
                if (bonusDict.ContainsKey("BonusType") &&
                    bonusDict.ContainsKey("ExpirationTimestamp") &&
                    bonusDict.ContainsKey("BonusMultiplier"))
                {
                    string bonusType = bonusDict["BonusType"].ToString();
                    long expirationTimestamp = Convert.ToInt64(bonusDict["ExpirationTimestamp"]);
                    int multiplier = Convert.ToInt32(bonusDict["BonusMultiplier"]);

                    if (expirationTimestamp > currentTimestamp)
                    {
                        float remainingTime = expirationTimestamp - currentTimestamp;
                        Color bonusColor = bonusColors.ContainsKey(bonusType) ? bonusColors[bonusType] : Color.white;
                        string displayName = bonusDisplayNames.ContainsKey(bonusType) ? bonusDisplayNames[bonusType] : bonusType;

                        activeBonuses.Add(new BonusInfo
                        {
                            bonusName = bonusType,
                            remainingTime = remainingTime,
                            multiplier = multiplier,
                            displayName = displayName,
                            color = bonusColor
                        });
                    }
                }
            }

            UpdateBonusUI();
            OnBonusMultiplierUpdated?.Invoke(GetTotalMultiplier());
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusApplicationManager: Erro ao buscar bônus ativos: {e.Message}");
        }
    }

    private void UpdateBonusUI()
    {
        if (activeBonuses.Count == 0)
        {
            if (bonusTimerContainer != null)
            {
                bonusTimerContainer.SetActive(false);
            }
            return;
        }

        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(true);
        }

        int totalMultiplier = 1;
        foreach (var bonus in activeBonuses)
        {
            totalMultiplier *= bonus.multiplier;
        }

        BonusInfo earliestExpiringBonus = activeBonuses.OrderBy(b => b.remainingTime).FirstOrDefault();

        if (earliestExpiringBonus != null && bonusTimerText != null)
        {
            int minutes = Mathf.FloorToInt(earliestExpiringBonus.remainingTime / 60);
            int seconds = Mathf.FloorToInt(earliestExpiringBonus.remainingTime % 60);

            if (activeBonuses.Count > 1)
            {
                bonusTimerText.text = $"Bônus Acumulados (x{totalMultiplier}): {minutes:00}:{seconds:00} minutos";
            }
            else
            {
                string bonusName = earliestExpiringBonus.displayName;
                if (bonusName.StartsWith("Bônus "))
                {
                    bonusName = bonusName.Substring(6);
                }

                bonusTimerText.text = $"{bonusName}: {minutes:00}:{seconds:00} minutos";
            }

            Image backgroundImage = bonusTimerContainer.GetComponent<Image>();
            if (backgroundImage != null)
            {
                if (activeBonuses.Count > 1)
                {
                    backgroundImage.color = new Color(1f, 0.76f, 0.1f);
                }
                else
                {
                    backgroundImage.color = earliestExpiringBonus.color;
                }
            }
        }
    }

    private IEnumerator UpdateTimersCoroutine()
    {
        while (true)
        {
            bool anyBonusActive = false;
            List<BonusInfo> expiredBonuses = new List<BonusInfo>();

            foreach (var bonus in activeBonuses)
            {
                bonus.remainingTime -= updateInterval;

                if (bonus.remainingTime <= 0)
                {
                    expiredBonuses.Add(bonus);
                }
                else
                {
                    anyBonusActive = true;
                }
            }

            foreach (var expiredBonus in expiredBonuses)
            {
                activeBonuses.Remove(expiredBonus);
            }

            UpdateBonusUI();

            if (expiredBonuses.Count > 0)
            {
                OnBonusMultiplierUpdated?.Invoke(GetTotalMultiplier());
            }

            if (Time.time - lastFirestoreUpdateTime > 30f)
            {
                UpdateBonusTimestampsInFirestore();
                lastFirestoreUpdateTime = Time.time;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    private async void UpdateBonusTimestampsInFirestore()
    {
        if (string.IsNullOrEmpty(userId) || activeBonuses.Count == 0)
        {
            return;
        }

        try
        {
            List<BonusType> bonusList = await userBonusManager.GetUserBonuses(userId);
            bool hasUserBonusChanges = false;

            foreach (var bonus in activeBonuses)
            {
                BonusType bonusToUpdate = bonusList.FirstOrDefault(b => b.BonusName == bonus.bonusName && b.IsBonusActive);
                if (bonusToUpdate != null)
                {
                    bonusToUpdate.SetExpirationFromDuration(bonus.remainingTime);
                    hasUserBonusChanges = true;
                }
            }

            if (hasUserBonusChanges)
            {
                await userBonusManager.SaveBonusList(userId, bonusList);
            }

            List<string> questionSceneBonusTypes = activeBonuses
                .Where(b => b.bonusName == "correctAnswerBonus")
                .Select(b => b.bonusName)
                .ToList();

            foreach (string bonusType in questionSceneBonusTypes)
            {
                BonusInfo bonusInfo = activeBonuses.FirstOrDefault(b => b.bonusName == bonusType);
                if (bonusInfo != null)
                {
                    await questionSceneBonusManager.UpdateExpirationTimestamp(userId, bonusInfo.remainingTime);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusApplicationManager: Erro ao atualizar timestamps: {e.Message}");
        }
    }

    public int GetTotalMultiplier()
    {
        if (activeBonuses.Count == 0)
        {
            return 1;
        }

        int totalMultiplier = 0;

        foreach (var bonus in activeBonuses)
        {
            totalMultiplier += bonus.multiplier;
        }

        return totalMultiplier;
    }

    public int ApplyTotalBonus(int baseValue)
    {
        return baseValue * GetTotalMultiplier();
    }

    public bool IsAnyBonusActive()
    {
        return activeBonuses.Count > 0;
    }

    public bool IsBonusActive(string bonusType)
    {
        return activeBonuses.Any(b => b.bonusName == bonusType);
    }

    public void AddActiveBonus(string bonusType, float durationInSeconds, int multiplier)
    {
        if (string.IsNullOrEmpty(bonusType) || durationInSeconds <= 0 || multiplier <= 0)
        {
            return;
        }

        Color bonusColor = bonusColors.ContainsKey(bonusType) ? bonusColors[bonusType] : Color.white;
        string displayName = bonusDisplayNames.ContainsKey(bonusType) ? bonusDisplayNames[bonusType] : bonusType;

        activeBonuses.Add(new BonusInfo
        {
            bonusName = bonusType,
            remainingTime = durationInSeconds,
            multiplier = multiplier,
            displayName = displayName,
            color = bonusColor
        });

        UpdateBonusUI();
        OnBonusMultiplierUpdated?.Invoke(GetTotalMultiplier());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        if (isInitialized)
        {
            UpdateBonusTimestampsInFirestore();
        }
    }
}