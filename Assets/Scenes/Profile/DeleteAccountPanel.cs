using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DeleteAccountPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup não encontrado em {gameObject.name}");
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        Debug.Log($"DeleteAccountPanel Awake - CanvasGroup encontrado: {canvasGroup != null}");
    }

    private void Start()
    {
        // Garantir que comece oculto
        HidePanel();
        Debug.Log($"DeleteAccountPanel Start - Panel hidden. Alpha: {canvasGroup.alpha}");
    }

    public void ShowPanel()
    {
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup é null em {gameObject.name}");
            return;
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Debug.Log($"Painel mostrado - Alpha: {canvasGroup.alpha}");
    }

    public void HidePanel()
    {
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup é null em {gameObject.name}");
            return;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        Debug.Log($"Painel ocultado - Alpha: {canvasGroup.alpha}");
    }

    private void OnEnable()
    {
        // Garantir que fique oculto mesmo quando reativado
        if (canvasGroup != null)
        {
            HidePanel();
        }
    }
}
