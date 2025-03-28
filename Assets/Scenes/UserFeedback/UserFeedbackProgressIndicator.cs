using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FeedbackProgressIndicator : MonoBehaviour
{
    [Header("Componentes UI")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressText;
    
    private UserFeedbackManager feedbackManager;
    private int totalFeedbackQuestions;
    private int currentQuestionIndex;
    
    private void Start()
    {
        feedbackManager = FindFirstObjectByType<UserFeedbackManager>();
        if (feedbackManager == null)
        {
            Debug.LogError("FeedbackProgressIndicator: Não foi possível encontrar o UserFeedbackManager");
            return;
        }
        
        feedbackManager.OnQuestionChanged.AddListener(UpdateProgress);
        feedbackManager.OnQuestionsLoaded.AddListener(InitializeProgressIndicator);
    }

    public void InitializeProgressIndicator(List<FeedbackQuestion> feedbackQuestions)
    {
        totalFeedbackQuestions = feedbackQuestions.Count;
        
        if (progressSlider != null)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = totalFeedbackQuestions - 1;
            progressSlider.value = 0;
        }
        
        UpdateProgress(0, feedbackQuestions[0]);
    }
    
    public void UpdateProgress(int index, FeedbackQuestion currentQuestion)
    {
        currentQuestionIndex = index;
        
        if (progressSlider != null)
        {
            progressSlider.value = index;
        }
        
        if (progressText != null)
        {
            progressText.text = $"Pergunta {index + 1} de {totalFeedbackQuestions}";
        }
    }
}
