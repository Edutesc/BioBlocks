using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestionBottomUIManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button nextQuestionButton;
    [SerializeField] private GameObject timePanel;
    [SerializeField] private TextMeshProUGUI timerText;

    public event UnityAction OnExitButtonClicked;
    public event UnityAction OnNextButtonClicked;
    public TextMeshProUGUI TimerText => timerText;

    private void Start()
    {
        ValidateComponents();
        InitializeUI();
    }

    private void ValidateComponents()
    {
        if (exitButton == null) Debug.LogError("ExitButton não atribuído");
        if (nextQuestionButton == null) Debug.LogError("NextQuestionButton não atribuído");
        if (timePanel == null) Debug.LogError("TimePanel não atribuído");
        if (timerText == null) Debug.LogError("TimerText não atribuído");
    }

    private void InitializeUI()
    {
        exitButton.gameObject.SetActive(true);
        nextQuestionButton.gameObject.SetActive(true);
        exitButton.interactable = false;
        nextQuestionButton.interactable = false;
        timePanel.SetActive(true);
    }

    public void SetupNavigationButtons(UnityAction exitAction, UnityAction nextAction)
    {
        exitButton.onClick.RemoveAllListeners();
        nextQuestionButton.onClick.RemoveAllListeners();

        exitButton.onClick.AddListener(() =>
        {
            OnExitButtonClicked?.Invoke();
            var bottomBar = FindFirstObjectByType<NavigationBottomBarManager>();

            if (bottomBar != null)
            {
                bottomBar.ForceRefreshState();
            }

            exitAction?.Invoke();
        });

        nextQuestionButton.onClick.AddListener(() =>
        {
            OnNextButtonClicked?.Invoke();
            nextAction?.Invoke();
        });
    }

    public void EnableNavigationButtons()
    {
        exitButton.interactable = true;
        nextQuestionButton.interactable = true;
    }

    public void DisableNavigationButtons()
    {
        exitButton.interactable = false;
        nextQuestionButton.interactable = false;
    }
}