using UnityEngine;
using System;
using System.Threading.Tasks;
using QuestionSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class QuestionManager : MonoBehaviour
{
    [Header("UI Managers")]
    [SerializeField] private TopBarUIManager topBarManager;
    [SerializeField] private BottomUIManager bottomBarManager;
    [SerializeField] private QuestionUIManager questionUIManager;
    [SerializeField] private QuestionCanvasGroupManager questionCanvasGroupManager;
    [SerializeField] private FeedbackUIElements feedbackElements;
    [SerializeField] private QuestionTransitionManager transitionManager;

    [Header("Game Logic Managers")]
    [SerializeField] private QuestionTimerManager timerManager;
    [SerializeField] private QuestionLoadManager loadManager;
    [SerializeField] private QuestionAnswerManager answerManager;
    [SerializeField] private QuestionScoreManager scoreManager;

    private QuestionSession currentSession;
    private Question nextQuestionToShow;

    private void Start()
    {
        if (!ValidateManagers())
        {
            Debug.LogError("Falha na validação dos managers necessários.");
            return;
        }

        InitializeAndStartSession();
    }

    private async void InitializeAndStartSession()
    {
        await InitializeSession();

        if (currentSession != null)
        {
            SetupEventHandlers();
            StartQuestion();
        }
    }

    private bool ValidateManagers()
    {
        return topBarManager != null &&
               bottomBarManager != null &&
               questionUIManager != null &&
               questionCanvasGroupManager != null &&
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
            QuestionSet currentSet = QuestionSetManager.GetCurrentQuestionSet();
            var questions = await loadManager.LoadQuestionsForSet(currentSet);

            // Verificar se há questões disponíveis
            if (questions == null || questions.Count == 0)
            {
                string currentDatabaseName = loadManager.DatabankName; // Usar o nome que já foi obtido
                Debug.Log($"Não há questões disponíveis em {currentDatabaseName}. Redirecionando para ResetDatabaseView");

                SceneDataManager.Instance.SetData(new Dictionary<string, object> { { "databankName", currentDatabaseName } });
                SceneManager.LoadScene("ResetDatabaseView");
                return;
            }

            currentSession = new QuestionSession(questions);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao inicializar sessão: {e.Message}");

            // Em caso de erro, ainda podemos pegar o nome do banco atual
            string currentDatabaseName = loadManager.DatabankName;
            SceneDataManager.Instance.SetData(new Dictionary<string, object> { { "databankName", currentDatabaseName } });
            SceneManager.LoadScene("ResetDatabaseView");
        }
    }

    private void SetupEventHandlers()
    {
        timerManager.OnTimerComplete += HandleTimeUp;
        answerManager.OnAnswerSelected += CheckAnswer;

        transitionManager.OnBeforeTransitionStart += PrepareNextQuestion;
        transitionManager.OnTransitionMidpoint += ApplyPreparedQuestion;
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
                ShowAnswerFeedback("Resposta correta!\n+5 pontos", true);
                await scoreManager.UpdateScore(5, true, currentQuestion);
            }
            else
            {
                ShowAnswerFeedback("Resposta errada!\n-2 Pontos.", false);
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

    private void ShowAnswerFeedback(string message, bool isCorrect, bool isCompleted = false)
    {
        if (isCompleted)
        {
            feedbackElements.QuestionsCompletedFeedbackText.text = message;
            questionCanvasGroupManager.ShowCompletionFeedback();
            Debug.Log("Feedback de conclusão ativado");
            return;
        }

        feedbackElements.FeedbackText.text = message;
        Color backgroundColor = isCorrect ? HexToColor("#D4EDDA") : HexToColor("#F8D7DA");
        questionCanvasGroupManager.ShowAnswerFeedback(isCorrect, HexToColor("#D4EDDA"), HexToColor("#F8D7DA"));
    }

    private async void PrepareNextQuestion()
    {
        if (!currentSession.IsLastQuestion())
        {
            // Avança para a próxima questão no objeto de sessão
            currentSession.NextQuestion();

            // Armazena a referência à próxima questão, mas ainda não a mostra
            nextQuestionToShow = currentSession.GetCurrentQuestion();

            // Pré-carrega quaisquer recursos necessários (imagens, etc.)
            await PreloadQuestionResources(nextQuestionToShow);
        }
        else
        {
            // Se for a última questão, prepara para verificar e carregar mais
            nextQuestionToShow = null;
        }
    }

    private async Task PreloadQuestionResources(Question question)
    {
        // Usa o QuestionUIManager para pré-carregar imagens
        if (question.isImageQuestion)
        {
            await questionUIManager.PreloadQuestionImage(question);
        }

        // Pré-configura os botões de resposta, se necessário
        // Isso pode variar dependendo da sua implementação do AnswerManager
        if (question.isImageAnswer)
        {
            // Se houver imagens nas respostas, você pode pré-carregá-las aqui
            // Por exemplo: await answerManager.PreloadAnswerImages(question);
        }
    }

    private void ApplyPreparedQuestion()
    {
        if (nextQuestionToShow != null)
        {
            answerManager.SetupAnswerButtons(nextQuestionToShow);
            questionCanvasGroupManager.ShowQuestion(
                isImageQuestion: nextQuestionToShow.isImageQuestion,
                isImageAnswer: nextQuestionToShow.isImageAnswer
            );
            questionUIManager.ShowQuestion(nextQuestionToShow);
            nextQuestionToShow = null;
        }
        else
        {
            StartCoroutine(HandleNoMoreQuestions());
        }
    }

    private void CleanupPreloadedResources()
    {
        questionUIManager.ClearPreloadedResources();
    }

    private IEnumerator HandleNoMoreQuestions()
    {
        // Corrotina para executar o CheckAndLoadMoreQuestions de forma assíncrona
        var task = CheckAndLoadMoreQuestions();
        yield return new WaitUntil(() => task.IsCompleted);

        if (currentSession != null && currentSession.GetCurrentQuestion() != null)
        {
            // Se novas questões foram carregadas, configura a UI
            var newQuestion = currentSession.GetCurrentQuestion();
            answerManager.SetupAnswerButtons(newQuestion);
            questionCanvasGroupManager.ShowQuestion(
                isImageQuestion: newQuestion.isImageQuestion,
                isImageAnswer: newQuestion.isImageAnswer
            );
            questionUIManager.ShowQuestion(newQuestion);
        }
    }

    private void StartQuestion()
    {
        try
        {
            var currentQuestion = currentSession.GetCurrentQuestion();
            answerManager.SetupAnswerButtons(currentQuestion);
            questionCanvasGroupManager.ShowQuestion(
                isImageQuestion: currentQuestion.isImageQuestion,
                isImageAnswer: currentQuestion.isImageAnswer
            );
            questionUIManager.ShowQuestion(currentQuestion);

            timerManager.StartTimer();
            Debug.Log($"Questão iniciada - isImageQuestion: {currentQuestion.isImageQuestion}, isImageAnswer: {currentQuestion.isImageAnswer}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao iniciar questão: {e.Message}");
        }
    }

    private async void HandleTimeUp()
    {
        answerManager.DisableAllButtons();
        ShowAnswerFeedback("Tempo Esgotado!\n-1 ponto", false);
        await scoreManager.UpdateScore(-1, false, currentSession.GetCurrentQuestion());
        bottomBarManager.EnableNavigationButtons();
        SetupNavigationButtons();
    }

    private void SetupNavigationButtons()
    {
        bottomBarManager.SetupNavigationButtons(
            () =>
            {
                HideAnswerFeedback();
                NavigationManager.Instance.NavigateTo("PathwayScene");
            },
            async () =>
            {
                HideAnswerFeedback();
                await HandleNextQuestion();
            }
        );
    }

    public void ReturnToPathway()
    {
        NavigationManager.Instance.NavigateTo("PathwayScene");
    }

    private void HideAnswerFeedback()
    {
        questionCanvasGroupManager.HideAnswerFeedback();
    }

    private async Task HandleNextQuestion()
    {
        bottomBarManager.DisableNavigationButtons();

        // Inicia a transição - a lógica de avançar a questão agora é tratada pelos eventos
        await transitionManager.TransitionToNextQuestion();

        // Inicia o timer para a nova questão
        timerManager.StartTimer();
    }

    private void OnDestroy()
    {
        if (timerManager != null)
            timerManager.OnTimerComplete -= HandleTimeUp;

        if (answerManager != null)
            answerManager.OnAnswerSelected -= CheckAnswer;

        if (transitionManager != null)
        {
            transitionManager.OnBeforeTransitionStart -= PrepareNextQuestion;
            transitionManager.OnTransitionMidpoint -= ApplyPreparedQuestion;
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
            ShowAnswerFeedback("Parabéns!! Você respondeu todas as perguntas desta lista corretamente!", false, true);
        }
    }

    private Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}