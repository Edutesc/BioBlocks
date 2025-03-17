using System.Collections;
using UnityEngine;
using TMPro;
using QuestionSystem;

public class QuestionBonusManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private QuestionBonusUIFeedback questionBonusUIFeedback;
    [SerializeField] private TextMeshProUGUI bonusCorrectAnswerTimer;
    [SerializeField] private GameObject bonusTimerContainer;

    [Header("Bonus Configuration")]
    [SerializeField] private int consecutiveCorrectAnswersNeeded = 5;
    [SerializeField] private float bonusDuration = 600f; // 10 minutes in seconds
    [SerializeField] private int bonusScoreMultiplier = 2; // Double the normal score

    [Header("References")]
    [SerializeField] private QuestionScoreManager scoreManager;
    [SerializeField] private QuestionAnswerManager answerManager;

    private int consecutiveCorrectAnswers = 0;
    private bool isBonusActive = false;
    private float currentBonusTime = 0f;
    private Coroutine bonusTimerCoroutine = null;

    private void Start()
    {
        // Validar componentes necessários
        if (!ValidateComponents())
        {
            Debug.LogError("QuestionBonusManager: Falha na validação dos componentes necessários.");
            return;
        }

        // Esconder a UI de bônus no início
        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(false);
        }
        else if (bonusCorrectAnswerTimer != null)
        {
            bonusCorrectAnswerTimer.gameObject.SetActive(false);
        }

        // Registrar para o evento de resposta selecionada
        if (answerManager != null)
        {
            answerManager.OnAnswerSelected += CheckAnswer;
        }

        // Registrar para eventos de feedback de resposta e temporizador
        QuestionManager questionManager = FindObjectOfType<QuestionManager>();
        if (questionManager != null)
        {
            Debug.Log("QuestionBonusManager inicializado e conectado ao QuestionManager");
        }
    }

    private bool ValidateComponents()
    {
        // Verificar componentes UI
        if (questionBonusUIFeedback == null)
        {
            questionBonusUIFeedback = FindFirstObjectByType<QuestionBonusUIFeedback>();
            if (questionBonusUIFeedback == null)
            {
                Debug.LogError("QuestionBonusManager: BonusUIFeedback não encontrado. Por favor, adicione-o à cena.");
                Debug.LogWarning("QuestionBonusManager: Você pode criar um GameObject com o script BonusUIFeedback.");
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
            // Tenta encontrar o container pai do timer
            bonusTimerContainer = bonusCorrectAnswerTimer?.transform.parent.gameObject;
            if (bonusTimerContainer == null)
            {
                // Se não encontrar, assume o próprio gameObject do timer
                bonusTimerContainer = bonusCorrectAnswerTimer?.gameObject;
            }
        }

        // Verificar managers de jogo
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

        return true;
    }

    private void CheckAnswer(int selectedAnswerIndex)
    {
        // Vamos pegar a referência do QuestionManager para acessar a questão atual
        QuestionManager questionManager = FindFirstObjectByType<QuestionManager>();
        if (questionManager == null)
        {
            Debug.LogError("QuestionBonusManager: QuestionManager não encontrado.");
            return;
        }

        // Acessar a sessão atual para obter a questão atual através de reflexão
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

        // Chamar o método GetCurrentQuestion usando reflexão
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

        // Atualizar o contador de respostas corretas consecutivas
        if (isCorrect)
        {
            consecutiveCorrectAnswers++;
            Debug.Log($"QuestionBonusManager: Resposta correta! Contador: {consecutiveCorrectAnswers}/{consecutiveCorrectAnswersNeeded}");

            // Verificar se atingiu o número necessário para ativar o bônus
            if (consecutiveCorrectAnswers >= consecutiveCorrectAnswersNeeded && !isBonusActive)
            {
                ActivateBonus();
            }
        }
        else
        {
            // Resposta incorreta, reiniciar o contador
            consecutiveCorrectAnswers = 0;
            Debug.Log("QuestionBonusManager: Resposta incorreta. Contador de respostas consecutivas reiniciado.");
        }
    }

    private void ActivateBonus()
    {
        isBonusActive = true;
        currentBonusTime = bonusDuration;

        // Mostrar feedback e timer
        if (questionBonusUIFeedback != null)
        {
            Debug.Log("Chamando ShowBonusActivatedFeedback()");
            questionBonusUIFeedback.ShowBonusActivatedFeedback();
        }
        else
        {
            Debug.LogError("bonusUIFeedback é null no momento de ativar!");
        }

        // Ativar o timer
        if (bonusTimerContainer != null)
        {
            bonusTimerContainer.SetActive(true);
        }
        else if (bonusCorrectAnswerTimer != null)
        {
            bonusCorrectAnswerTimer.gameObject.SetActive(true);
        }

        // Iniciar o timer
        if (bonusTimerCoroutine != null)
        {
            StopCoroutine(bonusTimerCoroutine);
        }
        bonusTimerCoroutine = StartCoroutine(BonusTimerCoroutine());

        Debug.Log("QuestionBonusManager: Bônus de XP dobrado ativado por 10 minutos!");
    }

    private void DeactivateBonus()
    {
        isBonusActive = false;

        // Esconder o timer
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

    private IEnumerator BonusTimerCoroutine()
    {
        while (currentBonusTime > 0)
        {
            // Atualizar texto do timer
            UpdateTimerDisplay();

            yield return new WaitForSeconds(1f);
            currentBonusTime -= 1f;
        }

        // Tempo acabou, desativar o bônus
        DeactivateBonus();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentBonusTime / 60);
        int seconds = Mathf.FloorToInt(currentBonusTime % 60);
        bonusCorrectAnswerTimer.text = $"Bônus ativo: {minutes:00}:{seconds:00}";
    }

    // Método público para verificar se o bônus está ativo
    public bool IsBonusActive()
    {
        return isBonusActive;
    }

    // Método público para obter o multiplicador de pontuação atual
    public int GetCurrentScoreMultiplier()
    {
        return isBonusActive ? bonusScoreMultiplier : 1;
    }

    // Método público para aplicar o bônus de pontuação
    public int ApplyBonusToScore(int baseScore)
    {
        if (isBonusActive && baseScore > 0)
        {
            return baseScore * bonusScoreMultiplier;
        }
        return baseScore;
    }

    private void OnDestroy()
    {
        // Cancelar o timer se estiver rodando
        if (bonusTimerCoroutine != null)
        {
            StopCoroutine(bonusTimerCoroutine);
        }

        // Remover o listener do evento
        if (answerManager != null)
        {
            answerManager.OnAnswerSelected -= CheckAnswer;
        }
    }
}