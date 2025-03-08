using UnityEngine;

public class DeleteAccountPanelOverlayManager : MonoBehaviour
{
    [SerializeField] private GameObject darkOverlay; // Opcional, pode encontrar em runtime
    
    // Chamado quando o objeto é desativado (quando o painel é fechado)
    private void OnDisable()
    {
        Debug.Log("DeleteAccountPanel sendo desativado - desativando overlay");
        
        // Se temos referência direta, usar
        if (darkOverlay != null)
        {
            darkOverlay.SetActive(false);
            Debug.Log("DarkOverlay desativado via referência direta");
            return;
        }
        
        // Caso não tenha referência, procurar pelo nome
        GameObject overlay = GameObject.Find("DarkOverlay");
        if (overlay != null)
        {
            overlay.SetActive(false);
            Debug.Log("DarkOverlay desativado via GameObject.Find");
        }
    }
}

