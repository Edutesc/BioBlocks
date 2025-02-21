using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UserFeedbackProgressIndicator : MonoBehaviour
{
    [Header("Componentes UI")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressText;

    private UserFeedbackManager feedbackManager;
    private int totalQuestions;
    private int currentQuestionIndex;

    private void Start()
    {
        feedbackManager = FindFirstObjectByType<UserFeedbackManager>();
        if (feedbackManager == null)
        {
            Debug.LogError("FeedbackProgressIndicator: Não foi possível encontrar o UserFeedbackManager");
            return;
        }
    }

    public void InitializeProgressIndicator(List<FeedbackQuestion> questions)
    {
        totalQuestions = questions.Count;

        // Configura o slider
        if (progressSlider != null)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = totalQuestions - 1; // -1 porque o índice começa em 0
            progressSlider.value = 0;
        }

        // Inicializa com a primeira pergunta
        UpdateProgress(0, questions[0]);
    }

    public void UpdateProgress(int index, FeedbackQuestion currentQuestion)
    {
        currentQuestionIndex = index;

        // Atualiza o slider
        if (progressSlider != null)
        {
            progressSlider.value = index;
        }

        // Atualiza o texto de progresso
        if (progressText != null)
        {
            progressText.text = $"Pergunta {index + 1} de {totalQuestions}";
        }
    }
}