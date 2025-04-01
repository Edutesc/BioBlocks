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
    [SerializeField] private QuestionBottomUIManager questionBottomBarManager;
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
        Debug.Log("QuestionManager: Start iniciado");

        if (!ValidateManagers())
        {
            Debug.LogError("Falha na validação dos managers necessários.");
            return;
        }

        InitializeAndStartSession();
    }

    private async void InitializeAndStartSession()
    {
        Debug.Log("QuestionManager: InitializeAndStartSession iniciado");
        await InitializeSession();

        if (currentSession != null)
        {
            Debug.Log("QuestionManager: Session inicializada com sucesso");
            SetupEventHandlers();
            StartQuestion();
        }
        else
        {
            Debug.LogError("QuestionManager: currentSession é null após InitializeSession");
        }
    }

    private bool ValidateManagers()
    {
        Debug.Log("QuestionManager: Iniciando validação dos managers");

        if (questionBottomBarManager == null)
            Debug.LogError("QuestionManager: questionBottomBarManager é null");

        if (questionUIManager == null)
            Debug.LogError("QuestionManager: questionUIManager é null");

        if (questionCanvasGroupManager == null)
            Debug.LogError("QuestionManager: questionCanvasGroupManager é null");

        if (timerManager == null)
            Debug.LogError("QuestionManager: timerManager é null");

        if (loadManager == null)
            Debug.LogError("QuestionManager: loadManager é null");

        if (answerManager == null)
            Debug.LogError("QuestionManager: answerManager é null");

        if (scoreManager == null)
            Debug.LogError("QuestionManager: scoreManager é null");

        if (feedbackElements == null)
            Debug.LogError("QuestionManager: feedbackElements é null");

        if (transitionManager == null)
            Debug.LogError("QuestionManager: transitionManager é null");

        bool isValid = questionBottomBarManager != null &&
               questionUIManager != null &&
               questionCanvasGroupManager != null &&
               timerManager != null &&
               loadManager != null &&
               answerManager != null &&
               scoreManager != null &&
               feedbackElements != null &&
               transitionManager != null;

        Debug.Log($"QuestionManager: Validação dos managers: {isValid}");
        return isValid;
    }

    private async Task InitializeSession()
    {
        Debug.Log("QuestionManager: Iniciando InitializeSession");
        try
        {
            QuestionSet currentSet = QuestionSetManager.GetCurrentQuestionSet();
            Debug.Log($"QuestionManager: QuestionSet atual: {currentSet}");

            IQuestionDatabase database = FindQuestionDatabase(currentSet);
            if (database == null)
            {
                Debug.LogError($"Nenhum database encontrado para o QuestionSet: {currentSet}");
                return;
            }

            string currentDatabaseName = database.GetDatabankName();
            Debug.Log($"QuestionManager: Database name: {currentDatabaseName}");

            loadManager.databankName = currentDatabaseName;
            List<string> answeredQuestions = await AnsweredQuestionsManager.Instance.FetchUserAnsweredQuestionsInTargetDatabase(currentDatabaseName);
            int answeredCount = answeredQuestions.Count;
            int totalQuestions = QuestionBankStatistics.GetTotalQuestions(currentDatabaseName);

            Debug.Log($"QuestionManager: Total de questões respondidas: {answeredCount}/{totalQuestions}");

            if (totalQuestions <= 0)
            {
                List<Question> allQuestions = database.GetQuestions();
                totalQuestions = allQuestions.Count;
                QuestionBankStatistics.SetTotalQuestions(currentDatabaseName, totalQuestions);
                Debug.Log($"QuestionManager: Atualizando total de questões para: {totalQuestions}");
            }

            bool allQuestionsAnswered = QuestionBankStatistics.AreAllQuestionsAnswered(currentDatabaseName, answeredCount);
            Debug.Log($"QuestionManager: Todas as questões foram respondidas? {allQuestionsAnswered}");

            if (allQuestionsAnswered)
            {
                Debug.Log("QuestionManager: Carregando cena ResetDatabaseView");
                SceneDataManager.Instance.SetData(new Dictionary<string, object> { { "databankName", currentDatabaseName } });
                SceneManager.LoadScene("ResetDatabaseView");
                return;
            }

            Debug.Log("QuestionManager: Carregando questões para o set atual");
            var questions = await loadManager.LoadQuestionsForSet(currentSet);
            if (questions == null)
            {
                Debug.LogError("QuestionManager: questions retornou null do LoadQuestionsForSet");
                SceneDataManager.Instance.SetData(new Dictionary<string, object> { { "databankName", currentDatabaseName } });
                SceneManager.LoadScene("ResetDatabaseView");
                return;
            }

            if (questions.Count == 0)
            {
                Debug.LogError("QuestionManager: questions retornou lista vazia");
                SceneDataManager.Instance.SetData(new Dictionary<string, object> { { "databankName", currentDatabaseName } });
                SceneManager.LoadScene("ResetDatabaseView");
                return;
            }

            Debug.Log($"QuestionManager: {questions.Count} questões carregadas com sucesso");
            currentSession = new QuestionSession(questions);
        }
        catch (Exception e)
        {
            Debug.LogError($"QuestionManager: Erro em InitializeSession: {e.Message}\n{e.StackTrace}");
            string currentDatabaseName = loadManager.DatabankName;
            SceneDataManager.Instance.SetData(new Dictionary<string, object> { { "databankName", currentDatabaseName } });
            SceneManager.LoadScene("ResetDatabaseView");
        }
    }

    private IQuestionDatabase FindQuestionDatabase(QuestionSet targetSet)
    {
        Debug.Log($"QuestionManager: Procurando database para o set: {targetSet}");
        try
        {
            MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            Debug.Log($"QuestionManager: Encontrados {allBehaviours.Length} MonoBehaviours na cena");

            foreach (MonoBehaviour behaviour in allBehaviours)
            {
                if (behaviour is IQuestionDatabase database)
                {
                    Debug.Log($"QuestionManager: Encontrado potencial database: {behaviour.GetType().Name}");
                    if (database.GetQuestionSetType() == targetSet)
                    {
                        Debug.Log($"QuestionManager: Database correspondente encontrado: {behaviour.GetType().Name}");
                        return database;
                    }
                }
            }

            Debug.LogError($"QuestionManager: Nenhum database encontrado para o set: {targetSet}");
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao procurar database: {e.Message}\n{e.StackTrace}");
            return null;
        }
    }

    private void SetupEventHandlers()
    {
        Debug.Log("QuestionManager: Configurando event handlers");
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
                int baseScore = 5;
                int actualScore = baseScore;

                // Verificar se há bônus ativo
                bool bonusActive = false;
                if (scoreManager.HasBonusActive())
                {
                    bonusActive = true;
                    actualScore = scoreManager.CalculateBonusScore(baseScore);
                }

                // Mensagem de feedback dinâmica baseada no bônus
                string feedbackMessage = bonusActive
                    ? $"Resposta correta!\n+{actualScore} pontos (bônus ativo!)"
                    : "Resposta correta!\n+5 pontos";

                ShowAnswerFeedback(feedbackMessage, true);
                await scoreManager.UpdateScore(baseScore, true, currentQuestion);
            }
            else
            {
                ShowAnswerFeedback("Resposta errada!\n-2 Pontos.", false);
                await scoreManager.UpdateScore(-2, false, currentQuestion);
            }

            questionBottomBarManager.EnableNavigationButtons();
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
            questionBottomBarManager.SetupNavigationButtons(
                () =>
                {
                    NavigationManager.Instance.NavigateTo("PathwayScene");
                },
                null
            );

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
            currentSession.NextQuestion();
            nextQuestionToShow = currentSession.GetCurrentQuestion();
            await PreloadQuestionResources(nextQuestionToShow);
        }
        else
        {
            nextQuestionToShow = null;
        }
    }

    private async Task PreloadQuestionResources(Question question)
    {
        if (question.isImageQuestion)
        {
            await questionUIManager.PreloadQuestionImage(question);
        }

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
        var task = CheckAndLoadMoreQuestions();
        yield return new WaitUntil(() => task.IsCompleted);

        if (currentSession != null && currentSession.GetCurrentQuestion() != null)
        {
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
        Debug.Log("QuestionManager: Iniciando questão");
        try
        {
            var currentQuestion = currentSession.GetCurrentQuestion();
            Debug.Log($"QuestionManager: Questão atual ID: {currentQuestion.questionNumber}");

            answerManager.SetupAnswerButtons(currentQuestion);
            questionCanvasGroupManager.ShowQuestion(
                isImageQuestion: currentQuestion.isImageQuestion,
                isImageAnswer: currentQuestion.isImageAnswer
            );
            questionUIManager.ShowQuestion(currentQuestion);
            timerManager.StartTimer();
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao iniciar questão: {e.Message}\n{e.StackTrace}");
        }
    }

    private async void HandleTimeUp()
    {
        answerManager.DisableAllButtons();
        ShowAnswerFeedback("Tempo Esgotado!\n-1 ponto", false);
        await scoreManager.UpdateScore(-1, false, currentSession.GetCurrentQuestion());
        questionBottomBarManager.EnableNavigationButtons();
        SetupNavigationButtons();
    }

    private void SetupNavigationButtons()
    {
        questionBottomBarManager.SetupNavigationButtons(
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
        questionBottomBarManager.DisableNavigationButtons();

        if (currentSession.IsLastQuestion())
        {
            string currentDatabaseName = loadManager.DatabankName;
            List<string> answeredQuestions = await AnsweredQuestionsManager.Instance.FetchUserAnsweredQuestionsInTargetDatabase(currentDatabaseName);
            int answeredCount = answeredQuestions.Count;

            if (QuestionBankStatistics.AreAllQuestionsAnswered(currentDatabaseName, answeredCount))
            {
                Debug.Log($"BONUS FLOW: Todas as questões do database {currentDatabaseName} foram respondidas!");
                int totalQuestions = QuestionBankStatistics.GetTotalQuestions(currentDatabaseName);

                try
                {
                    // Primeiro, processa o bônus
                    Debug.Log("BONUS FLOW: Chamando HandleDatabaseCompletion...");
                    await HandleDatabaseCompletion(currentDatabaseName);
                    Debug.Log("BONUS FLOW: HandleDatabaseCompletion concluído");

                    // Depois, mostra o feedback com a mensagem de bônus
                    string completionMessage = $"Parabéns!! Você respondeu todas as {totalQuestions} perguntas desta lista corretamente!\n\nVocê ganhou um Bônus das Listas que pode ser ativado na tela de Bônus.";
                    ShowAnswerFeedback(completionMessage, true, true);

                    // Garantir que o texto esteja visível
                    if (feedbackElements != null && feedbackElements.QuestionsCompletedFeedbackText != null)
                    {
                        feedbackElements.QuestionsCompletedFeedbackText.text = completionMessage;
                        Debug.Log("BONUS FLOW: Texto de conclusão atualizado com mensagem sobre bônus");
                    }
                }
                catch (Exception bonusEx)
                {
                    Debug.LogError($"BONUS FLOW: ERRO ao processar bônus: {bonusEx.Message}\n{bonusEx.StackTrace}");

                    // Se houve erro ao processar o bônus, ainda mostra a mensagem de conclusão, mas sem menção ao bônus
                    ShowAnswerFeedback($"Parabéns!! Você respondeu todas as {totalQuestions} perguntas desta lista corretamente!", true, true);
                }

                return;
            }
        }

        await transitionManager.TransitionToNextQuestion();
        timerManager.StartTimer();
    }
    private void OnDestroy()
    {
        Debug.Log("QuestionManager: OnDestroy chamado");
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
            QuestionSet currentSet = QuestionSetManager.GetCurrentQuestionSet();
            string currentDatabaseName = loadManager.DatabankName;
            List<string> answeredQuestionsIds = await AnsweredQuestionsManager.Instance.FetchUserAnsweredQuestionsInTargetDatabase(currentDatabaseName);
            int answeredCount = answeredQuestionsIds.Count;
            int totalQuestions = QuestionBankStatistics.GetTotalQuestions(currentDatabaseName);

            if (QuestionBankStatistics.AreAllQuestionsAnswered(currentDatabaseName, answeredCount))
            {
                // Ativar o bônus das listas quando todas as questões forem respondidas
                await HandleDatabaseCompletion(currentDatabaseName);

                ShowAnswerFeedback($"Parabéns!! Você respondeu todas as {totalQuestions} perguntas desta lista corretamente!\n\nVocê ganhou um Bônus das Listas que pode ser ativado na tela de Bônus.", true, true);
                return;
            }

            var newQuestions = await loadManager.LoadQuestionsForSet(currentSet);

            if (newQuestions == null || newQuestions.Count == 0)
            {
                ShowAnswerFeedback("Não foi possível carregar mais questões. Volte ao menu principal.", false, true);
                return;
            }

            var unansweredQuestions = newQuestions
                .Where(q => !answeredQuestionsIds.Contains(q.questionNumber.ToString()))
                .ToList();

            if (unansweredQuestions.Count == 0)
            {
                // Também ativar o bônus aqui, para o caso deste caminho de código ser atingido
                await HandleDatabaseCompletion(currentDatabaseName);

                ShowAnswerFeedback($"Parabéns!! Você respondeu todas as {totalQuestions} perguntas desta lista corretamente!\n\nVocê ganhou um Bônus das Listas que pode ser ativado na tela de Bônus.", true, true);
            }
            else
            {
                currentSession = new QuestionSession(newQuestions);
                StartQuestion();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro em CheckAndLoadMoreQuestions: {ex.Message}\n{ex.StackTrace}");
            ShowAnswerFeedback("Ocorreu um erro ao verificar questões. Volte ao menu principal.", false, true);
        }
    }

    private async Task HandleDatabaseCompletion(string databankName)
    {
        try
        {
            if (string.IsNullOrEmpty(databankName) || string.IsNullOrEmpty(UserDataStore.CurrentUserData?.UserId))
            {
                return;
            }

            string userId = UserDataStore.CurrentUserData.UserId;
            ListCompletionBonusManager listBonusManager = new ListCompletionBonusManager();

            // Verificar se este databank já foi marcado como completo
            bool isEligible = await listBonusManager.CheckIfDatabankEligibleForBonus(userId, databankName);

            if (isEligible)
            {
                // Marcar o databank como completo
                await listBonusManager.MarkDatabankAsCompleted(userId, databankName);

                // Incrementar o contador de bônus das listas
                await listBonusManager.IncrementListCompletionBonus(userId, databankName);

                Debug.Log($"Database {databankName} completado! Bônus das Listas incrementado.");

                // Mostrar uma mensagem informando que o usuário ganhou um bônus
                // Isso pode ser implementado através de um sistema de notificação ou UI específica
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao processar conclusão do database: {e.Message}");
        }
    }
    private Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}