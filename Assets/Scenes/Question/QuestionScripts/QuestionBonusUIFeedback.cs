using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionBonusUIFeedback : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI bonusMessageText;
    [SerializeField] private Image bonusPanel;
    [SerializeField] private float displayDuration = 5f;

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

        Debug.Log($"Feedback será escondido em {displayDuration} segundos");
    }

    private void HideFeedback()
    {
        Debug.Log("HideFeedback chamado - escondendo o feedback de bônus");
        
        // Não desative o gameObject, apenas torne-o invisível pelo CanvasGroup
        // para que o QuestionCanvasGroupManager possa continuar gerenciando-o
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Debug.Log("CanvasGroup definido como invisível");
        }
        else
        {
            // Se não tiver CanvasGroup, desativa o GameObject
            gameObject.SetActive(false);
        }
    }

    // Método público para verificar o estado de visibilidade
    public bool IsVisible()
    {
        if (canvasGroup != null)
        {
            return canvasGroup.alpha > 0f && gameObject.activeSelf;
        }
        
        return gameObject.activeSelf;
    }
    
    // Método para forçar a visibilidade através do código
    public void ForceVisibility(bool visible)
    {
        gameObject.SetActive(true);
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
            Debug.Log($"Visibilidade forçada para: {(visible ? "visível" : "invisível")}");
        }
        else
        {
            // Se não tiver CanvasGroup, define a ativação do GameObject
            gameObject.SetActive(visible);
        }
    }
}