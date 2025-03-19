using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private BonusFirestore bonusFirestore;
    
    private const string CONSECUTIVE_ANSWERS_BONUS = "correctAnswerBonus";

    private void Start()
    {
        if (!ValidateComponents())
        {
            Debug.LogError("QuestionBonusManager: Falha na validação dos componentes necessários.");
            return;
        }

        bonusFirestore = new BonusFirestore();

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
        
        // Verificar se existe bônus ativo persistente no Firestore
        // Chama o método assíncrono sem await, pois Start não pode ser async
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

        // Usar uma task e aguardar sua conclusão
        var task = bonusFirestore.GetUserBonuses(UserDataStore.CurrentUserData.UserId);
        
        // Aguardamos a tarefa completar sem usar try-catch ao redor do yield
        yield return new WaitUntil(() => task.IsCompleted);
        
        // Após o yield, agora podemos usar try-catch
        try
        {
            if (task.Exception != null)
            {
                Debug.LogError($"QuestionBonusManager: Erro ao obter bônus: {task.Exception.Message}");
                yield break;
            }
            
            // Buscar os bônus do usuário
            List<BonusType> userBonuses = task.Result;
            
            // Procurar por um bônus de respostas consecutivas
            BonusType consecutiveAnswersBonus = userBonuses.FirstOrDefault(b => b.BonusName == CONSECUTIVE_ANSWERS_BONUS);
            
            if (consecutiveAnswersBonus != null && consecutiveAnswersBonus.IsBonusActive && !consecutiveAnswersBonus.IsExpired())
            {
                // O bônus está ativo e ainda não expirou
                Debug.Log($"QuestionBonusManager: Bônus persistente encontrado, ainda ativo!");
                
                // Calcular tempo restante
                long remainingSeconds = consecutiveAnswersBonus.GetRemainingSeconds();
                if (remainingSeconds > 0)
                {
                    // Ativar o bônus com o tempo restante
                    ActivateBonusWithRemainingTime((float)remainingSeconds);
                }
            }
            else
            {
                Debug.Log("QuestionBonusManager: Nenhum bônus ativo encontrado ou bônus expirado.");
                
                // Se o bônus expirou, certifique-se de que está desativado no banco de dados
                if (consecutiveAnswersBonus != null && consecutiveAnswersBonus.IsBonusActive && consecutiveAnswersBonus.IsExpired())
                {
                    // Iniciar outra task para desativar o bônus
                    _ = bonusFirestore.DeactivateBonus(UserDataStore.CurrentUserData.UserId, CONSECUTIVE_ANSWERS_BONUS);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"QuestionBonusManager: Erro ao verificar bônus ativo: {e.Message}");
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

    private async void CheckAnswer(int selectedAnswerIndex)
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

    // Método para ativar o bônus com o tempo restante
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

    private void ActivateBonus()
    {
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
        
        // Atualiza imediatamente o estado no Firestore para tornar o bônus persistente
        if (UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            string userId = UserDataStore.CurrentUserData.UserId;
            long expirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(bonusDuration).ToUnixTimeSeconds();
            
            // Cria o bônus persistente
            BonusType bonus = new BonusType(
                CONSECUTIVE_ANSWERS_BONUS,
                consecutiveCorrectAnswersNeeded,
                true,
                expirationTimestamp,
                true
            );
            
            // Salva no Firestore (de forma assíncrona)
            _ = bonusFirestore.UpdateBonus(userId, bonus);
        }

        Debug.Log("QuestionBonusManager: Bônus de XP dobrado ativado por 10 minutos!");
    }

    private void DeactivateBonus()
    {
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
        
        // Desativa o bônus no Firestore quando expira
        if (UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            string userId = UserDataStore.CurrentUserData.UserId;
            
            // Desativa o bônus de forma assíncrona
            _ = bonusFirestore.DeactivateBonus(userId, CONSECUTIVE_ANSWERS_BONUS);
        }

        Debug.Log("QuestionBonusManager: Bônus de XP dobrado desativado.");
    }

    private IEnumerator BonusTimerCoroutine()
    {
        float lastUpdateTime = currentBonusTime;
        
        while (currentBonusTime > 0)
        {
            UpdateTimerDisplay();

            yield return new WaitForSeconds(1f);
            currentBonusTime -= 1f;
            
            // Atualiza o timestamp de expiração no Firestore periodicamente (a cada 30 segundos)
            // Isso garante que mesmo se o app fechar inesperadamente, o tempo máximo perdido seja de 30 segundos
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
        if (isBonusActive && UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            try
            {
                string userId = UserDataStore.CurrentUserData.UserId;
                long expirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(currentBonusTime).ToUnixTimeSeconds();
                
                // Busca os bônus atuais
                List<BonusType> userBonuses = await bonusFirestore.GetUserBonuses(userId);
                BonusType correctAnswerBonus = userBonuses.FirstOrDefault(b => b.BonusName == CONSECUTIVE_ANSWERS_BONUS);
                
                if (correctAnswerBonus != null)
                {
                    // Atualiza o timestamp de expiração
                    correctAnswerBonus.ExpirationTimestamp = expirationTimestamp;
                    correctAnswerBonus.IsPersistent = true;
                    
                    // Salva no Firestore
                    await bonusFirestore.UpdateBonus(userId, correctAnswerBonus);
                    
                    Debug.Log($"QuestionBonusManager: Timestamp de expiração atualizado. Tempo restante: {currentBonusTime}s");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"QuestionBonusManager: Erro ao atualizar timestamp de expiração: {e.Message}");
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentBonusTime / 60);
        int seconds = Mathf.FloorToInt(currentBonusTime % 60);
        bonusCorrectAnswerTimer.text = $"Bônus de 5 respostas corretas ativo: {minutes:00}:{seconds:00}";
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
        
        // Atualiza o timestamp de expiração no Firestore quando o usuário sai da cena
        // Não podemos usar await em OnDestroy, então chamamos de forma não-bloqueante
        if (isBonusActive)
        {
            SaveBonusStateOnExitNonBlocking();
        }
    }
    
    private void SaveBonusStateOnExitNonBlocking()
    {
        // Verifica se o bônus está ativo e se o usuário está logado
        if (isBonusActive && UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            try
            {
                string userId = UserDataStore.CurrentUserData.UserId;
                
                // Calcula o novo timestamp de expiração com base no tempo restante
                long expirationTimestamp = DateTimeOffset.UtcNow.AddSeconds(currentBonusTime).ToUnixTimeSeconds();
                
                // Cria um bônus atualizado
                BonusType bonus = new BonusType(
                    CONSECUTIVE_ANSWERS_BONUS,
                    consecutiveCorrectAnswersNeeded,
                    true,
                    expirationTimestamp,
                    true
                );
                
                // Salva no Firestore de forma não-bloqueante
                _ = bonusFirestore.UpdateBonus(userId, bonus);
                
                Debug.Log($"QuestionBonusManager: Estado do bônus salvo ao sair. Tempo restante: {currentBonusTime} segundos. Expira em: {DateTimeOffset.FromUnixTimeSeconds(expirationTimestamp).LocalDateTime}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"QuestionBonusManager: Erro ao salvar estado do bônus ao sair: {e.Message}");
            }
        }
    }

    public async Task<bool> CheckIfUserHasActiveBonus(string bonusType)
    {
        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            return false;
        }

        List<BonusType> userBonuses = await bonusFirestore.GetUserBonuses(UserDataStore.CurrentUserData.UserId);
        BonusType targetBonus = userBonuses.FirstOrDefault(b => b.BonusName == bonusType);

        return targetBonus != null && targetBonus.IsBonusActive && !targetBonus.IsExpired();
    }
}