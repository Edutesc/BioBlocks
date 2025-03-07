using UnityEngine;
using UnityEngine.UI;

// Adicione este script a um GameObject na cena para ajudar a debugar
// os problemas de interação com a half view
public class HalfViewDebugger : MonoBehaviour
{
    [SerializeField] private HalfViewMenu halfViewMenu;
    [SerializeField] private Button debugButton;
    
    private void Start()
    {
        if (debugButton != null)
        {
            debugButton.onClick.AddListener(DebugHalfView);
        }
        
        // Localiza o halfViewMenu se não estiver configurado
        if (halfViewMenu == null)
        {
            halfViewMenu = FindObjectOfType<HalfViewMenu>();
        }
    }
    
    public void DebugHalfView()
    {
        if (halfViewMenu == null)
        {
            Debug.LogError("HalfViewMenu não encontrado!");
            return;
        }
        
        // Verifica status dos botões e outras propriedades
        halfViewMenu.LogButtonsStatus();
        
        // Verifica configurações de raycasts nos objetos
        CheckRaycastSettings();
    }
    
    private void CheckRaycastSettings()
    {
        Debug.Log("Verificando configurações de raycast...");
        
        Transform halfViewTransform = halfViewMenu.transform;
        Canvas halfViewCanvas = halfViewTransform.GetComponent<Canvas>();
        
        if (halfViewCanvas != null)
        {
            Debug.Log($"HalfViewCanvas - Override Sorting: {halfViewCanvas.overrideSorting}, Sort Order: {halfViewCanvas.sortingOrder}");
        }
        
        // Encontra o painel principal da half view
        RectTransform halfViewPanel = halfViewTransform.GetComponentInChildren<RectTransform>();
        if (halfViewPanel != null)
        {
            CanvasGroup panelCanvasGroup = halfViewPanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup != null)
            {
                Debug.Log($"Half View Panel Canvas Group - Alpha: {panelCanvasGroup.alpha}, Interactable: {panelCanvasGroup.interactable}, Blocks Raycasts: {panelCanvasGroup.blocksRaycasts}");
            }
            
            // Verifica configurações de raycast em todos os elementos da half view
            CheckComponentRaycastSettings(halfViewPanel);
        }
        
        // Verifica DarkOverlay
        Transform darkOverlay = FindDarkOverlay();
        if (darkOverlay != null)
        {
            Canvas overlayCanvas = darkOverlay.GetComponent<Canvas>();
            if (overlayCanvas != null)
            {
                Debug.Log($"DarkOverlay Canvas - Override Sorting: {overlayCanvas.overrideSorting}, Sort Order: {overlayCanvas.sortingOrder}");
            }
            
            Image overlayImage = darkOverlay.GetComponent<Image>();
            if (overlayImage != null)
            {
                Debug.Log($"DarkOverlay Image - Raycast Target: {overlayImage.raycastTarget}, Color Alpha: {overlayImage.color.a}");
            }
        }
        
        // Verifica interactability dos botões principais
        VerificarBotoes();
    }
    
    private void CheckComponentRaycastSettings(Transform parent)
    {
        if (parent == null) return;
        
        // Verifica Image components e se são Raycast Targets
        Image[] images = parent.GetComponentsInChildren<Image>(true);
        foreach (Image img in images)
        {
            Debug.Log($"Image '{img.gameObject.name}' - Raycast Target: {img.raycastTarget}");
        }
        
        // Verifica botões
        Button[] buttons = parent.GetComponentsInChildren<Button>(true);
        foreach (Button btn in buttons)
        {
            Debug.Log($"Button '{btn.gameObject.name}' - Interactable: {btn.interactable}");
            
            // Verifica o Transition type
            string transitionType = "Desconhecido";
            switch (btn.transition)
            {
                case Selectable.Transition.None:
                    transitionType = "None";
                    break;
                case Selectable.Transition.ColorTint:
                    transitionType = "Color Tint";
                    break;
                case Selectable.Transition.SpriteSwap:
                    transitionType = "Sprite Swap";
                    break;
                case Selectable.Transition.Animation:
                    transitionType = "Animation";
                    break;
            }
            Debug.Log($"Button '{btn.gameObject.name}' - Transition Type: {transitionType}");
        }
    }
    
    private Transform FindDarkOverlay()
    {
        // Procura pelo DarkOverlay na cena
        GameObject darkOverlay = GameObject.Find("DarkOverlay");
        if (darkOverlay != null)
        {
            return darkOverlay.transform;
        }
        
        // Procura na hierarquia do HalfViewMenu
        if (halfViewMenu != null)
        {
            Transform overlay = halfViewMenu.transform.Find("DarkOverlay");
            if (overlay != null)
            {
                return overlay;
            }
        }
        
        return null;
    }
    
    private void VerificarBotoes()
    {
        // Verifica botões específicos da half view
        Button logoutButton = GameObject.Find("LogoutButton")?.GetComponent<Button>();
        Button deleteAccountButton = GameObject.Find("DeleteAccountButton")?.GetComponent<Button>();
        Button closeButton = GameObject.Find("CloseButton")?.GetComponent<Button>();
        
        Debug.Log("Status dos botões (encontrados por nome na cena):");
        CheckButtonStatus("LogoutButton", logoutButton);
        CheckButtonStatus("DeleteAccountButton", deleteAccountButton);
        CheckButtonStatus("CloseButton", closeButton);
    }
    
    private void CheckButtonStatus(string name, Button button)
    {
        if (button != null)
        {
            Debug.Log($"{name} - Interactable: {button.interactable}, IsActive: {button.gameObject.activeInHierarchy}");
            
            // Verifica componentes associados
            Graphic targetGraphic = button.targetGraphic;
            if (targetGraphic != null)
            {
                Debug.Log($"{name} Target Graphic - Type: {targetGraphic.GetType().Name}, Raycast Target: {targetGraphic.raycastTarget}");
            }
            else
            {
                Debug.Log($"{name} - Sem Target Graphic!");
            }
        }
        else
        {
            Debug.Log($"{name} - Não encontrado na cena");
        }
    }
}
