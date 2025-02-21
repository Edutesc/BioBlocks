using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingFeedbackQuestionController : MonoBehaviour, IFeedbackQuestionController
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI helperText;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private Slider ratingSlider;
    [SerializeField] private Image[] starImages;
    [SerializeField] private TextMeshProUGUI minLabel;
    [SerializeField] private TextMeshProUGUI maxLabel;
    [SerializeField] private TextMeshProUGUI currentValueText;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private Color starActiveColor = Color.yellow;
    [SerializeField] private Color starInactiveColor = Color.gray;

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

        ratingSlider.minValue = 1;
        ratingSlider.maxValue = question.maxRating;
        ratingSlider.wholeNumbers = true;
        ratingSlider.value = 1;

        minLabel.text = question.minRatingLabel;
        maxLabel.text = question.maxRatingLabel;

        // Mostra apenas o número correto de estrelas
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].gameObject.SetActive(i < question.maxRating);
            starImages[i].color = (i == 0) ? starActiveColor : starInactiveColor;
        }

        ratingSlider.onValueChanged.AddListener(UpdateRatingUI);
        UpdateRatingUI(ratingSlider.value);

        errorText.gameObject.SetActive(false);
    }

    private void UpdateRatingUI(float value)
    {
        int rating = Mathf.RoundToInt(value);
        currentValueText.text = rating.ToString();

        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < starImages.Length)
            {
                starImages[i].color = (i < rating) ? starActiveColor : starInactiveColor;
            }
        }
    }

    public bool Validate()
    {
        // Rating sempre será válido pois o valor mínimo é 1
        return true;
    }

    public KeyValuePair<string, object> GetResult()
    {
        return new KeyValuePair<string, object>(questionData.id, Mathf.RoundToInt(ratingSlider.value));
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void ClearAnswer()
    {
        if (ratingSlider != null)
        {
            ratingSlider.value = ratingSlider.minValue;
        }

        if (starImages != null)
        {
            foreach (var star in starImages)
            {
                if (star != null)
                    star.color = starInactiveColor;
            }
        }

        if (currentValueText != null)
        {
            currentValueText.text = ratingSlider.minValue.ToString();
        }

        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
    }
}
