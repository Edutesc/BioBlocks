using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFeedbackQuestionController : MonoBehaviour, IFeedbackQuestionController
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI helperText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI categoryText;
    
    private FeedbackQuestion questionData;
    
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
        
        inputField.characterLimit = question.maxCharacters;
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = 
            !string.IsNullOrEmpty(question.placeholderText) ? question.placeholderText : "Digite sua resposta...";
        
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
}
