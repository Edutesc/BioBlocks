using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFeedbackQuestionController : MonoBehaviour, IFeedbackQuestionController
{
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI helperText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Character Limit Settings")]
    [SerializeField] private bool useDefaultCharLimit = true;
    [SerializeField] private int defaultCharLimit = 1000;

    private FeedbackQuestion questionData;

    private void OnValidate()
    {
        // Isso será chamado no Editor sempre que houver alterações no Inspector
        if (inputField != null && useDefaultCharLimit)
        {
            inputField.characterLimit = defaultCharLimit;
        }
    }

    private void Awake()
    {
        if (useDefaultCharLimit && inputField != null)
        {
            inputField.characterLimit = defaultCharLimit;
        }
    }

    public void SetupQuestion(FeedbackQuestion question)
    {
        questionData = question;
        questionText.text = question.titleText;

        if (categoryText != null)
        {
            categoryText.gameObject.SetActive(!string.IsNullOrEmpty(question.category));
            categoryText.text = question.category;
        }

        helperText.gameObject.SetActive(!string.IsNullOrEmpty(question.helperText));
        helperText.text = question.helperText;

        if (useDefaultCharLimit)
        {
            inputField.characterLimit = defaultCharLimit;
        }
        else if (question.maxCharacters > 0)
        {
            inputField.characterLimit = question.maxCharacters;
        }

        if (!string.IsNullOrEmpty(question.placeholderText))
        {
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = question.placeholderText;
        }

        errorText.gameObject.SetActive(false);
    }

    public bool Validate()
    {
        bool isValid = !questionData.isRequired || !string.IsNullOrEmpty(inputField.text);
        errorText.gameObject.SetActive(!isValid);
        errorText.text = isValid ? "" : "Esta resposta é obrigatória";
        return isValid;
    }

    public KeyValuePair<string, object> GetResult()
    {
        return new KeyValuePair<string, object>(questionData.id, inputField.text);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void ClearAnswer()
    {
        if (inputField != null)
            inputField.text = "";
    }
}