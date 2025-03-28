using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedbackUIElements : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private Image feedbackPanel;
    [SerializeField] private TextMeshProUGUI questionsCompletedFeedbackText;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 1.5f;

    // Propriedades públicas
    public TextMeshProUGUI FeedbackText => feedbackText;
    public Image FeedbackPanel => feedbackPanel;
    public TextMeshProUGUI QuestionsCompletedFeedbackText => questionsCompletedFeedbackText;
    public float FadeDuration => fadeDuration;
    public float DisplayDuration => displayDuration;

    public void ValidateComponents()
    {
        if (feedbackText == null) Debug.LogError("FeedbackText não atribuído");
        if (feedbackPanel == null) Debug.LogError("FeedbackPanel não atribuído");
        if (questionsCompletedFeedbackText == null) Debug.LogError("QuestionsCompletedFeedbackText não atribuído");
    }
}
