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
    [SerializeField] private GameObject bonusTimerContainer; // O container laranja existente
    [SerializeField] private TextMeshProUGUI bonusTimerText; // O texto existente
    
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
        // Verificar e procurar os elementos de UI se não estiverem atribuídos
        if (bonusTimerText == null)
        {
            bonusTimerText = GameObject.Find("BonusTimerText")?.GetComponent<TextMeshProUGUI>();
        }
        
        if (bonusTimerContainer == null && bonusTimerText != null)
        {
            bonusTimerContainer = bonusTimerText.transform.parent.gameObject;
        }
        
        // Ocultar container de bônus inicialmente
        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(false);
        }
        
        // Inicializar e buscar bônus ativos
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
    
    // Método público para ser chamado pelo QuestionBonusManager
    public async void RefreshActiveBonuses()
    {
        if (isInitialized)
        {
            await FetchAndDisplayAllActiveBonuses();
        }
    }
    
    // Método que combina os bônus de ambas as fontes
    private async Task FetchAndDisplayAllActiveBonuses()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusApplicationManager: UserId não definido");
            return;
        }

        try
        {
            // Limpar a lista de bônus ativos
            activeBonuses.Clear();
            
            // Obter bônus da QuestionSceneBonus (bônus ganhos por respostas corretas)
            List<Dictionary<string, object>> questionSceneBonuses = await questionSceneBonusManager.GetActiveBonuses(userId);
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            foreach (var bonusDict in questionSceneBonuses)
            {
                if (bonusDict.ContainsKey("BonusType") && 
                    bonusDict.ContainsKey("ExpirationTimestamp") && 
                    bonusDict.ContainsKey("BonusMultiplier"))
                {
                    string bonusType = bonusDict["BonusType"].ToString();
                    long expirationTimestamp = Convert.ToInt64(bonusDict["ExpirationTimestamp"]);
                    int multiplier = Convert.ToInt32(bonusDict["BonusMultiplier"]);
                    
                    // Verificar se ainda não expirou
                    if (expirationTimestamp > currentTimestamp)
                    {
                        float remainingTime = expirationTimestamp - currentTimestamp;
                        Color bonusColor = bonusColors.ContainsKey(bonusType) ? bonusColors[bonusType] : Color.white;
                        string displayName = bonusDisplayNames.ContainsKey(bonusType) ? bonusDisplayNames[bonusType] : bonusType;
                        
                        activeBonuses.Add(new BonusInfo {
                            bonusName = bonusType,
                            remainingTime = remainingTime,
                            multiplier = multiplier,
                            displayName = displayName,
                            color = bonusColor
                        });
                    }
                }
            }
            
            // Obter bônus de UserBonus (ativados pelo usuário na BonusScene)
            List<BonusType> userBonuses = await userBonusManager.GetUserBonuses(userId);
            foreach (var bonus in userBonuses)
            {
                if (bonus.IsBonusActive && !bonus.IsExpired())
                {
                    string bonusName = bonus.BonusName;
                    float remainingTime = bonus.GetRemainingSeconds();
                    Color bonusColor = bonusColors.ContainsKey(bonusName) ? bonusColors[bonusName] : Color.white;
                    string displayName = bonusDisplayNames.ContainsKey(bonusName) ? bonusDisplayNames[bonusName] : bonusName;
                    
                    activeBonuses.Add(new BonusInfo {
                        bonusName = bonusName,
                        remainingTime = remainingTime,
                        multiplier = bonus.Multiplier,
                        displayName = displayName,
                        color = bonusColor
                    });
                }
            }
            
            // Atualizar a UI
            UpdateBonusUI();
            
            // Notificar sobre a mudança no multiplicador
            OnBonusMultiplierUpdated?.Invoke(GetTotalMultiplier());
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusApplicationManager: Erro ao buscar bônus ativos: {e.Message}");
        }
    }
    
    // Método para atualizar a UI com base nos bônus ativos
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
        
        // Mostrar o container
        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(true);
        }
        
        // Calcular o multiplicador total
        int totalMultiplier = 1;
        foreach (var bonus in activeBonuses)
        {
            totalMultiplier *= bonus.multiplier;
        }
        
        // Encontrar o bônus que vai expirar primeiro
        BonusInfo earliestExpiringBonus = activeBonuses.OrderBy(b => b.remainingTime).FirstOrDefault();
        
        if (earliestExpiringBonus != null && bonusTimerText != null)
        {
            // Calcular minutos e segundos
            int minutes = Mathf.FloorToInt(earliestExpiringBonus.remainingTime / 60);
            int seconds = Mathf.FloorToInt(earliestExpiringBonus.remainingTime % 60);
            
            // Formatar o texto baseado no número de bônus ativos
            if (activeBonuses.Count > 1)
            {
                bonusTimerText.text = $"Bônus Acumulados (x{totalMultiplier}): {minutes:00}:{seconds:00} minutos";
            }
            else
            {
                // Extrair o nome do bônus sem o "Bônus" prefixo para economizar espaço
                string bonusName = earliestExpiringBonus.displayName;
                if (bonusName.StartsWith("Bônus "))
                {
                    bonusName = bonusName.Substring(6);
                }
                
                bonusTimerText.text = $"{bonusName}: {minutes:00}:{seconds:00} minutos";
            }
            
            // Atualizar a cor do fundo baseado no tipo de bônus
            // Se houver múltiplos bônus, usar uma cor especial para indicar combinação
            Image backgroundImage = bonusTimerContainer.GetComponent<Image>();
            if (backgroundImage != null)
            {
                if (activeBonuses.Count > 1)
                {
                    // Mistura de cores ou usar uma cor específica para bônus acumulados
                    backgroundImage.color = new Color(0.8f, 0.3f, 0.8f); // Roxo para múltiplos bônus
                }
                else
                {
                    backgroundImage.color = earliestExpiringBonus.color;
                }
            }
        }
    }
    
    // Método para decrementar o tempo dos bônus ativos
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
            
            // Remover bônus expirados
            foreach (var expiredBonus in expiredBonuses)
            {
                activeBonuses.Remove(expiredBonus);
            }
            
            // Atualizar a UI
            UpdateBonusUI();
            
            // Notificar sobre mudança no multiplicador quando algum bônus expirar
            if (expiredBonuses.Count > 0)
            {
                OnBonusMultiplierUpdated?.Invoke(GetTotalMultiplier());
            }
            
            // Atualizar Firestore periodicamente
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
            // Atualizar timestamps de UserBonus
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
            
            // Atualizar timestamps em QuestionSceneBonus
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
    
    // Métodos públicos úteis para outras classes
    
    public int GetTotalMultiplier()
    {
        if (activeBonuses.Count == 0)
        {
            return 1; 
        }
        
        int totalMultiplier = 1;
        foreach (var bonus in activeBonuses)
        {
            totalMultiplier *= bonus.multiplier;
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
    
    // Método para adicionar um novo bônus (chamado pelo QuestionBonusManager)
    public void AddActiveBonus(string bonusType, float durationInSeconds, int multiplier)
    {
        if (string.IsNullOrEmpty(bonusType) || durationInSeconds <= 0 || multiplier <= 0)
        {
            return;
        }
        
        Color bonusColor = bonusColors.ContainsKey(bonusType) ? bonusColors[bonusType] : Color.white;
        string displayName = bonusDisplayNames.ContainsKey(bonusType) ? bonusDisplayNames[bonusType] : bonusType;
        
        activeBonuses.Add(new BonusInfo {
            bonusName = bonusType,
            remainingTime = durationInSeconds,
            multiplier = multiplier,
            displayName = displayName,
            color = bonusColor
        });
        
        // Atualizar a UI
        UpdateBonusUI();
        
        // Notificar sobre a mudança no multiplicador
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