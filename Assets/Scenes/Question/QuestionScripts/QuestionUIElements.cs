using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestionUIElements : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button nextQuestionButton;
    [SerializeField] private GameObject timePanel;
    [SerializeField] private TextMeshProUGUI timerText;

    // Propriedades públicas
    public TextMeshProUGUI QuestionText => questionText;
    public TextMeshProUGUI ScoreText => scoreText;
    public TextMeshProUGUI NameText => nameText;
    public Button ExitButton => exitButton;
    public Button NextQuestionButton => nextQuestionButton;
    public GameObject TimePanel => timePanel;
    public TextMeshProUGUI TimerText => timerText;

    public void SetupButtonListeners(UnityAction exitAction, UnityAction nextAction)
    {
        exitButton.onClick.RemoveAllListeners();
        nextQuestionButton.onClick.RemoveAllListeners();
        
        exitButton.onClick.AddListener(exitAction);
        nextQuestionButton.onClick.AddListener(nextAction);
    }

    public void ValidateComponents()
    {
        if (questionText == null) Debug.LogError("QuestionText não atribuído");
        if (scoreText == null) Debug.LogError("ScoreText não atribuído");
        if (nameText == null) Debug.LogError("NameText não atribuído");
        if (exitButton == null) Debug.LogError("ExitButton não atribuído");
        if (nextQuestionButton == null) Debug.LogError("NextQuestionButton não atribuído");
        if (timePanel == null) Debug.LogError("TimePanel não atribuído");
        if (timerText == null) Debug.LogError("TimerText não atribuído");
    }
}
