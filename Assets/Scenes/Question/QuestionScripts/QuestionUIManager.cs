using UnityEngine;
using UnityEngine.UI;
using QuestionSystem;
using System;
using System.Collections;
using UnityEngine.Events;

public class QuestionUIManager : MonoBehaviour
{
    [SerializeField] private QuestionCanvasGroups canvasGroups;
    [SerializeField] private QuestionUIElements uiElements;
    [SerializeField] private FeedbackUIElements feedbackElements;
    [SerializeField] private Image questionImage;
    private Coroutine feedbackCoroutine;

    private void Start()
    {
        ValidateComponents();
        InitializeUI();
        UpdateUserInfo();

        UserDataStore.OnUserDataChanged += OnUserDataChanged;
    }

    private void ValidateComponents()
    {
        if (canvasGroups == null)
        {
            Debug.LogError("QuestionCanvasGroups não está atribuído no QuestionUIManager");
            return;
        }

        if (uiElements == null)
        {
            Debug.LogError("QuestionUIElements não está atribuído no QuestionUIManager");
            return;
        }

        if (feedbackElements == null)
        {
            Debug.LogError("FeedbackUIElements não está atribuído no QuestionUIManager");
            return;
        }

        canvasGroups.ValidateComponents();
        uiElements.ValidateComponents();
        feedbackElements.ValidateComponents();
    }

    private void InitializeUI()
    {
        try
        {
            if (uiElements != null)
            {
                if (uiElements.ExitButton != null)
                    uiElements.ExitButton.gameObject.SetActive(true);

                if (uiElements.NextQuestionButton != null)
                    uiElements.NextQuestionButton.gameObject.SetActive(true);

                if (uiElements.ExitButton != null)
                    uiElements.ExitButton.interactable = false;

                if (uiElements.NextQuestionButton != null)
                    uiElements.NextQuestionButton.interactable = false;

                if (uiElements.TimePanel != null)
                    uiElements.TimePanel.SetActive(true);
            }

            if (canvasGroups != null)
            {
                if (canvasGroups.QuestionsCompletedFeedback != null)
                    canvasGroups.QuestionsCompletedFeedback.gameObject.SetActive(false);

                if (canvasGroups.BottomBar != null)
                    canvasGroups.BottomBar.gameObject.SetActive(true);

                if (canvasGroups.QuestionTextBackground != null)
                    canvasGroups.QuestionTextBackground.gameObject.SetActive(true);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao inicializar UI: {e.Message}");
        }
    }

    private void OnDestroy()
    {
        // Remove a inscrição quando o objeto for destruído
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
    }

    private void UpdateUserInfo()
    {
        if (UserDataStore.CurrentUserData != null)
        {
            UpdateScore(UserDataStore.CurrentUserData.Score);
            UpdateName(UserDataStore.CurrentUserData.NickName);
            Debug.Log($"UI atualizada - Nome: {UserDataStore.CurrentUserData.NickName}, Score: {UserDataStore.CurrentUserData.Score}");
        }
        else
        {
            Debug.LogError("CurrentUserData é null ao tentar atualizar UI");
        }
    }

    private void OnUserDataChanged(UserData userData)
    {
        if (userData != null)
        {
            UpdateScore(userData.Score);
            UpdateName(userData.NickName);
            Debug.Log($"UI atualizada após mudança - Nome: {userData.NickName}, Score: {userData.Score}");
        }
    }

    public void UpdateScore(int score)
    {
        if (uiElements != null && uiElements.ScoreText != null)
        {
            uiElements.ScoreText.text = $"{score} XP";
            Debug.Log($"Score atualizado para: {score}");
        }
        else
        {
            Debug.LogError("Não foi possível atualizar o score - componentes null");
        }
    }

    public void UpdateName(string name)
    {
        if (uiElements != null && uiElements.NameText != null)
        {
            uiElements.NameText.text = name;
            Debug.Log($"Nome atualizado para: {name}");
        }
        else
        {
            Debug.LogError("Não foi possível atualizar o nome - componentes null");
        }
    }

    public void ShowQuestion(Question question)
    {
        // Controla o formato da pergunta (texto ou imagem)
        if (question.isImageQuestion)
        {
            // Configura para exibir a pergunta como imagem
            canvasGroups.QuestionTextBackground.alpha = 0f;
            canvasGroups.QuestionImageContainer.alpha = 1f;

            // Carrega a imagem da questão
            if (!string.IsNullOrEmpty(question.questionImagePath))
            {
                StartCoroutine(LoadQuestionImage(question.questionImagePath));
            }
            else
            {
                Debug.LogError("Question marked as image question but no image path provided");
            }
        }
        else
        {
            // Configura para exibir a pergunta como texto
            canvasGroups.QuestionTextBackground.alpha = 1f;
            canvasGroups.QuestionImageContainer.alpha = 0f;
            uiElements.QuestionText.text = question.questionText;
        }

        // Controla o formato das respostas (texto ou imagem)
        if (question.isImageAnswer)
        {
            canvasGroups.QuestionCanvasGroup.gameObject.SetActive(false);
            canvasGroups.QuestionImageCanvasGroup.gameObject.SetActive(true);
            canvasGroups.QuestionImageCanvasGroup.interactable = true;
            canvasGroups.QuestionImageCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroups.QuestionCanvasGroup.gameObject.SetActive(true);
            canvasGroups.QuestionImageCanvasGroup.gameObject.SetActive(false);
        }
    }

    private void ShowImageQuestion(Question question)
    {
        // Desativa o background de texto da questão
        canvasGroups.QuestionTextBackground.gameObject.SetActive(false);

        // Carrega e exibe a imagem da questão
        if (!string.IsNullOrEmpty(question.questionImagePath))
        {
            StartCoroutine(LoadQuestionImage(question.questionImagePath));
        }
        else
        {
            Debug.LogError("Question marked as image question but no image path provided");
        }
    }

    private IEnumerator LoadQuestionImage(string imagePath)
    {
        if (questionImage == null)
        {
            Debug.LogError("QuestionImage component not assigned");
            yield break;
        }

        // Carrega a imagem de forma assíncrona
        var request = Resources.LoadAsync<Sprite>(imagePath);
        yield return request;

        if (request.asset == null)
        {
            Debug.LogError($"Failed to load question image at path: {imagePath}");
            yield break;
        }

        questionImage.sprite = request.asset as Sprite;
    }

    private void ShowTextQuestion(Question question)
    {
        // Ativa o background de texto e define o texto da questão
        canvasGroups.QuestionTextBackground.gameObject.SetActive(true);
        uiElements.QuestionText.text = question.questionText;

        // Garante que a imagem da questão está desativada
        if (questionImage != null)
        {
            questionImage.gameObject.SetActive(false);
        }
    }

    public void ShowFeedback(string message, bool isCorrect, bool isCompleted = false)
    {
        if (isCompleted)
        {
            ShowCompletionFeedback(message);
            return;
        }

        if (feedbackElements.FeedbackText == null || feedbackElements.FeedbackPanel == null)
        {
            Debug.LogError("Feedback components are not assigned!");
            return;
        }

        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);
        }

        SetupFeedback(message, isCorrect);
        feedbackCoroutine = StartCoroutine(ShowFeedbackCoroutine());
    }

    private void SetupFeedback(string message, bool isCorrect)
    {
        feedbackElements.FeedbackText.text = message;
        feedbackElements.FeedbackPanel.gameObject.SetActive(true);

        // Configurar cores do feedback
        Color backgroundColor = isCorrect ? HexToColor("#D4EDDA") : HexToColor("#F8D7DA");
        feedbackElements.FeedbackPanel.color = backgroundColor;

        Color shadowColor = isCorrect ? HexToColor("#A0D6B5") : HexToColor("#E8A8A8");
        Shadow shadow = feedbackElements.FeedbackPanel.GetComponent<Shadow>();
        if (shadow != null)
        {
            shadow.effectColor = shadowColor;
        }

        Color fontColor = isCorrect ? HexToColor("#28A745") : HexToColor("#721C24");
        feedbackElements.FeedbackText.color = fontColor;
    }

    private void ShowCompletionFeedback(string message)
    {
        feedbackElements.QuestionsCompletedFeedbackText.text = message;
        DisableAllUIElements();
        canvasGroups.QuestionsCompletedFeedback.gameObject.SetActive(true);
    }

    public void DisableAllUIElements()
    {
        uiElements.ExitButton.gameObject.SetActive(false);
        uiElements.NextQuestionButton.gameObject.SetActive(false);
        uiElements.TimePanel.SetActive(false);
        canvasGroups.LoadingCanvasGroup.gameObject.SetActive(false);
        canvasGroups.QuestionCanvasGroup.gameObject.SetActive(false);
        canvasGroups.QuestionTextBackground.gameObject.SetActive(false);
        canvasGroups.QuestionImageCanvasGroup.gameObject.SetActive(false);
    }

    public void HideFeedback()
    {
        if (feedbackElements.FeedbackPanel != null)
        {
            feedbackElements.FeedbackPanel.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowFeedbackCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < feedbackElements.FadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / feedbackElements.FadeDuration);
            feedbackElements.FeedbackPanel.color = new Color(
                feedbackElements.FeedbackPanel.color.r,
                feedbackElements.FeedbackPanel.color.g,
                feedbackElements.FeedbackPanel.color.b,
                alpha
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        feedbackCoroutine = null;
    }

    private Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

    public void SetupNavigationButtons(UnityAction exitAction, UnityAction nextAction)
    {
        if (uiElements == null)
        {
            Debug.LogError("UIElements não está atribuído");
            return;
        }

        // Remove listeners anteriores
        uiElements.ExitButton.onClick.RemoveAllListeners();
        uiElements.NextQuestionButton.onClick.RemoveAllListeners();

        // Adiciona novos listeners
        uiElements.ExitButton.onClick.AddListener(exitAction);
        uiElements.NextQuestionButton.onClick.AddListener(nextAction);
    }

    public void EnableNavigationButtons()
    {
        if (uiElements == null)
        {
            Debug.LogError("UIElements não está atribuído");
            return;
        }

        uiElements.ExitButton.interactable = true;
        uiElements.NextQuestionButton.interactable = true;
    }

    public void DisableNavigationButtons()
    {
        if (uiElements == null)
        {
            Debug.LogError("UIElements não está atribuído");
            return;
        }

        uiElements.ExitButton.interactable = false;
        uiElements.NextQuestionButton.interactable = false;
    }
}
