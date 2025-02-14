using UnityEngine;
using System;
using System.Threading.Tasks;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private QuestionUIManager uiManager;
    [SerializeField] private QuestionTimerManager timerManager;
    [SerializeField] private QuestionLoadManager loadManager;
    [SerializeField] private QuestionAnswerManager answerManager;
    [SerializeField] private QuestionTransitionManager transitionManager;
    [SerializeField] private QuestionScoreManager scoreManager;
    private string targetScene;

    private QuestionSession currentSession;

    private async void Start()
    {
        if (!ValidateManagers())
        {
            Debug.LogError("Falha na validação dos managers necessários. Verifique as referências no Inspector.");
            return;
        }

        await InitializeSession();
        SetupEventHandlers();
        StartQuestion();
    }

    private bool ValidateManagers()
    {
        bool isValid = true;

        if (uiManager == null)
        {
            Debug.LogError("QuestionUIManager não está atribuído no QuestionManager");
            isValid = false;
        }

        if (timerManager == null)
        {
            Debug.LogError("QuestionTimerManager não está atribuído no QuestionManager");
            isValid = false;
        }

        if (loadManager == null)
        {
            Debug.LogError("QuestionLoadManager não está atribuído no QuestionManager");
            isValid = false;
        }

        if (answerManager == null)
        {
            Debug.LogError("QuestionAnswerManager não está atribuído no QuestionManager");
            isValid = false;
        }

        if (transitionManager == null)
        {
            Debug.LogError("QuestionTransitionManager não está atribuído no QuestionManager");
            isValid = false;
        }

        if (scoreManager == null)
        {
            Debug.LogError("QuestionScoreManager não está atribuído no QuestionManager");
            isValid = false;
        }

        return isValid;
    }

    private async Task InitializeSession()
    {
        try
        {
            var questions = await loadManager.LoadQuestionsForSet(QuestionSetManager.GetCurrentQuestionSet());
            currentSession = new QuestionSession(questions);

            if (currentSession.GetTotalQuestions() == 0)
            {
                Debug.LogWarning("Nenhuma questão carregada para a sessão");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao inicializar sessão: {e.Message}\nStackTrace: {e.StackTrace}");
        }
    }

    private void SetupEventHandlers()
    {
        if (timerManager != null)
        {
            timerManager.OnTimerComplete += HandleTimeUp;
        }

        if (answerManager != null)
        {
            answerManager.OnAnswerSelected += CheckAnswer;
        }
    }

    private async void CheckAnswer(int selectedAnswerIndex)
    {
        Debug.Log($"Verificando resposta selecionada: {selectedAnswerIndex}");

        timerManager.StopTimer();
        answerManager.DisableAllButtons();

        var currentQuestion = currentSession.GetCurrentQuestion();
        bool isCorrect = selectedAnswerIndex == currentQuestion.correctIndex;

        try
        {
            if (isCorrect)
            {
                uiManager.ShowFeedback("Resposta correta!\n+5 pontos", true);
                await scoreManager.UpdateScore(5, true, currentQuestion);
            }
            else
            {
                uiManager.ShowFeedback("Resposta errada!\n-2 Pontos.", false);
                await scoreManager.UpdateScore(-2, false, currentQuestion);
            }

            uiManager.EnableNavigationButtons();
            SetupNavigationButtons();
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao processar resposta: {e.Message}");
        }
    }

    private void StartQuestion()
    {
        if (currentSession == null)
        {
            Debug.LogError("currentSession é null");
            return;
        }

        try
        {
            var currentQuestion = currentSession.GetCurrentQuestion();
            Debug.Log($"Questão atual: {currentQuestion.questionText}");
            Debug.Log($"Respostas: {string.Join(", ", currentQuestion.answers)}");

            uiManager.ShowQuestion(currentQuestion);
            answerManager.SetupAnswerButtons(currentQuestion);

            // Inicia o timer por último, após toda a UI estar configurada
            if (timerManager != null)
            {
                timerManager.StartTimer();
                Debug.Log("Timer iniciado no StartQuestion");
            }
            else
            {
                Debug.LogError("timerManager é null em StartQuestion");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao iniciar questão: {e.Message}\nStackTrace: {e.StackTrace}");
        }
    }

    private async void HandleTimeUp()
    {
        answerManager.DisableAllButtons();
        uiManager.ShowFeedback("Tempo Esgotado!\n-1 ponto", false);
        await scoreManager.UpdateScore(-1, false, currentSession.GetCurrentQuestion());
        uiManager.EnableNavigationButtons();
        SetupNavigationButtons();
    }

    private void SetupNavigationButtons()
    {
        uiManager.SetupNavigationButtons(
            () =>
            {
                uiManager.HideFeedback();
                NavigationManager.Instance.NavigateTo("PathwayScene");
            },
            async () =>
            {
                uiManager.HideFeedback();
                await HandleNextQuestion();
            }
        );
    }

    public void NavigateTo(string scene)
    {
        Debug.LogError($"Inciando a cena {targetScene}");
        NavigationManager.Instance.NavigateTo(scene);
        Debug.LogError($"Navegação para {targetScene} completada");
    }

    private async Task HandleNextQuestion()
    {
        uiManager.DisableNavigationButtons();

        if (!currentSession.IsLastQuestion())
        {
            currentSession.NextQuestion();
            StartQuestion();
        }
        else
        {
            await CheckAndLoadMoreQuestions();
        }
    }

    private async Task CheckAndLoadMoreQuestions()
    {
        var newQuestions = await loadManager.LoadQuestionsForSet(QuestionSetManager.GetCurrentQuestionSet());

        if (newQuestions != null && newQuestions.Count > 0)
        {
            currentSession = new QuestionSession(newQuestions);
            await transitionManager.TransitionToQuestion();
            StartQuestion();
        }
        else
        {
            uiManager.ShowFeedback("Parabéns!! Você respondeu todas as perguntas desta lista corretamente!", false, true);
        }
    }

}
