using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFeedbackQuestionController : MonoBehaviour, IFeedbackQuestionController
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI helperText;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private Toggle trueToggle;
    [SerializeField] private Toggle falseToggle;
    [SerializeField] private TextMeshProUGUI trueLabel;
    [SerializeField] private TextMeshProUGUI falseLabel;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private ToggleGroup toggleGroup;

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

        trueLabel.text = question.toggleTrueLabel;
        falseLabel.text = question.toggleFalseLabel;

        trueToggle.group = toggleGroup;
        falseToggle.group = toggleGroup;

        errorText.gameObject.SetActive(false);
    }

    public bool Validate()
    {
        bool isValid = !questionData.isRequired || (trueToggle.isOn || falseToggle.isOn);
        errorText.gameObject.SetActive(!isValid);
        errorText.text = isValid ? "" : "Esta resposta é obrigatória";
        return isValid;
    }

    public KeyValuePair<string, object> GetResult()
    {
        if (!trueToggle.isOn && !falseToggle.isOn)
            return new KeyValuePair<string, object>(questionData.id, null);

        return new KeyValuePair<string, object>(questionData.id, trueToggle.isOn);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void ClearAnswer()
    {
        if (trueToggle != null)
            trueToggle.isOn = false;

        if (falseToggle != null)
            falseToggle.isOn = false;

        if (toggleGroup != null)
            toggleGroup.SetAllTogglesOff();

        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }
}