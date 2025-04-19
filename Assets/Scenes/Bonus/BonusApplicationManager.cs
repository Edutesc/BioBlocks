using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BonusApplicationManager : MonoBehaviour
{
    [Header("Global Settings")]
    [SerializeField] private float updateInterval = 1f;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject bonusTimersContainer;
    [SerializeField] private GameObject bonusTimerPrefab;
    [SerializeField] private Transform bonusTimersLayout;
    
    [Header("Bonus Colors")]
    [SerializeField] private Color specialBonusColor = new Color(1f, 0.5f, 0.2f);
    [SerializeField] private Color listBonusColor = new Color(0.2f, 0.8f, 0.3f);
    [SerializeField] private Color persistenceBonusColor = new Color(0.2f, 0.5f, 1f);
    
    // Mapeamento de cores
    private Dictionary<string, Color> bonusColors = new Dictionary<string, Color>();
    
    // Mapeamento de nomes amigáveis
    private Dictionary<string, string> bonusDisplayNames = new Dictionary<string, string>
    {
        { "specialBonus", "XP Triplicada" },
        { "listCompletionBonus", "XP Duplicada (Listas)" },
        { "persistenceBonus", "XP Duplicada (Incansável)" }
    };

    private UserBonusManager userBonusManager;
    private string userId;
    private Dictionary<string, ActiveBonusUI> activeBonusUIs = new Dictionary<string, ActiveBonusUI>();
    private bool isInitialized = false;
    private float lastFirestoreUpdateTime = 0f;
    
    private class ActiveBonusUI
    {
        public GameObject uiObject;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI nameText;
        public Image backgroundImage;
        public float remainingTime;
        public int multiplier;
        public string bonusName;
    }

    private void Awake()
    {
        userBonusManager = new UserBonusManager();
        
        // Configurar mapeamento de cores
        bonusColors["specialBonus"] = specialBonusColor;
        bonusColors["listCompletionBonus"] = listBonusColor;
        bonusColors["persistenceBonus"] = persistenceBonusColor;
    }
    
    private void Start()
    {
        if (bonusTimersContainer != null)
        {
            bonusTimersContainer.SetActive(false);
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

    private async void InitializeAndFetchActiveBonuses()
    {
        if (UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            userId = UserDataStore.CurrentUserData.UserId;
            await FetchAndDisplayActiveBonuses();
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
            await FetchAndDisplayActiveBonuses();
        }
    }

    private async System.Threading.Tasks.Task FetchAndDisplayActiveBonuses()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusApplicationManager: UserId não definido");
            return;
        }

        try
        {
            List<BonusType> activeBonuses = await userBonusManager.GetAllActiveBonuses(userId);
            
            // Limpar UI existente
            ClearActiveBonusUI();
            
            if (activeBonuses.Count > 0)
            {
                if (bonusTimersContainer != null)
                {
                    bonusTimersContainer.SetActive(true);
                }
                
                foreach (BonusType bonus in activeBonuses)
                {
                    if (!bonus.IsExpired())
                    {
                        AddBonusToUI(bonus);
                    }
                }
            }
            else
            {
                if (bonusTimersContainer != null)
                {
                    bonusTimersContainer.SetActive(false);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusApplicationManager: Erro ao buscar bônus ativos: {e.Message}");
        }
    }
    
    private void ClearActiveBonusUI()
    {
        foreach (var bonusUI in activeBonusUIs.Values)
        {
            if (bonusUI.uiObject != null)
            {
                Destroy(bonusUI.uiObject);
            }
        }
        
        activeBonusUIs.Clear();
    }
    
    private void AddBonusToUI(BonusType bonus)
    {
        if (bonusTimerPrefab == null || bonusTimersLayout == null)
        {
            Debug.LogError("BonusApplicationManager: Prefab ou layout não configurados");
            return;
        }
        
        string baseBonusName = bonus.BonusName.Replace("active_", "");
        string displayName = bonusDisplayNames.ContainsKey(baseBonusName) 
            ? bonusDisplayNames[baseBonusName] 
            : baseBonusName;
            
        GameObject bonusTimerObj = Instantiate(bonusTimerPrefab, bonusTimersLayout);
        
        TextMeshProUGUI nameText = bonusTimerObj.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timerText = bonusTimerObj.transform.Find("TimerText")?.GetComponent<TextMeshProUGUI>();
        Image background = bonusTimerObj.GetComponent<Image>();
        
        if (nameText != null)
        {
            nameText.text = displayName;
        }
        
        if (background != null && bonusColors.ContainsKey(baseBonusName))
        {
            background.color = bonusColors[baseBonusName];
        }
        
        float remainingTime = bonus.GetRemainingSeconds();
        
        ActiveBonusUI activeBonusUI = new ActiveBonusUI
        {
            uiObject = bonusTimerObj,
            timerText = timerText,
            nameText = nameText,
            backgroundImage = background,
            remainingTime = remainingTime,
            multiplier = bonus.Multiplier,
            bonusName = bonus.BonusName
        };
        
        activeBonusUIs[bonus.BonusName] = activeBonusUI;
        UpdateTimerText(activeBonusUI);
    }
    
    private IEnumerator UpdateTimersCoroutine()
    {
        while (true)
        {
            bool anyBonusActive = false;
            List<string> expiredBonuses = new List<string>();
            
            foreach (var bonusEntry in activeBonusUIs)
            {
                ActiveBonusUI bonusUI = bonusEntry.Value;
                bonusUI.remainingTime -= updateInterval;
                
                if (bonusUI.remainingTime <= 0)
                {
                    expiredBonuses.Add(bonusEntry.Key);
                }
                else
                {
                    anyBonusActive = true;
                    UpdateTimerText(bonusUI);
                }
            }
            
            // Remover bônus expirados
            foreach (string expiredBonus in expiredBonuses)
            {
                if (activeBonusUIs.TryGetValue(expiredBonus, out ActiveBonusUI bonusUI))
                {
                    if (bonusUI.uiObject != null)
                    {
                        Destroy(bonusUI.uiObject);
                    }
                    
                    activeBonusUIs.Remove(expiredBonus);
                }
            }
            
            // Atualizar Firestore periodicamente
            if (Time.time - lastFirestoreUpdateTime > 30f)
            {
                UpdateBonusTimestampsInFirestore();
                lastFirestoreUpdateTime = Time.time;
            }
            
            // Ocultar container se não houver bônus ativos
            if (!anyBonusActive && bonusTimersContainer != null)
            {
                bonusTimersContainer.SetActive(false);
            }
            
            yield return new WaitForSeconds(updateInterval);
        }
    }
    
    private void UpdateTimerText(ActiveBonusUI bonusUI)
    {
        if (bonusUI.timerText != null)
        {
            int minutes = Mathf.FloorToInt(bonusUI.remainingTime / 60);
            int seconds = Mathf.FloorToInt(bonusUI.remainingTime % 60);
            bonusUI.timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    private async void UpdateBonusTimestampsInFirestore()
    {
        if (string.IsNullOrEmpty(userId) || activeBonusUIs.Count == 0)
        {
            return;
        }
        
        try
        {
            List<BonusType> bonusList = await userBonusManager.GetUserBonuses(userId);
            bool hasChanges = false;
            
            foreach (var bonusEntry in activeBonusUIs)
            {
                string bonusName = bonusEntry.Key;
                ActiveBonusUI bonusUI = bonusEntry.Value;
                
                BonusType bonusToUpdate = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
                if (bonusToUpdate != null && bonusToUpdate.GetRemainingSeconds() != bonusUI.remainingTime)
                {
                    bonusToUpdate.SetExpirationFromDuration(bonusUI.remainingTime);
                    hasChanges = true;
                }
            }
            
            if (hasChanges)
            {
                await userBonusManager.SaveBonusList(userId, bonusList);
                Debug.Log("BonusApplicationManager: Timestamps de bônus atualizados no Firestore");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusApplicationManager: Erro ao atualizar timestamps: {e.Message}");
        }
    }
    
    public int GetTotalMultiplier()
    {
        if (activeBonusUIs.Count == 0)
        {
            return 1; // Multiplicador padrão
        }
        
        int totalMultiplier = 1;
        
        foreach (var bonusUI in activeBonusUIs.Values)
        {
            totalMultiplier *= bonusUI.multiplier;
        }
        
        return totalMultiplier;
    }
    
    public int ApplyTotalBonus(int baseValue)
    {
        return baseValue * GetTotalMultiplier();
    }
    
    public bool IsAnyBonusActive()
    {
        return activeBonusUIs.Count > 0;
    }
    
    public bool IsBonusActive(string bonusType)
    {
        string activeBonusName = $"active_{bonusType}";
        return activeBonusUIs.ContainsKey(activeBonusName);
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

