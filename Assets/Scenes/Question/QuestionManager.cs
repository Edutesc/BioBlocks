using UnityEngine;
using System;
using System.Threading.Tasks;
using QuestionSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

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
            // Se for o feedback de conclusão, usar o texto e canvas group corretos
            feedbackElements.QuestionsCompletedFeedbackText.text = message;
            questionCanvasGroupManager.ShowCompletionFeedback();
            Debug.Log("Feedback de conclusão ativado com mensagem: " + message);

            // Desabilitar a navegação para próxima questão, apenas permitir voltar ao menu
            bottomBarManager.SetupNavigationButtons(
                () =>
                {
                    NavigationManager.Instance.NavigateTo("PathwayScene");
                },
                null  // Não permitir avançar para próxima questão
            );

            return;
        }

        // Feedback normal para respostas
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

    // private async Task HandleNextQuestion()
    // {
    //     bottomBarManager.DisableNavigationButtons();

    //     // Inicia a transição - a lógica de avançar a questão agora é tratada pelos eventos
    //     await transitionManager.TransitionToNextQuestion();

    //     // Inicia o timer para a nova questão
    //     timerManager.StartTimer();
    // }

    private async Task HandleNextQuestion()
    {
        bottomBarManager.DisableNavigationButtons();

        if (currentSession.IsLastQuestion())
        {
            string currentDatabaseName = loadManager.DatabankName;
            List<string> answeredQuestions = await AnsweredQuestionsManager.Instance.FetchUserAnsweredQuestionsInTargetDatabase(currentDatabaseName);
            int answeredCount = answeredQuestions.Count;

            if (answeredCount >= 50)
            {
                Debug.Log("HandleNextQuestion: Todas as questões respondidas. Exibindo feedback de conclusão.");
                ShowAnswerFeedback("Parabéns!! Você respondeu todas as perguntas desta lista corretamente!", true, true);
                return; 
            }
        }

        await transitionManager.TransitionToNextQuestion();

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
        try
        {
            // Obter o banco de dados atual
            QuestionSet currentSet = QuestionSetManager.GetCurrentQuestionSet();
            string currentDatabaseName = loadManager.DatabankName;

            Debug.Log($"Verificando questões para o banco: {currentDatabaseName}");

            // Obter a lista de números de questões respondidas para esse banco de dados
            List<string> answeredQuestionsIds = await AnsweredQuestionsManager.Instance.FetchUserAnsweredQuestionsInTargetDatabase(currentDatabaseName);

            // Converter para inteiros para facilitar a comparação
            List<int> answeredQuestionNumbers = answeredQuestionsIds
                .Select(id => int.TryParse(id, out int num) ? num : -1)
                .Where(num => num != -1)
                .ToList();

            Debug.Log($"Questões respondidas: {string.Join(", ", answeredQuestionNumbers)}");
            Debug.Log($"Total de questões respondidas: {answeredQuestionNumbers.Count}/50");

            // Se todas as 50 questões foram respondidas, mostrar feedback de conclusão
            if (answeredQuestionNumbers.Count >= 50)
            {
                Debug.Log("Todas as 50 questões foram respondidas corretamente! Mostrando feedback de conclusão.");
                ShowAnswerFeedback("Parabéns!! Você respondeu todas as perguntas desta lista corretamente!", true, true);
                return;
            }

            // Carregar todas as questões disponíveis para este conjunto
            var allQuestions = await loadManager.LoadQuestionsForSet(currentSet);

            if (allQuestions == null || allQuestions.Count == 0)
            {
                Debug.LogWarning("Não foi possível carregar questões para este conjunto");
                return;
            }

            // Filtrar apenas as questões que ainda não foram respondidas
            var unansweredQuestions = allQuestions
                .Where(q => !answeredQuestionNumbers.Contains(q.questionNumber))
                .ToList();

            Debug.Log($"Questões não respondidas: {unansweredQuestions.Count}");

            if (unansweredQuestions.Count > 0)
            {
                // Ainda há questões para responder
                currentSession = new QuestionSession(unansweredQuestions);
                StartQuestion();
            }
            else
            {
                // Não há mais questões para responder - todas foram respondidas
                Debug.Log("Não há mais questões pendentes! Mostrando feedback de conclusão.");
                ShowAnswerFeedback("Parabéns!! Você respondeu todas as perguntas desta lista corretamente!", true, true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro em CheckAndLoadMoreQuestions: {ex.Message}\n{ex.StackTrace}");
            // Em caso de erro, para evitar loop infinito, mostrar algum feedback
            ShowAnswerFeedback("Ocorreu um erro ao verificar questões. Volte ao menu principal.", false, true);
        }
    }

    private Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}