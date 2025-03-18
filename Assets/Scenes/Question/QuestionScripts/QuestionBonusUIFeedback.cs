using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionBonusUIFeedback : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI bonusMessageText;
    [SerializeField] private Image bonusPanel;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            Debug.Log("CanvasGroup adicionado ao QuestionBonusUIFeedback");
        }

        if (bonusMessageText == null)
        {
            bonusMessageText = transform.Find("FeedbackText")?.GetComponent<TextMeshProUGUI>();
            if (bonusMessageText == null)
                Debug.LogError("QuestionBonusUIFeedback: Não foi possível encontrar o TextMeshProUGUI 'FeedbackText'");
        }

        if (bonusPanel == null)
        {
            bonusPanel = GetComponent<Image>();
            if (bonusPanel == null)
                Debug.LogError("QuestionBonusUIFeedback: Não foi possível encontrar o componente Image");
        }
    }

    private void Start()
    {
        HideFeedback();
    }

    public void ShowBonusActivatedFeedback()
    {
        Debug.Log("Mostrando feedback de bônus");
        gameObject.SetActive(true);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void HideFeedback()
    {
        Debug.Log("Escondendo feedback de bônus");

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsVisible()
    {
        if (canvasGroup != null)
            return canvasGroup.alpha > 0f && gameObject.activeSelf;
        
        return gameObject.activeSelf;
    }

    public void ForceVisibility(bool visible)
    {
        if (visible)
            ShowBonusActivatedFeedback();
        else
            HideFeedback();
    }
}