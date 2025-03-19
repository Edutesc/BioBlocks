using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using QuestionSystem;

public class QuestionBonusManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private QuestionBonusUIFeedback questionBonusUIFeedback;
    [SerializeField] private TextMeshProUGUI bonusCorrectAnswerTimer;
    [SerializeField] private GameObject bonusTimerContainer;

    [Header("Bonus Configuration")]
    [SerializeField] private int consecutiveCorrectAnswersNeeded = 5;
    [SerializeField] private float bonusDuration = 600f; // 10 minutos em segundos
    [SerializeField] private int bonusScoreMultiplier = 2; // Dobra a pontuação normal

    [Header("References")]
    [SerializeField] private QuestionScoreManager scoreManager;
    [SerializeField] private QuestionAnswerManager answerManager;
    [SerializeField] private QuestionCanvasGroupManager canvasGroupManager;
    [SerializeField] private BottomUIManager bottomUIManager;

    private int consecutiveCorrectAnswers = 0;
    private bool isBonusActive = false;
    private float currentBonusTime = 0f;
    private Coroutine bonusTimerCoroutine = null;
    
    // Novo gerenciador do bônus de respostas corretas
    private CorrectAnswerBonusManager correctAnswerBonusManager;

    private void Start()
    {
        if (!ValidateComponents())
        {
            Debug.LogError("QuestionBonusManager: Falha na validação dos componentes necessários.");
            return;
        }

        // Inicializar o novo gerenciador
        correctAnswerBonusManager = new CorrectAnswerBonusManager();

        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(false);
        }
        else if (bonusCorrectAnswerTimer != null)
        {
            bonusCorrectAnswerTimer.gameObject.SetActive(false);
        }

        if (answerManager != null)
        {
            answerManager.OnAnswerSelected += CheckAnswer;
        }

        if (bottomUIManager != null)
        {
            bottomUIManager.OnExitButtonClicked += HideBonusFeedback;
            bottomUIManager.OnNextButtonClicked += HideBonusFeedback;
            Debug.Log("QuestionBonusManager: Registrado para eventos dos botões da BottomBar");
        }
        else
        {
            Debug.LogWarning("QuestionBonusManager: BottomUIManager não encontrado. O feedback de bônus não será escondido ao navegar.");
        }

        QuestionManager questionManager = FindFirstObjectByType<QuestionManager>();
        if (questionManager != null)
        {
            Debug.Log("QuestionBonusManager inicializado e conectado ao QuestionManager");
        }
        
        // Verificar se existe bônus ativo persistente
        StartCoroutine(CheckForActiveBonusCoroutine());
    }
    
    // Corrotina para verificar se existe um bônus ativo
    private IEnumerator CheckForActiveBonusCoroutine()
    {
        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            Debug.Log("QuestionBonusManager: Usuário não está logado, não é possível verificar bônus.");
            yield break;
        }

        string userId = UserDataStore.CurrentUserData.UserId;
        
        // Verificar se o bônus está ativo
        var isActiveTask = correctAnswerBonusManager.IsCorrectAnswerBonusActive(userId);
        yield return new WaitUntil(() => isActiveTask.IsCompleted);
        
        bool isActive = false;
        try
        {
            isActive = isActiveTask.Result;
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao verificar bônus ativo: {e.Message}");
            yield break;
        }
        
        if (isActive)
        {
            Debug.Log("QuestionBonusManager: Bônus ativo encontrado, obtendo tempo restante");
            
            // Obter tempo restante
            var remainingTimeTask = correctAnswerBonusManager.GetRemainingTime(userId);
            yield return new WaitUntil(() => remainingTimeTask.IsCompleted);
            
            float remainingTime = 0;
            try
            {
                remainingTime = remainingTimeTask.Result;
            }
            catch (Exception e)
            {
                Debug.LogError($"QuestionBonusManager: Erro ao obter tempo restante: {e.Message}");
                yield break;
            }
            
            if (remainingTime > 0)
            {
                // Ativar o bônus com o tempo restante
                ActivateBonusWithRemainingTime(remainingTime);
            }
        }
        else
        {
            Debug.Log("QuestionBonusManager: Nenhum bônus ativo encontrado.");
        }
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

        if (bonusCorrectAnswerTimer == null)
        {
            bonusCorrectAnswerTimer = GameObject.Find("BonusCorrectAnswerTimer")?.GetComponent<TextMeshProUGUI>();
            if (bonusCorrectAnswerTimer == null)
            {
                Debug.LogError("QuestionBonusManager: BonusCorrectAnswerTimer (TextMeshProUGUI) não encontrado.");
                Debug.LogWarning("QuestionBonusManager: Certifique-se de ter um objeto Text com o nome 'BonusCorrectAnswerTimer'.");
                return false;
            }
        }

        if (bonusTimerContainer == null)
        {
            bonusTimerContainer = bonusCorrectAnswerTimer?.transform.parent.gameObject;
            if (bonusTimerContainer == null)
            {
                bonusTimerContainer = bonusCorrectAnswerTimer?.gameObject;
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

        if (bottomUIManager == null)
        {
            bottomUIManager = FindFirstObjectByType<BottomUIManager>();
            if (bottomUIManager == null)
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
            Debug.Log($"QuestionBonusManager: Resposta correta! Contador: {consecutiveCorrectAnswers}/{consecutiveCorrectAnswersNeeded}");

            if (consecutiveCorrectAnswers >= consecutiveCorrectAnswersNeeded && !isBonusActive)
            {
                ActivateBonus();
            }
        }
        else
        {
            consecutiveCorrectAnswers = 0;
            Debug.Log("QuestionBonusManager: Resposta incorreta. Contador de respostas consecutivas reiniciado.");
        }
    }

    private void ActivateBonusWithRemainingTime(float remainingSeconds)
    {
        isBonusActive = true;
        currentBonusTime = remainingSeconds;
        
        Debug.Log($"QuestionBonusManager: Ativando bônus com {remainingSeconds} segundos restantes");
        
        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(true);
        }
        else if (bonusCorrectAnswerTimer != null)
        {
            bonusCorrectAnswerTimer.gameObject.SetActive(true);
        }
        
        // Inicia a contagem regressiva
        if (bonusTimerCoroutine != null)
        {
            StopCoroutine(bonusTimerCoroutine);
        }
        bonusTimerCoroutine = StartCoroutine(BonusTimerCoroutine());
        
        // Atualiza o display imediatamente
        UpdateTimerDisplay();
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
            // Ativar o bônus no novo sistema
            await correctAnswerBonusManager.ActivateCorrectAnswerBonus(userId, bonusDuration);
            
            // Configurar UI e estado local
            isBonusActive = true;
            currentBonusTime = bonusDuration;

            if (canvasGroupManager != null)
            {
                Debug.Log("Exibindo feedback de bônus através do CanvasGroupManager");
                canvasGroupManager.ShowBonusFeedback(true);

                if (questionBonusUIFeedback != null)
                {
                    questionBonusUIFeedback.ShowBonusActivatedFeedback();
                }
            }
            else
            {
                Debug.Log("Exibindo feedback de bônus diretamente (sem CanvasGroupManager)");
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

            if (bonusTimerContainer != null)
            {
                bonusTimerContainer.SetActive(true);
            }
            else if (bonusCorrectAnswerTimer != null)
            {
                bonusCorrectAnswerTimer.gameObject.SetActive(true);
            }

            if (bonusTimerCoroutine != null)
            {
                StopCoroutine(bonusTimerCoroutine);
            }
            bonusTimerCoroutine = StartCoroutine(BonusTimerCoroutine());

            Debug.Log("QuestionBonusManager: Bônus de XP dobrado ativado por 10 minutos!");
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
            // Desativar o bônus no novo sistema
            await correctAnswerBonusManager.DeactivateCorrectAnswerBonus(userId);
            
            // Atualizar UI e estado local
            isBonusActive = false;

            if (canvasGroupManager != null)
            {
                canvasGroupManager.ShowBonusFeedback(false);
            }

            if (bonusTimerContainer != null)
            {
                bonusTimerContainer.SetActive(false);
            }
            else if (bonusCorrectAnswerTimer != null)
            {
                bonusCorrectAnswerTimer.gameObject.SetActive(false);
            }

            Debug.Log("QuestionBonusManager: Bônus de XP dobrado desativado.");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao desativar bônus: {e.Message}");
        }
    }

    private IEnumerator BonusTimerCoroutine()
    {
        float lastUpdateTime = currentBonusTime;
        
        while (currentBonusTime > 0)
        {
            UpdateTimerDisplay();

            yield return new WaitForSeconds(1f);
            currentBonusTime -= 1f;
            
            // Atualizar o timestamp de expiração periodicamente
            if (lastUpdateTime - currentBonusTime >= 30f || currentBonusTime <= 10f)
            {
                lastUpdateTime = currentBonusTime;
                UpdateBonusExpirationInFirestore();
            }
        }

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
            // Atualizar o timestamp de expiração no novo sistema
            await correctAnswerBonusManager.UpdateExpirationTimestamp(userId, currentBonusTime);
            
            Debug.Log($"QuestionBonusManager: Timestamp de expiração atualizado. Tempo restante: {currentBonusTime}s");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao atualizar timestamp de expiração: {e.Message}");
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentBonusTime / 60);
        int seconds = Mathf.FloorToInt(currentBonusTime % 60);
        bonusCorrectAnswerTimer.text = $"Bônus de XP drobrada ativo: {minutes:00}:{seconds:00}";
    }

    public bool IsBonusActive()
    {
        return isBonusActive;
    }

    public int GetCurrentScoreMultiplier()
    {
        return isBonusActive ? bonusScoreMultiplier : 1;
    }

    public int ApplyBonusToScore(int baseScore)
    {
        if (isBonusActive && baseScore > 0)
        {
            return baseScore * bonusScoreMultiplier;
        }
        return baseScore;
    }

    private void HideBonusFeedback()
    {
        Debug.Log("QuestionBonusManager: Botão da BottomBar clicado, escondendo feedback de bônus");

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

            Debug.Log("QuestionBonusManager: Feedback de bônus escondido após clique em botão");
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

        if (bottomUIManager != null)
        {
            bottomUIManager.OnExitButtonClicked -= HideBonusFeedback;
            bottomUIManager.OnNextButtonClicked -= HideBonusFeedback;
        }
        
        // Salvar estado do bônus ao sair da cena
        SaveBonusStateOnExitNonBlocking();
    }
    
    private void SaveBonusStateOnExitNonBlocking()
    {
        if (!isBonusActive || UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            return;
        }

        string userId = UserDataStore.CurrentUserData.UserId;
        
        try
        {
            // Salvar o estado atual do bônus no novo sistema
            _ = correctAnswerBonusManager.UpdateExpirationTimestamp(userId, currentBonusTime);
            
            Debug.Log($"QuestionBonusManager: Estado do bônus salvo ao sair. Tempo restante: {currentBonusTime} segundos.");
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
            if (bonusType == "correctAnswerBonus")
            {
                // Verificar se o bônus de respostas corretas está ativo
                return await correctAnswerBonusManager.IsCorrectAnswerBonusActive(userId);
            }
            else
            {
                // Para outros tipos de bônus, verificar no SpecialBonusManager
                SpecialBonusManager specialBonusManager = new SpecialBonusManager();
                List<BonusType> bonuses = await specialBonusManager.GetUserBonuses(userId);
                BonusType targetBonus = bonuses.Find(b => b.BonusName == bonusType);
                
                return targetBonus != null && targetBonus.IsBonusActive && !targetBonus.IsExpired();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao verificar bônus ativo: {e.Message}");
            return false;
        }
    }
}