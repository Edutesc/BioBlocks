using UnityEngine;
using UnityEngine.UI;
using QuestionSystem;
using System.Collections;
using TMPro;

public class QuestionUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup questionTextContainer;
    [SerializeField] private CanvasGroup questionImageContainer;
    [SerializeField] private CanvasGroup answerTextCanvasGroup;
    [SerializeField] private CanvasGroup answerImageCanvasGroup;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image questionImage;

    private void Start()
    {
        ValidateComponents();
        InitializeUI();
    }

    private void ValidateComponents()
    {
        if (questionTextContainer == null) Debug.LogError("QuestionTextContainer não atribuído");
        if (questionImageContainer == null) Debug.LogError("QuestionImageContainer não atribuído");
        if (answerTextCanvasGroup == null) Debug.LogError("AnswerTextCanvasGroup não atribuído");
        if (answerImageCanvasGroup == null) Debug.LogError("AnswerImageCanvasGroup não atribuído");
        if (questionText == null) Debug.LogError("QuestionText não atribuído");
        if (questionImage == null) Debug.LogError("QuestionImage não atribuído");
    }

    private void InitializeUI()
    {
        questionTextContainer.gameObject.SetActive(true);
        answerTextCanvasGroup.gameObject.SetActive(true);
    }

    public void ShowQuestion(Question question)
    {
        if (question.isImageQuestion)
        {
            ShowImageQuestion(question);
        }
        else
        {
            ShowTextQuestion(question);
        }

        SetupAnswerFormat(question.isImageAnswer);
    }

    private void ShowImageQuestion(Question question)
    {
        questionTextContainer.alpha = 0f;
        questionImageContainer.alpha = 1f;

        if (!string.IsNullOrEmpty(question.questionImagePath))
        {
            StartCoroutine(LoadQuestionImage(question.questionImagePath));
        }
    }

    private void ShowTextQuestion(Question question)
    {
        questionTextContainer.alpha = 1f;
        questionImageContainer.alpha = 0f;
        questionText.text = question.questionText;
    }

    private void SetupAnswerFormat(bool isImageAnswer)
    {
        answerTextCanvasGroup.gameObject.SetActive(!isImageAnswer);
        answerImageCanvasGroup.gameObject.SetActive(isImageAnswer);
        
        if (isImageAnswer)
        {
            answerImageCanvasGroup.interactable = true;
            answerImageCanvasGroup.blocksRaycasts = true;
        }
    }

    private IEnumerator LoadQuestionImage(string imagePath)
    {
        var request = Resources.LoadAsync<Sprite>(imagePath);
        yield return request;

        if (request.asset != null)
        {
            questionImage.sprite = request.asset as Sprite;
        }
        else
        {
            Debug.LogError($"Failed to load question image at path: {imagePath}");
        }
    }
}