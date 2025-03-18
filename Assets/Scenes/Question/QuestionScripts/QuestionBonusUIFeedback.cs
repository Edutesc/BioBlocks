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
        // Verificar se tem um CanvasGroup e adicionar se não tiver
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            Debug.Log("CanvasGroup adicionado ao QuestionBonusUIFeedback");
        }

        // Verificar e obter referências se necessário
        if (bonusMessageText == null)
        {
            bonusMessageText = transform.Find("FeedbackText")?.GetComponent<TextMeshProUGUI>();
            if (bonusMessageText == null)
            {
                Debug.LogError("QuestionBonusUIFeedback: Não foi possível encontrar o TextMeshProUGUI 'FeedbackText'");
            }
        }

        if (bonusPanel == null)
        {
            bonusPanel = GetComponent<Image>();
            if (bonusPanel == null)
            {
                Debug.LogError("QuestionBonusUIFeedback: Não foi possível encontrar o componente Image");
            }
        }
    }

    private void Start()
    {
        // Esconder no início
        gameObject.SetActive(false);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowBonusActivatedFeedback()
    {
        Debug.Log("ShowBonusActivatedFeedback iniciado");

        gameObject.SetActive(true);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Debug.Log("CanvasGroup definido como visível");
        }

        // Verificar e definir a mensagem
        if (bonusMessageText != null)
        {
            bonusMessageText.text = "Parabéns!!\n5 Repostas Corretas em Sequência\nXP dobrada por 10 minutos";
            bonusMessageText.enabled = true;
            Debug.Log("Texto de bônus definido: " + bonusMessageText.text);
        }
        else
        {
            Debug.LogError("bonusMessageText é null!");
        }

        // Verificar o painel
        if (bonusPanel != null)
        {
            bonusPanel.enabled = true;
            Color panelColor = new Color(1f, 0.6f, 0f, 1f); // Laranja
            bonusPanel.color = panelColor;
            Debug.Log("Painel de bônus configurado com cor laranja");
        }
        else
        {
            Debug.LogError("bonusPanel é null!");
        }
    }

    private void HideFeedback()
    {
        Debug.Log("HideFeedback chamado - escondendo o feedback de bônus");

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Debug.Log("CanvasGroup definido como invisível");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsVisible()
    {
        if (canvasGroup != null)
        {
            return canvasGroup.alpha > 0f && gameObject.activeSelf;
        }

        return gameObject.activeSelf;
    }

    public void ForceVisibility(bool visible)
    {
        if (visible)
        {
            gameObject.SetActive(true);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                Debug.Log("Visibilidade forçada para: visível");
            }
        }
        else
        {
            HideFeedback();
        }
    }

}