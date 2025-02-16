using UnityEngine;
using System;
using System.Threading.Tasks;

public class QuestionManager : MonoBehaviour
{
    [Header("UI Managers")]
    [SerializeField] private TopBarUIManager topBarManager;
    [SerializeField] private BottomUIManager bottomBarManager;
    [SerializeField] private QuestionUIManager questionUIManager;
    
    [Header("Game Logic Managers")]
    [SerializeField] private QuestionTimerManager timerManager;
    [SerializeField] private QuestionLoadManager loadManager;
    [SerializeField] private QuestionAnswerManager answerManager;
    [SerializeField] private QuestionScoreManager scoreManager;
    [SerializeField] private FeedbackUIElements feedbackElements;

    private QuestionSession currentSession;

    private async void Start()
    {
        if (!ValidateManagers())
        {
            Debug.LogError("Falha na validação dos managers necessários.");
            return;
        }

        await InitializeSession();
        SetupEventHandlers();
        StartQuestion();
    }

    private bool ValidateManagers()
    {
        return topBarManager != null && 
               bottomBarManager != null && 
               questionUIManager != null &&
               timerManager != null && 
               loadManager != null && 
               answerManager != null && 
               scoreManager != null &&
               feedbackElements != null;
    }

    private async Task InitializeSession()
    {
        try
        {
            var questions = await loadManager.LoadQuestionsForSet(QuestionSetManager.GetCurrentQuestionSet());
            currentSession = new QuestionSession(questions);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao inicializar sessão: {e.Message}");
        }
    }

    private void SetupEventHandlers()
    {
        timerManager.OnTimerComplete += HandleTimeUp;
        answerManager.OnAnswerSelected += CheckAnswer;
    }

    private async void CheckAnswer(int selectedAnswerIndex)
    {
        timerManager.StopTimer();
        answerManager.DisableAllButtons();

        var currentQuestion = currentSession.GetCurrentQuestion();
        bool isCorrect = selectedAnswerIndex == currentQuestion.correctIndex;

        try
        {
            if (isCorrect)
            {
                ShowFeedback("Resposta correta!\n+5 pontos", true);
                await scoreManager.UpdateScore(5, true, currentQuestion);
            }
            else
            {
                ShowFeedback("Resposta errada!\n-2 Pontos.", false);
                await scoreManager.UpdateScore(-2, false, currentQuestion);
            }

            bottomBarManager.EnableNavigationButtons();
            SetupNavigationButtons();
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao processar resposta: {e.Message}");
        }
    }

    private void ShowFeedback(string message, bool isCorrect, bool isCompleted = false)
    {
        if (isCompleted)
        {
            feedbackElements.QuestionsCompletedFeedbackText.text = message;
            feedbackElements.QuestionsCompletedFeedbackText.gameObject.SetActive(true);
            return;
        }

        feedbackElements.FeedbackText.text = message;
        feedbackElements.FeedbackPanel.gameObject.SetActive(true);
        
        Color backgroundColor = isCorrect ? HexToColor("#D4EDDA") : HexToColor("#F8D7DA");
        feedbackElements.FeedbackPanel.color = backgroundColor;
    }

    private void StartQuestion()
    {
        try
        {
            var currentQuestion = currentSession.GetCurrentQuestion();
            questionUIManager.ShowQuestion(currentQuestion);
            answerManager.SetupAnswerButtons(currentQuestion);
            timerManager.StartTimer();
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao iniciar questão: {e.Message}");
        }
    }

    private async void HandleTimeUp()
    {
        answerManager.DisableAllButtons();
        ShowFeedback("Tempo Esgotado!\n-1 ponto", false);
        await scoreManager.UpdateScore(-1, false, currentSession.GetCurrentQuestion());
        bottomBarManager.EnableNavigationButtons();
        SetupNavigationButtons();
    }

    private void SetupNavigationButtons()
    {
        bottomBarManager.SetupNavigationButtons(
            () => {
                HideFeedback();
                NavigationManager.Instance.NavigateTo("PathwayScene");
            },
            async () => {
                HideFeedback();
                await HandleNextQuestion();
            }
        );
    }

    private void HideFeedback()
    {
        feedbackElements.FeedbackPanel.gameObject.SetActive(false);
    }

    private async Task HandleNextQuestion()
    {
        bottomBarManager.DisableNavigationButtons();

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
            StartQuestion();
        }
        else
        {
            ShowFeedback("Parabéns!! Você respondeu todas as perguntas desta lista corretamente!", false, true);
        }
    }

    private Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}