using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using QuestionSystem;

public class QuestionBonusManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestionScoreManager scoreManager;
    [SerializeField] private QuestionAnswerManager answerManager;
    [SerializeField] private QuestionCanvasGroupManager canvasGroupManager;
    [SerializeField] private QuestionBottomUIManager questionBottomUIManager;

    [Header("UI Components")]
    [SerializeField] private QuestionBonusUIFeedback questionBonusUIFeedback;
    [SerializeField] private TextMeshProUGUI bonusTimerText;
    [SerializeField] private GameObject bonusTimerContainer;

    [Header("Bonus Configuration")]
    [SerializeField] private int consecutiveCorrectAnswersNeeded = 5;
    [SerializeField] private float bonusDuration = 600f; // 10 minutos em segundos

    private int combinedMultiplier = 1;
    private List<Dictionary<string, object>> activeBonuses = new List<Dictionary<string, object>>();
    private int consecutiveCorrectAnswers = 0;
    private bool isBonusActive = false;
    private float currentBonusTime = 0f;
    private Coroutine bonusTimerCoroutine = null;
    private QuestionSceneBonusManager bonusManager;

    private void Start()
    {
        // Inicializar o QuestionSceneBonusManager
        bonusManager = new QuestionSceneBonusManager();

        // Validar componentes no início para garantir que tudo esteja configurado corretamente
        if (!ValidateComponents())
        {
            Debug.LogError("QuestionBonusManager: Falha na validação dos componentes necessários.");
            return;
        }

        // Configurar o container do timer após a validação
        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(false);
            Debug.Log("Timer container inicialmente desativado");
        }
        else if (bonusTimerText != null)
        {
            bonusTimerText.gameObject.SetActive(false);
            Debug.Log("Timer text inicialmente desativado");
        }

        // Configurar event listeners
        if (answerManager != null)
        {
            answerManager.OnAnswerSelected += CheckAnswer;
        }

        if (questionBottomUIManager != null)
        {
            questionBottomUIManager.OnExitButtonClicked += HideBonusFeedback;
            questionBottomUIManager.OnNextButtonClicked += HideBonusFeedback;
        }
        else
        {
            Debug.LogWarning("QuestionBonusManager: BottomUIManager não encontrado. O feedback de bônus não será escondido ao navegar.");
        }

        QuestionManager questionManager = FindFirstObjectByType<QuestionManager>();
        if (questionManager == null)
        {
            Debug.LogWarning("QuestionManager não encontrado");
        }

        // Iniciar a verificação de bônus ativos
        StartCoroutine(InitCheckForBonus());
    }

    private bool ValidateComponents()
    {
        if (questionBonusUIFeedback == null)
        {
            questionBonusUIFeedback = FindFirstObjectByType<QuestionBonusUIFeedback>();
            if (questionBonusUIFeedback == null)
            {
                Debug.LogError("QuestionBonusManager: QuestionBonusUIFeedback não encontrado. Por favor, adicione-o à cena.");
                Debug.LogWarning("QuestionBonusManager: Você pode criar um GameObject com o script QuestionBonusUIFeedback.");
                return false;
            }
        }

        if (bonusTimerText == null)
        {
            // Primeiro, tentar encontrar pelo nome original
            bonusTimerText = GameObject.Find("BonusCorrectAnswerTimer")?.GetComponent<TextMeshProUGUI>();

            // Se não encontrar, tentar nomes alternativos
            if (bonusTimerText == null)
            {
                bonusTimerText = GameObject.Find("BonusTimer")?.GetComponent<TextMeshProUGUI>();
            }

            if (bonusTimerText == null)
            {
                // Buscar qualquer TextMeshProUGUI que tenha 'bonus' e 'timer' no nome
                TextMeshProUGUI[] allTexts = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var text in allTexts)
                {
                    if (text.name.ToLower().Contains("bonus") && text.name.ToLower().Contains("timer"))
                    {
                        bonusTimerText = text;
                        break;
                    }
                }
            }

            if (bonusTimerText == null)
            {
                Debug.LogError("QuestionBonusManager: Não foi possível encontrar o TextMeshProUGUI do timer de bônus.");
                Debug.LogWarning("QuestionBonusManager: Tente adicionar um Text com um nome como 'BonusTimer' ou 'BonusCorrectAnswerTimer'.");
                return false;
            }
            else
            {
                Debug.Log($"Timer de bônus encontrado: {bonusTimerText.name}");
            }
        }

        if (bonusTimerContainer == null)
        {
            bonusTimerContainer = bonusTimerText?.transform.parent.gameObject;
            if (bonusTimerContainer == null)
            {
                bonusTimerContainer = bonusTimerText?.gameObject;
            }

            if (bonusTimerContainer != null)
            {
                Debug.Log($"Container do timer de bônus encontrado: {bonusTimerContainer.name}");
            }
        }

        if (scoreManager == null)
        {
            scoreManager = FindFirstObjectByType<QuestionScoreManager>();
            if (scoreManager == null)
            {
                Debug.LogError("QuestionBonusManager: QuestionScoreManager não encontrado.");
                return false;
            }
        }

        if (answerManager == null)
        {
            answerManager = FindFirstObjectByType<QuestionAnswerManager>();
            if (answerManager == null)
            {
                Debug.LogError("QuestionBonusManager: QuestionAnswerManager não encontrado.");
                return false;
            }
        }

        if (canvasGroupManager == null)
        {
            canvasGroupManager = FindFirstObjectByType<QuestionCanvasGroupManager>();
            if (canvasGroupManager == null)
            {
                Debug.LogWarning("QuestionBonusManager: QuestionCanvasGroupManager não encontrado. Feedback de bônus pode não ser exibido corretamente.");
            }
        }

        if (questionBottomUIManager == null)
        {
            questionBottomUIManager = FindFirstObjectByType<QuestionBottomUIManager>();
            if (questionBottomUIManager == null)
            {
                Debug.LogWarning("QuestionBonusManager: BottomUIManager não encontrado. O feedback de bônus não será escondido automaticamente ao navegar.");
            }
        }

        CanvasGroup feedbackCanvasGroup = questionBonusUIFeedback.GetComponent<CanvasGroup>();
        if (feedbackCanvasGroup == null)
        {
            Debug.LogWarning("QuestionBonusUIFeedback não tem um componente CanvasGroup. Adicionando automaticamente.");
            feedbackCanvasGroup = questionBonusUIFeedback.gameObject.AddComponent<CanvasGroup>();
        }

        return true;
    }

    private IEnumerator InitCheckForBonus()
    {
        yield return new WaitForSeconds(0.5f);

        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            yield break;
        }

        string userId = UserDataStore.CurrentUserData.UserId;
        CheckForActiveBonuses(userId);
    }

    private async void CheckForActiveBonuses(string userId)
    {
        try
        {
            bool hasBonus = await bonusManager.HasAnyActiveBonus(userId);

            if (hasBonus)
            {
                // Obter a lista de bônus ativos
                activeBonuses = await bonusManager.GetActiveBonuses(userId);

                if (activeBonuses.Count > 0)
                {
                    // Calcular o multiplicador combinado
                    combinedMultiplier = await bonusManager.GetCombinedMultiplier(userId);

                    // Obter o tempo restante
                    float remainingTime = await bonusManager.GetRemainingTime(userId);

                    if (remainingTime > 0)
                    {
                        Debug.Log($"Bonus ativo encontrado com {remainingTime} segundos restantes. Ativando UI...");
                        isBonusActive = true;
                        ActivateBonusWithRemainingTime(remainingTime);
                    }
                    else
                    {
                        Debug.Log("Bonus expirado. Desativando.");
                        isBonusActive = false;
                        DeactivateBonus();
                    }
                }
                else
                {
                    Debug.Log("Nenhum bônus ativo encontrado após verificação.");
                    isBonusActive = false;
                }
            }
            else
            {
                Debug.Log("Nenhum bônus ativo no Firestore.");
                isBonusActive = false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao verificar bônus: {e.Message}");
        }
    }

    private void ActivateBonusWithRemainingTime(float remainingSeconds)
    {
        isBonusActive = true;
        currentBonusTime = remainingSeconds;

        Debug.Log($"QuestionBonusManager: Ativando bônus com {remainingSeconds} segundos restantes");

        // Garantir que o contêiner do timer esteja visível
        EnsureTimerUIIsVisible();

        if (bonusTimerCoroutine != null)
        {
            StopCoroutine(bonusTimerCoroutine);
        }
        bonusTimerCoroutine = StartCoroutine(BonusTimerCoroutine());

        // Atualizar o display imediatamente
        UpdateTimerDisplay();
    }

    private void EnsureTimerUIIsVisible()
    {
        if (bonusTimerContainer == null && bonusTimerText != null)
        {
            bonusTimerContainer = bonusTimerText.transform.parent.gameObject;
            if (bonusTimerContainer == null)
            {
                bonusTimerContainer = bonusTimerText.gameObject;
            }
        }

        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(true);
            Debug.Log("Timer container ativado");
        }
        else if (bonusTimerText != null)
        {
            bonusTimerText.gameObject.SetActive(true);
            Debug.Log("Timer text ativado diretamente");
        }
        else
        {
            Debug.LogError("Nenhum elemento de UI do timer encontrado");
        }
    }

    private async void ActivateBonus()
    {
        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            Debug.LogError("QuestionBonusManager: Usuário não está logado");
            return;
        }

        string userId = UserDataStore.CurrentUserData.UserId;

        try
        {
            // Ativar o bônus de respostas corretas (2x)
            await bonusManager.ActivateBonus(userId, "correctAnswerBonus", bonusDuration, 2);

            // Atualizar a lista de bônus ativos
            activeBonuses = await bonusManager.GetActiveBonuses(userId);

            // Recalcular o multiplicador
            combinedMultiplier = await bonusManager.GetCombinedMultiplier(userId);

            isBonusActive = true;
            currentBonusTime = bonusDuration;

            // Mostrar feedback visual
            if (canvasGroupManager != null)
            {
                canvasGroupManager.ShowBonusFeedback(true);

                if (questionBonusUIFeedback != null)
                {
                    questionBonusUIFeedback.ShowBonusActivatedFeedback();
                }
            }
            else
            {
                if (questionBonusUIFeedback != null)
                {
                    questionBonusUIFeedback.gameObject.SetActive(true);
                    questionBonusUIFeedback.ShowBonusActivatedFeedback();
                }
                else
                {
                    Debug.LogError("questionBonusUIFeedback é null no momento de ativar!");
                }
            }

            // Mostrar o timer
            if (bonusTimerContainer != null)
            {
                bonusTimerContainer.SetActive(true);
            }
            else if (bonusTimerText != null)
            {
                bonusTimerText.gameObject.SetActive(true);
            }

            if (bonusTimerCoroutine != null)
            {
                StopCoroutine(bonusTimerCoroutine);
            }
            bonusTimerCoroutine = StartCoroutine(BonusTimerCoroutine());
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao ativar bônus: {e.Message}");
        }
    }

    private async void DeactivateBonus()
    {
        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            Debug.LogError("QuestionBonusManager: Usuário não está logado");
            return;
        }

        string userId = UserDataStore.CurrentUserData.UserId;

        try
        {
            await bonusManager.DeactivateAllBonuses(userId);

            isBonusActive = false;
            activeBonuses.Clear();
            combinedMultiplier = 1;

            // Esconder o feedback
            if (canvasGroupManager != null)
            {
                canvasGroupManager.ShowBonusFeedback(false);
            }

            // Esconder o timer
            if (bonusTimerContainer != null)
            {
                bonusTimerContainer.SetActive(false);
            }
            else if (bonusTimerText != null)
            {
                bonusTimerText.gameObject.SetActive(false);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao desativar bônus: {e.Message}");
        }
    }

    private IEnumerator BonusTimerCoroutine()
    {
        float lastUpdateTime = currentBonusTime;

        // Já atualizar no início
        UpdateTimerDisplay();

        while (currentBonusTime > 0)
        {
            // Verificar se o timer está visível e se o texto está correto
            if (bonusTimerContainer != null && !bonusTimerContainer.activeSelf)
            {
                Debug.LogWarning("Timer container não está ativo. Reativando...");
                bonusTimerContainer.SetActive(true);
            }

            yield return new WaitForSeconds(1f);
            currentBonusTime -= 1f;

            // Atualizar o texto a cada segundo
            UpdateTimerDisplay();

            // Atualizar no Firestore periodicamente
            if (lastUpdateTime - currentBonusTime >= 30f || currentBonusTime <= 10f)
            {
                lastUpdateTime = currentBonusTime;
                UpdateBonusExpirationInFirestore();
            }
        }

        Debug.Log("Timer expirado. Desativando bonus.");
        DeactivateBonus();
    }

    private async void UpdateBonusExpirationInFirestore()
    {
        if (!isBonusActive || UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            return;
        }

        string userId = UserDataStore.CurrentUserData.UserId;

        try
        {
            await bonusManager.UpdateBonusExpirations(userId, currentBonusTime);
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao atualizar timestamp de expiração: {e.Message}");
        }
    }

    private void UpdateTimerDisplay()
    {
        if (bonusTimerText == null)
        {
            Debug.LogError("QuestionBonusManager: bonusTimerText é null ao tentar atualizar o display");
            return;
        }

        int minutes = Mathf.FloorToInt(currentBonusTime / 60);
        int seconds = Mathf.FloorToInt(currentBonusTime % 60);

        string displayText;

        if (activeBonuses.Count > 1)
        {
            // Múltiplos bônus ativos
            displayText = $"Bônus Acumulados (x{combinedMultiplier}): {minutes:00}:{seconds:00}";
        }
        else if (activeBonuses.Count == 1)
        {
            // Um único bônus ativo
            var bonus = activeBonuses[0];
            var bonusType = bonus.ContainsKey("BonusType") ?
                bonus["BonusType"].ToString() : "desconhecido";

            switch (bonusType)
            {
                case "correctAnswerBonus":
                    displayText = $"Bônus de XP dobrada ativo: {minutes:00}:{seconds:00}";
                    break;
                case "specialBonus":
                    displayText = $"Bônus de XP triplicada ativo: {minutes:00}:{seconds:00}";
                    break;
                default:
                    displayText = $"Bônus de XP (x{combinedMultiplier}) ativo: {minutes:00}:{seconds:00}";
                    break;
            }
        }
        else
        {
            // Sem bônus ativo (caso de erro)
            displayText = "Sem bônus ativo";
        }

        bonusTimerText.text = displayText;

        // Log para debug
        Debug.Log($"Timer atualizado: {displayText}, Container ativo: {bonusTimerContainer.activeSelf}");
    }

    public bool IsBonusActive()
    {
        return isBonusActive;
    }

    public int GetCurrentScoreMultiplier()
    {
        return isBonusActive ? combinedMultiplier : 1;
    }

    public int ApplyBonusToScore(int baseScore)
    {
        if (isBonusActive && baseScore > 0)
        {
            return baseScore * combinedMultiplier;
        }
        return baseScore;
    }

    private void HideBonusFeedback()
    {
        if (questionBonusUIFeedback != null && questionBonusUIFeedback.IsVisible())
        {
            if (canvasGroupManager != null)
            {
                canvasGroupManager.ShowBonusFeedback(false);
            }
            else
            {
                questionBonusUIFeedback.ForceVisibility(false);
            }
        }
    }

    private void OnDestroy()
    {
        if (bonusTimerCoroutine != null)
        {
            StopCoroutine(bonusTimerCoroutine);
        }

        if (answerManager != null)
        {
            answerManager.OnAnswerSelected -= CheckAnswer;
        }

        if (questionBottomUIManager != null)
        {
            questionBottomUIManager.OnExitButtonClicked -= HideBonusFeedback;
            questionBottomUIManager.OnNextButtonClicked -= HideBonusFeedback;
        }

        SaveBonusStateOnExit();
    }

    private async void SaveBonusStateOnExit()
    {
        if (!isBonusActive || UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            return;
        }

        string userId = UserDataStore.CurrentUserData.UserId;

        try
        {
            // Usar UpdateBonusExpirations em vez de UpdateExpirationTimestamp
            await bonusManager.UpdateBonusExpirations(userId, currentBonusTime);
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao salvar estado do bônus ao sair: {e.Message}");
        }
    }

    public async Task<bool> CheckIfUserHasActiveBonus(string bonusType)
    {
        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            return false;
        }

        string userId = UserDataStore.CurrentUserData.UserId;

        try
        {
            if (bonusType.StartsWith("active_"))
            {
                UserBonusManager userBonusManager = new UserBonusManager();
                List<BonusType> bonuses = await userBonusManager.GetUserBonuses(userId);
                BonusType targetBonus = bonuses.Find(b => b.BonusName == bonusType);

                return targetBonus != null && targetBonus.IsBonusActive && !targetBonus.IsExpired();
            }
            else
            {
                // Verificar em QuestionSceneBonus
                return await bonusManager.IsBonusActive(userId, bonusType);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao verificar bônus ativo: {e.Message}");
            return false;
        }
    }

    private void CheckAnswer(int selectedAnswerIndex)
    {
        QuestionManager questionManager = FindFirstObjectByType<QuestionManager>();
        if (questionManager == null)
        {
            Debug.LogError("QuestionBonusManager: QuestionManager não encontrado.");
            return;
        }

        var currentSessionField = typeof(QuestionManager).GetField("currentSession", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (currentSessionField == null)
        {
            Debug.LogError("QuestionBonusManager: Campo 'currentSession' não encontrado em QuestionManager.");
            return;
        }

        var currentSession = currentSessionField.GetValue(questionManager);
        if (currentSession == null)
        {
            Debug.LogError("QuestionBonusManager: currentSession é null.");
            return;
        }

        var getCurrentQuestionMethod = currentSession.GetType().GetMethod("GetCurrentQuestion");
        if (getCurrentQuestionMethod == null)
        {
            Debug.LogError("QuestionBonusManager: Método 'GetCurrentQuestion' não encontrado.");
            return;
        }

        var currentQuestion = getCurrentQuestionMethod.Invoke(currentSession, null) as Question;
        if (currentQuestion == null)
        {
            Debug.LogError("QuestionBonusManager: currentQuestion é null.");
            return;
        }

        bool isCorrect = selectedAnswerIndex == currentQuestion.correctIndex;

        if (isCorrect)
        {
            consecutiveCorrectAnswers++;

            if (consecutiveCorrectAnswers >= consecutiveCorrectAnswersNeeded && !isBonusActive)
            {
                ActivateBonus();
            }
        }
        else
        {
            consecutiveCorrectAnswers = 0;
        }
    }
}