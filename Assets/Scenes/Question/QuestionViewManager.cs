// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using QuestionSystem;
// using TMPro;
// using System.Linq;
// using System;
// using System.Collections;

// public class QuestionViewManager : MonoBehaviour
// {
//     [Header("Instances")]
//     private UserData currentUserData;
//     public AnsweredQuestionsManager answeredQuestionsManager;

//     [Header("Main UI")]
//     [SerializeField] private TextMeshProUGUI questionText;
//     [SerializeField] private Button[] textAnswerButtons;
//     [SerializeField] private Button[] imageAnswerButtons;
//     [SerializeField] private Button exitButton;
//     [SerializeField] private Button nextQuestionButton;
//     [SerializeField] private TextMeshProUGUI timerText;
//     [SerializeField] private GameObject timePanel;

//     [Header("Top Bar")]
//     [SerializeField] private TextMeshProUGUI scoreText;
//     [SerializeField] private TextMeshProUGUI nameText;

//     [Header("Feedbacks")]
//     [SerializeField] private TextMeshProUGUI feedbackText;
//     [SerializeField] private Image feedbackPanel;
//     [SerializeField] private TextMeshProUGUI questionsCompletedFeedbackText;
//     [SerializeField] private float fadeDuration = 0.5f;

//     [Header("Loading Spinner Configuration")]
//     [SerializeField] private float spinnerRotationSpeed = 100f;
//     [SerializeField] private Image loadingSpinner;

//     [Header("CanvasGroup Configuration")]
//     [SerializeField] private CanvasGroup loadingCanvasGroup;
//     [SerializeField] private CanvasGroup questionCanvasGroup;
//     [SerializeField] private CanvasGroup questionImageCanvasGroup;
//     [SerializeField] private CanvasGroup questionsCompletedFeedback;
//     [SerializeField] private CanvasGroup bottomBar;
//     [SerializeField] private CanvasGroup questionTextBackground;

//     [Header("Logics")]
//     [SerializeField] private float displayDuration = 1.5f;
//     [SerializeField] private float timeLeft = 20f;
//     private List<Question> questions;
//     private string databankName = "";
//     private int currentQuestionIndex = 0;
//     public int numberOfQuestions;
//     private bool timerRunning = false;
//     private Coroutine feedbackCoroutine;

//     private void Update()
//     {
//         // Rotacionar o spinner
//         if (loadingCanvasGroup != null && (loadingCanvasGroup.gameObject.activeSelf || loadingCanvasGroup.gameObject.activeSelf))
//         {
//             loadingSpinner.transform.Rotate(0f, 0f, -spinnerRotationSpeed * Time.deltaTime);
//         }
//     }

//     private async void Start()
//     {
//         exitButton.gameObject.SetActive(true);
//         nextQuestionButton.gameObject.SetActive(true);
//         exitButton.interactable = false;
//         nextQuestionButton.interactable = false;
//         timePanel.gameObject.SetActive(false);
//         questionsCompletedFeedback.gameObject.SetActive(false);
//         bottomBar.gameObject.SetActive(true);

//         if (!AuthenticationRepository.Instance.IsUserLoggedIn())
//         {
//             Debug.LogError("Usuário não está autenticado!");
//             return;
//         }

//         currentUserData = UserDataStore.CurrentUserData;
//         if (currentUserData == null)
//         {
//             Debug.LogError("CurrentUserData é null!");
//             return;
//         }

//         // Inscreve-se para receber atualizações do UserDataStore
//         UserDataStore.OnUserDataChanged += OnUserDataChanged;

//         UpdateUI(currentUserData);

//         await LoadQuestionsForCurrentSceneAsync();
//         await HandleQuestionsLoadResult();

//         timePanel.gameObject.SetActive(true);
//         timerText.gameObject.SetActive(true);

//         timerText = GameObject.Find("TimerText")?.GetComponent<TextMeshProUGUI>();
//         if (timerText == null)
//         {
//             Debug.LogError("Timer Text não está sendo atribuído no inspector");
//         }
//         else
//         {
//             Debug.Log("Timer Text encontrado e atribuído");
//         }

//         answeredQuestionsManager = AnsweredQuestionsManager.Instance;
//         if (answeredQuestionsManager == null)
//         {
//             Debug.LogError("AnsweredQuestionsManager not found in the scene!");
//         }
//     }

//     private void UpdateUI(UserData userData)
//     {
//         if (userData == null)
//         {
//             Debug.LogError("Tentando atualizar UI com userData null");
//             return;
//         }

//         nameText.text = userData.NickName;
//         scoreText.text = $"{userData.Score} XP";
//         Debug.Log($"UI atualizada - Nome: {userData.NickName}, Score: {userData.Score}");
//     }

//     private async Task HandleQuestionsLoadResult()
//     {
//         if (questions == null || questions.Count == 0)
//         {
//             Debug.LogError($"No questions loaded on {databankName}");
//             NavigateToResetDatabaseScene(databankName);
//             return;
//         }

//         Debug.Log($"Successfully loaded {questions.Count} questions");
//         numberOfQuestions = questions.Count;

//         await TransitionToQuestions();
//         Debug.Log($"TransitionToQuestions Successfully loaded");

//         ShowQuestion(currentQuestionIndex);
//     }

//     private async Task TransitionToQuestions()
//     {
//         Debug.Log("Starting TransitionToQuestion with DOTween");

//         try
//         {
//             var tcs = new TaskCompletionSource<bool>();
//             Debug.Log($"tcs: {tcs}");
//             StartCoroutine(SmoorhTransitionCoroutine(tcs));
//             await tcs.Task;
//             Debug.Log($"tcs.Task: {tcs.Task}");
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($" Error in TransitionToQuestions: {e.Message}\n{e.StackTrace}");
//         }
//     }

//     private void OnUserDataChanged(UserData userData)
//     {
//         currentUserData = userData;
//         UpdateUI(userData);

//     }

//     private async Task UpdateScore(int scoreToAdd, bool isCorrect)
//     {
//         if (questions == null || currentQuestionIndex < 0 || currentQuestionIndex >= questions.Count)
//         {
//             Debug.LogError("Error updating score: Invalid state.");
//             return;
//         }

//         Question currentQuestion = questions[currentQuestionIndex];
//         string databankName = currentQuestion.questionDatabankName;

//         try
//         {
//             int newScore = currentUserData.Score + scoreToAdd;
//             await FirestoreRepository.Instance.UpdateUserScore(
//                 currentUserData.UserId,
//                 newScore,
//                 currentQuestion.questionNumber,
//                 databankName,
//                 isCorrect
//             );

//             Debug.Log($"Score atualizado com sucesso: {newScore}");
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"Error updating score: {e.Message}");
//         }
//     }

//     private IEnumerator SmoorhTransitionCoroutine(TaskCompletionSource<bool> tcs)
//     {
//         Debug.Log("Starting Smooth transition");
//         questionCanvasGroup.gameObject.SetActive(true);
//         questionCanvasGroup.alpha = 0f;
//         questionCanvasGroup.interactable = false;
//         questionCanvasGroup.blocksRaycasts = false;

//         loadingCanvasGroup.gameObject.SetActive(true);
//         loadingCanvasGroup.alpha = 1f;
//         loadingCanvasGroup.interactable = true;
//         loadingCanvasGroup.blocksRaycasts = true;

//         yield return new WaitForSeconds(0.1f);

//         float elapsedTime = 0f;
//         float duration = 0.5f;

//         while (elapsedTime < duration)
//         {
//             float normalizedTime = elapsedTime / duration;
//             loadingCanvasGroup.alpha = 1f - normalizedTime;
//             yield return null;
//             elapsedTime += Time.deltaTime;
//         }
//         loadingCanvasGroup.alpha = 0f;
//         loadingCanvasGroup.interactable = false;
//         loadingCanvasGroup.blocksRaycasts = false;
//         loadingCanvasGroup.gameObject.SetActive(false);
//         Debug.Log("Loading fade out completed.");

//         questionCanvasGroup.alpha = 1f;
//         questionCanvasGroup.interactable = true;
//         questionCanvasGroup.blocksRaycasts = true;

//         tcs.SetResult(true);
//     }

//     private async Task LoadQuestionsForCurrentSceneAsync()
//     {
//         MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
//         QuestionSet targetSet = QuestionSetManager.GetCurrentQuestionSet();
//         Debug.Log($"Attempting to load questions for set {targetSet}");

//         foreach (MonoBehaviour behaviour in allBehaviours)
//         {
//             if (behaviour is IQuestionDatabase database && database.GetQuestionSetType() == targetSet)
//             {
//                 await LoadQuestionsFromDatabase(database);
//                 return;
//             }
//         }
//     }

//     private async Task LoadQuestionsFromDatabase(IQuestionDatabase database)
//     {
//         try
//         {
//             // Mantém o mesmo database durante toda a sessão
//             List<Question> allQuestions = database.GetQuestions();

//             if (string.IsNullOrEmpty(databankName))
//             {
//                 // Só atualiza o databankName na primeira vez
//                 databankName = database.GetDatabankName();
//             }

//             Debug.Log($"Carregando questões do banco {databankName}");

//             // Usa o AnsweredQuestionsManager para obter questões já respondidas corretamente
//             List<string> answeredQuestions = await answeredQuestionsManager.FetchUserAnsweredQuestionsInTargetDatabase(databankName);
//             Debug.Log($"Questões já respondidas corretamente: {answeredQuestions.Count}");

//             // Filtra para obter apenas questões não respondidas corretamente do database atual
//             List<Question> unansweredQuestions = allQuestions
//                 .Where(q => !answeredQuestions.Contains(q.questionNumber.ToString()))
//                 .ToList();

//             Debug.Log($"Questões não respondidas ainda: {unansweredQuestions.Count} no banco {databankName}");
//             questions = unansweredQuestions;
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"Erro em LoadQuestionsFromDatabase: {e.Message}");
//             throw;
//         }
//     }

//     private List<Question> SelectRandomQuestions(List<Question> allQuestions, int count)
//     {
//         if (allQuestions.Count <= count)
//         {
//             return new List<Question>(allQuestions);
//         }

//         return allQuestions.OrderBy(q => UnityEngine.Random.value).Take(count).ToList();
//     }

//     private void ShowQuestion(int index)
//     {
//         Debug.Log($"Attempting to show question at index: {index}");
//         if (timerText == null)
//         {
//             Debug.LogError("Timer Text is null in ShowQuestion");
//             return;
//         }

//         StartTimer();

//         if (questions == null || questions.Count == 0 || index >= questions.Count)
//         {
//             Debug.LogError("Invalid question data");
//             return;
//         }

//         Question currentQuestion = questions[index];
//         questionText.text = currentQuestion.questionText;

//         if (currentQuestion.isImageAnswer)
//         {
//             questionTextBackground.gameObject.SetActive(true);
//             ShowImageAnswers(currentQuestion);
//         }
//         else
//         {
//             questionTextBackground.gameObject.SetActive(true);
//             ShowTextAnswers(currentQuestion);
//         }
//     }

//     private async void ShowImageAnswers(Question question)
//     {
//         questionCanvasGroup.gameObject.SetActive(false);
//         questionImageCanvasGroup.gameObject.SetActive(true);

//         for (int i = 0; i < imageAnswerButtons.Length && i < question.answers.Length; i++)
//         {
//             if (imageAnswerButtons[i] == null)
//             {
//                 Debug.LogError($"Image answer button at index {i} is null");
//                 continue;
//             }

//             Image buttonImage = imageAnswerButtons[i].GetComponent<Image>();
//             if (buttonImage == null)
//             {
//                 Debug.LogError($"Image component not found on image answer button at index {i}");
//                 continue;
//             }

//             imageAnswerButtons[i].interactable = true;

//             string imagePath = question.answers[i];
//             await LoadImageAsync(buttonImage, imagePath);

//             int capturedIndex = i;
//             imageAnswerButtons[i].onClick.RemoveAllListeners();
//             imageAnswerButtons[i].onClick.AddListener(() => CheckAnswer(capturedIndex));

//             Debug.Log($"Button {i} configured with listener for index {capturedIndex}");
//         }

//         questionImageCanvasGroup.interactable = true;
//         questionImageCanvasGroup.blocksRaycasts = true;
//     }

//     private void ShowTextAnswers(Question question)
//     {
//         questionCanvasGroup.gameObject.SetActive(true);
//         questionImageCanvasGroup.gameObject.SetActive(false);

//         for (int i = 0; i < textAnswerButtons.Length && i < question.answers.Length; i++)
//         {
//             if (textAnswerButtons[i] == null)
//             {
//                 Debug.LogError($"Text answer button at index {i} is null");
//                 continue;
//             }

//             TextMeshProUGUI buttonText = textAnswerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
//             if (buttonText == null)
//             {
//                 Debug.LogError($"Text component not found on text answer button at index {i}");
//                 continue;
//             }

//             buttonText.text = question.answers[i];
//             int capturedIndex = i;
//             textAnswerButtons[i].onClick.RemoveAllListeners();
//             textAnswerButtons[i].onClick.AddListener(() => CheckAnswer(capturedIndex));
//             textAnswerButtons[i].interactable = true;
//         }
//     }

//     private async Task LoadImageAsync(Image targetImage, string imagePath)
//     {
//         if (string.IsNullOrEmpty(imagePath))
//         {
//             Debug.LogError("Image path is null or empty");
//             return;
//         }

//         try
//         {
//             string resourcePath = imagePath
//                 .Replace("Assets/Images/AnswerImages/", "")
//                 .Replace("Assets/", "")
//                 .Replace(".png", "");

//             Debug.Log($"Attempting to load image from Resources path: {resourcePath}");

//             await Task.Yield();
//             Sprite sprite = Resources.Load<Sprite>(resourcePath);

//             if (sprite != null)
//             {
//                 targetImage.sprite = sprite;
//                 Debug.Log($"Successfully loaded sprite: {resourcePath}");
//             }
//             else
//             {
//                 Debug.LogError($"Failed to load sprite from Resources path: {resourcePath}");
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"Error Loading Image: {e.Message}");
//         }
//     }

//     public async void CheckAnswer(int selectedAnswerIndex)
//     {
//         timerRunning = false;
//         StopAllCoroutines();
//         DisableAnswerButtons();

//         Button selectedButton = questions[currentQuestionIndex].isImageAnswer
//                 ? imageAnswerButtons[selectedAnswerIndex]
//                 : textAnswerButtons[selectedAnswerIndex];

//         bool isCorrect = selectedAnswerIndex == questions[currentQuestionIndex].correctIndex;

//         if (selectedButton != null)
//         {
//             if (isCorrect)
//             {
//                 ShowFeedback("Resposta correta!\n+5 pontos", isCorrect, false);
//                 await UpdateScore(5, true);
//             }
//             else
//             {
//                 selectedButton.GetComponent<Image>().color = HexToColor("#D3A3A3");
//                 ShowFeedback("Resposta errada!\n -2 Pontos.", isCorrect, false);
//                 await UpdateScore(-2, false);
//             }

//             // Ativa os botões somente após a atualização do score
//             EnableNavigationButtons();
//         }
//     }

//     private void EnableNavigationButtons()
//     {
//         exitButton.interactable = true;
//         nextQuestionButton.interactable = true;

//         exitButton.onClick.RemoveAllListeners();
//         nextQuestionButton.onClick.RemoveAllListeners();

//         exitButton.onClick.AddListener(() =>
//         {
//             HideFeedback();
//             BackToPathwayScene();
//         });

//         nextQuestionButton.onClick.AddListener(() =>
//         {
//             HideFeedback();
//             HandleNextQuestion();

//         });
//     }

//     private void HandleNextQuestion()
//     {
//         Debug.Log($"HandleNextQuestion - currentQuestionIndex: {currentQuestionIndex}, questions.Count: {questions.Count}");

//         exitButton.interactable = false;
//         nextQuestionButton.interactable = false;

//         // Não precisa fazer ResetButtonStates aqui pois será feito no ShowQuestion
//         LoadNextScene();
//     }

//     private void HideFeedback()
//     {
//         if (feedbackPanel != null)
//         {
//             feedbackPanel.gameObject.SetActive(false);
//         }
//     }

//     private void nextQuestion()
//     {
//         HideFeedback();
//         exitButton.interactable = false;
//         nextQuestionButton.interactable = false;
//         ResetButtonStates();

//         if (currentQuestionIndex < questions.Count)
//         {
//             ShowQuestion(currentQuestionIndex);
//         }
//         else
//         {
//             LoadNextScene();
//         }
//     }

//     private void ResetButtonStates()
//     {
//         EventSystem.current.SetSelectedGameObject(null);

//         Button[] currentButtons = questions[currentQuestionIndex].isImageAnswer
//             ? imageAnswerButtons
//             : textAnswerButtons;

//         for (int i = 0; i < currentButtons.Length; i++)
//         {
//             if (currentButtons[i] != null)
//             {
//                 Button button = currentButtons[i];
//                 button.transition = Selectable.Transition.ColorTint;
//                 button.interactable = true;

//                 ColorBlock cb = button.colors;
//                 cb.normalColor = HexToColor("#D9EAF3");
//                 cb.highlightedColor = HexToColor("#C2DCEB");
//                 cb.pressedColor = HexToColor("#A3C6D3");
//                 cb.selectedColor = HexToColor("#BFD9F0");
//                 cb.disabledColor = HexToColor("#E9F3FA");
//                 button.colors = cb;

//                 button.OnDeselect(null);
//                 button.onClick.RemoveAllListeners();
//                 int index = i;
//                 button.onClick.AddListener(() => CheckAnswer(index));
//                 button.GetComponent<Image>().color = HexToColor("#D9EAF3");
//             }
//         }

//         Canvas.ForceUpdateCanvases();
//     }

//     private async void LoadNextScene()
//     {
//         Debug.Log($"LoadNextScene - Index atual: {currentQuestionIndex}, Total questões: {questions.Count}");

//         // Para o timer atual
//         timerRunning = false;
//         StopAllCoroutines();

//         if (currentQuestionIndex >= questions.Count - 1)
//         {
//             // Chegamos ao final da lista atual
//             MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
//             IQuestionDatabase currentDatabase = null;

//             foreach (MonoBehaviour behaviour in allBehaviours)
//             {
//                 if (behaviour is IQuestionDatabase database && database.GetDatabankName() == databankName)
//                 {
//                     currentDatabase = database;
//                     break;
//                 }
//             }

//             if (currentDatabase == null)
//             {
//                 Debug.LogError($"Database {databankName} não encontrada");
//                 return;
//             }

//             // Desabilita os botões durante o carregamento
//             DisableUIElements();

//             // Recarrega a lista de questões não respondidas do mesmo database
//             await LoadQuestionsFromDatabase(currentDatabase);

//             if (questions == null || questions.Count == 0)
//             {
//                 ShowCompletionFeedback();
//                 return;
//             }

//             // Verificação extra usando o AnsweredQuestionsManager para o mesmo database
//             bool hasRemaining = await answeredQuestionsManager.HasRemainingQuestions(
//                 databankName,
//                 questions.Select(q => q.questionNumber.ToString()).ToList()
//             );

//             if (!hasRemaining)
//             {
//                 ShowCompletionFeedback();
//                 return;
//             }

//             // Reseta para a primeira questão da nova lista do mesmo database
//             currentQuestionIndex = 0;
//             Debug.Log($"Carregando nova lista com {questions.Count} questões do banco {databankName}");
//             await TransitionToQuestions();
//             ShowQuestion(currentQuestionIndex);
//         }
//         else
//         {
//             // Ainda há questões na lista atual
//             currentQuestionIndex++;
//             await FirestoreRepository.Instance.UpdateUserProgress(currentUserData.UserId, currentQuestionIndex);
//             ShowQuestion(currentQuestionIndex);
//         }
//     }

//     private void DisableUIElements()
//     {
//         if (exitButton != null) exitButton.interactable = false;
//         if (nextQuestionButton != null) nextQuestionButton.interactable = false;

//         foreach (var button in textAnswerButtons)
//         {
//             if (button != null) button.interactable = false;
//         }

//         foreach (var button in imageAnswerButtons)
//         {
//             if (button != null) button.interactable = false;
//         }
//     }

//     private void ShowCompletionFeedback()
//     {
//         Debug.Log("Todas as questões foram respondidas corretamente");
//         ShowFeedback("Parabéns!! Você respondeu todas as perguntas desta lista corretamente!", false, true);
//         bottomBar.gameObject.SetActive(false);
//         questionTextBackground.gameObject.SetActive(false);
//     }

//     private async Task CheckRemainingQuestions()
//     {
//         Debug.Log("Iniciando CheckRemainingQuestions...");

//         List<Question> remainingQuestions = await GetRemainingQuestionsFromFirestore();

//         if (remainingQuestions == null)
//         {
//             ShowFeedback("Erro ao carregar questões. Tente novamente.", false, false);
//             return;
//         }

//         if (remainingQuestions.Count > 0)
//         {
//             questions = remainingQuestions;
//             currentQuestionIndex = 0;
//             ShowQuestion(currentQuestionIndex);
//         }
//         else
//         {
//             ShowFeedback("Parabéns!! Você respondeu todas as perguntas desta lista", false, true);
//             bottomBar.gameObject.SetActive(false);
//             questionTextBackground.gameObject.SetActive(false);
//         }
//     }

//     private async Task<List<Question>> GetRemainingQuestionsFromFirestore()
//     {
//         Debug.Log("Iniciando GetRemainingQuestionsFromFirestore...");

//         if (answeredQuestionsManager == null)
//         {
//             Debug.LogError("AnsweredQuestionsManager não está disponível!");
//             return null;
//         }

//         try
//         {
//             List<string> savedAnsweredQuestions = await answeredQuestionsManager.FetchUserAnsweredQuestionsInTargetDatabase(databankName);

//             await LoadQuestionsForCurrentSceneAsync();

//             if (questions == null)
//             {
//                 Debug.LogError("Falha ao carregar questões do banco de dados!");
//                 return null;
//             }

//             if (savedAnsweredQuestions == null)
//             {
//                 return questions;
//             }

//             List<Question> remainingQuestions = questions.Where(q => !savedAnsweredQuestions.Contains(q.questionNumber.ToString())).ToList();

//             if (remainingQuestions.Count > 0)
//             {
//                 return remainingQuestions;
//             }
//             else
//             {
//                 return new List<Question>();
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"Erro ao buscar questões restantes: {e.Message}");
//             Debug.LogException(e);
//             return null;
//         }
//     }

//     private void ShowFeedback(string message, bool isCorrect, bool isCompleted)
//     {
//         if (isCompleted)
//         {

//             questionsCompletedFeedbackText.text = message;
//             exitButton.gameObject.SetActive(false);
//             nextQuestionButton.gameObject.SetActive(false);
//             timePanel.gameObject.SetActive(false);
//             timerText.gameObject.SetActive(false);
//             loadingCanvasGroup.gameObject.SetActive(false);
//             questionCanvasGroup.gameObject.SetActive(false);
//             questionTextBackground.gameObject.SetActive(false);
//             questionImageCanvasGroup.gameObject.SetActive(false);
//             questionsCompletedFeedback.gameObject.SetActive(true);
//             return;
//         }

//         if (feedbackText == null || feedbackPanel == null)
//         {
//             Debug.LogError("Feedback components are not assigned!");
//             return;
//         }

//         if (feedbackCoroutine != null)
//         {
//             StopCoroutine(feedbackCoroutine);
//         }

//         feedbackText.text = message;
//         feedbackPanel.gameObject.SetActive(true);

//         Color backgroundColor = isCorrect ? HexToColor("#D4EDDA") : HexToColor("#F8D7DA");
//         feedbackPanel.color = backgroundColor;

//         Color shadowColor = isCorrect ? HexToColor("#A0D6B5") : HexToColor("#E8A8A8");
//         Shadow shadow = feedbackPanel.GetComponent<Shadow>();
//         if (shadow != null)
//         {
//             shadow.effectColor = shadowColor;
//         }

//         Color fontColor = isCorrect ? HexToColor("#28A745") : HexToColor("#721C24");
//         feedbackText.color = fontColor;

//         feedbackCoroutine = StartCoroutine(ShowFeedbackCoroutine());
//     }

//     // private IEnumerator ShowFeedbackCoroutine()
//     // {
//     //     float elapsedTime = 0f;

//     //     while (elapsedTime < fadeDuration)
//     //     {
//     //         float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
//     //         feedbackPanel.color = new Color(feedbackPanel.color.r, feedbackPanel.color.g, feedbackPanel.color.b, alpha);
//     //         elapsedTime += Time.deltaTime;
//     //         yield return null;
//     //     }

//     //     yield return new WaitForSeconds(displayDuration);

//     //     elapsedTime = 0f;
//     //     while (elapsedTime < fadeDuration)
//     //     {
//     //         float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
//     //         Color currentColor = feedbackPanel.color;
//     //         feedbackPanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
//     //         elapsedTime += Time.deltaTime;
//     //         yield return null;
//     //     }

//     //     feedbackPanel.gameObject.SetActive(false);
//     //     feedbackCoroutine = null;
//     // }

//     private IEnumerator ShowFeedbackCoroutine()
//     {
//         float elapsedTime = 0f;

//         // Apenas fade in do feedback
//         while (elapsedTime < fadeDuration)
//         {
//             float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
//             feedbackPanel.color = new Color(feedbackPanel.color.r, feedbackPanel.color.g, feedbackPanel.color.b, alpha);
//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         // Mantém o feedback visível (não faz fade out)
//         feedbackCoroutine = null;
//     }


//     private void NavigateToResetDatabaseScene(string databankName)
//     {
//         Dictionary<string, object> sceneData = new Dictionary<string, object>
//     {
//         { "databankName", databankName }
//     };

//         NavigationManager.Instance.NavigateTo("ResetDatabaseView", sceneData);
//     }

//     private Color HexToColor(string hex)
//     {
//         Color color = new Color();
//         ColorUtility.TryParseHtmlString(hex, out color);
//         return color;
//     }

//     public async void BackToPathwayScene()
//     {
//          HideFeedback();
        
//         // Força uma atualização antes de voltar para a PathwayScene
//         if (answeredQuestionsManager != null)
//         {
//             await answeredQuestionsManager.ForceUpdate();
//         }
        
//         Debug.Log("Chamando PathwayScene dentro de BackToPathwayScene()");
//         NavigationManager.Instance.NavigateTo("PathwayScene");

//     }


//     private void StartTimer()
//     {
//         if (timerText == null)
//         {
//             Debug.LogError("Timer Text is null");
//             return;
//         }
//         Debug.Log("Timer started");
//         timeLeft = 20f;
//         timerRunning = true;
//         UpdateTimerDisplay();
//         StopAllCoroutines();
//         StartCoroutine(TimerCoroutine());
//     }

//     private IEnumerator TimerCoroutine()
//     {
//         Debug.Log($"Timer left: {timeLeft}");
//         while (timerRunning && timeLeft > 0)
//         {
//             Debug.Log("TimerCoroutine started");
//             yield return new WaitForSeconds(1f);
//             timeLeft -= 1f;
//             UpdateTimerDisplay();
//         }

//         if (timeLeft <= 0)
//         {
//             Debug.Log($"Timer finished");
//             timerRunning = false;
//             yield return new WaitForSeconds(1f);
//             ShowFeedback("Tempo Esgotado!\n-1 ponto", false, false);
//             StartCoroutine(HandleTimerFinish());
//         }
//     }

//     private IEnumerator HandleTimerFinish()
//     {
//         DisableAnswerButtons();
//         ShowFeedback("Tempo Esgotado!\n-1 ponto", false, false);

//         // Aguarda a atualização do score
//         yield return UpdateScore(-1, false);

//         // Ativa os botões após a atualização do score
//         EnableNavigationButtons();
//     }


//     private IEnumerator UpdateScoreAndShowButtons(int scoreToAdd)
//     {
//         yield return UpdateScore(scoreToAdd, false);
//         exitButton.interactable = true;
//         nextQuestionButton.interactable = true;
//         Debug.Log("Score updated");
//     }

//     private void UpdateTimerDisplay()
//     {
//         if (timerText != null)
//         {
//             timerText.text = $"{Mathf.Ceil(timeLeft)}";
//             Debug.Log($"Time display updated: {timerText.text}");
//         }
//         else
//         {
//             Debug.LogError("Time Text is still null in UpdateTimerDisplay");
//         }
//     }

//     private void DisableAnswerButtons()
//     {
//         if (questions == null || currentQuestionIndex >= questions.Count)
//         {
//             Debug.LogWarning("Tentativa de desabilitar botões com índice inválido");
//             return;
//         }

//         if (questions[currentQuestionIndex].isImageAnswer)
//         {
//             foreach (var button in imageAnswerButtons)
//             {
//                 if (button != null)
//                 {
//                     button.interactable = false;
//                 }
//             }
//         }
//         else
//         {
//             foreach (var button in textAnswerButtons)
//             {
//                 if (button != null)
//                 {
//                     button.interactable = false;
//                 }
//             }
//         }
//     }

//     private IEnumerator EnableButtonsAfterFeedback()
//     {
//         yield return new WaitForSeconds(displayDuration + fadeDuration);
//         exitButton.interactable = true;
//         nextQuestionButton.interactable = true;
//         exitButton.onClick.AddListener(BackToPathwayScene);
//         nextQuestionButton.onClick.AddListener(nextQuestion);
//     }
// }
