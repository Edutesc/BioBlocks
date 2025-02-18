using UnityEngine;
using UnityEngine.UI;
using QuestionSystem;
using System.Collections;
using TMPro;

public class QuestionUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image questionImage;

    private void Start()
    {
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (questionText == null) Debug.LogError("QuestionText não atribuído");
        if (questionImage == null) Debug.LogError("QuestionImage não atribuído");
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
    }

    private void ShowImageQuestion(Question question)
    {
        if (!string.IsNullOrEmpty(question.questionImagePath))
        {
            StartCoroutine(LoadQuestionImage(question.questionImagePath));
        }
    }

    private void ShowTextQuestion(Question question)
    {
        questionText.text = question.questionText;
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