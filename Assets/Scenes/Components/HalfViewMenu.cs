using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HalfViewMenu : MonoBehaviour
{
    [Header("Referências do Painel")]
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private Canvas menuCanvas;
    
    [Header("Botões da Interface")]
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button deleteAccountButton;
    [SerializeField] private Button closeButton;
    
    [Header("Botão de Ativação")]
    [SerializeField] private Button engineButton;
    
    [Header("Configurações de Animação")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Configurações de Overlay")]
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private float overlayAlpha = 0.6f;
    
    // Referência ao ProfileManager
    private ProfileManager profileManager;
    
    // Variáveis privadas
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isVisible = false;
    private Coroutine animationCoroutine;
    
    // Lista de elementos interativos a serem desabilitados
    private List<Selectable> interactableElements = new List<Selectable>();
    private List<bool> originalInteractableStates = new List<bool>();
    
    // Variáveis para auto-detecção da bottomBar
    private RectTransform bottomBar = null;
    private Canvas bottomBarCanvas = null;
    private int originalBottomBarSortingOrder = 0;

    void Awake()
    {
        // Localiza automaticamente a bottomBar (se existir)
        FindBottomBar();
        
        // Localiza o ProfileManager
        profileManager = FindFirstObjectByType<ProfileManager>();
        if (profileManager == null)
        {
            Debug.LogWarning("ProfileManager não encontrado na cena!");
        }
        else
        {
            Debug.Log("ProfileManager encontrado com sucesso");
        }
        
        // Inicializa o menuCanvas, se não estiver configurado
        if (menuCanvas == null)
        {
            menuCanvas = GetComponent<Canvas>();
            if (menuCanvas == null)
            {
                menuCanvas = gameObject.AddComponent<Canvas>();
                menuCanvas.overrideSorting = true;
            }
        }
        
        // Garante que o Canvas tem override sorting habilitado
        menuCanvas.overrideSorting = true;
    }

    void Start()
    {
        // Configura as posições
        SetupPositions();
        
        // Inicia na posição escondida
        if (menuPanel != null)
        {
            menuPanel.anchoredPosition = hiddenPosition;
        }
        
        // Configura listeners
        SetupButtonListeners();
        
        // Esconde o menu inicialmente
        gameObject.SetActive(false);
        
        // Configura e esconde o overlay, se existir
        SetupOverlay();
        
        // Coleta todos os elementos interativos na cena principal
        CollectInteractableElements();
        
        // Log para debug
        Debug.Log("HalfViewMenu iniciado. Botões configurados: " + 
                 (logoutButton != null ? "Logout OK, " : "Logout NULL, ") +
                 (deleteAccountButton != null ? "Delete OK, " : "Delete NULL, ") +
                 (closeButton != null ? "Close OK" : "Close NULL"));
    }

    private void FindBottomBar()
    {
        // Procura por objetos que possam ser a bottomBar
        Canvas[] allCanvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.gameObject.name.ToLower().Contains("bottom") || 
                canvas.gameObject.name.ToLower().Contains("bar") ||
                canvas.gameObject.name.ToLower().Contains("tabbar") ||
                canvas.gameObject.name.ToLower().Contains("navbar"))
            {
                bottomBarCanvas = canvas;
                bottomBar = canvas.GetComponent<RectTransform>();
                originalBottomBarSortingOrder = canvas.sortingOrder;
                Debug.Log("BottomBar detectada automaticamente: " + canvas.gameObject.name);
                break;
            }
        }
        
        // Se não conseguiu encontrar pelo nome, tenta encontrar pelo posicionamento e tamanho
        if (bottomBar == null)
        {
            RectTransform[] allRectTransforms = FindObjectsOfType<RectTransform>(true);
            foreach (RectTransform rect in allRectTransforms)
            {
                // Procura por um objeto que esteja posicionado na parte inferior da tela
                // e que tenha largura aproximada da tela
                if (rect.anchorMin.y <= 0.1f && rect.anchorMax.y <= 0.2f && 
                    rect.anchorMin.x <= 0.1f && rect.anchorMax.x >= 0.9f)
                {
                    Canvas canvas = rect.GetComponentInParent<Canvas>();
                    if (canvas != null && canvas != menuCanvas)
                    {
                        bottomBarCanvas = canvas;
                        bottomBar = rect;
                        originalBottomBarSortingOrder = canvas.sortingOrder;
                        Debug.Log("BottomBar detectada automaticamente pela posição: " + rect.gameObject.name);
                        break;
                    }
                }
            }
        }
    }

    private void SetupPositions()
    {
        if (menuPanel == null)
        {
            Debug.LogError("Menu Panel não atribuído!");
            return;
        }
        
        // A posição escondida é abaixo da tela
        hiddenPosition = new Vector2(0, -menuPanel.rect.height);
        
        // A posição visível é na parte inferior da tela
        visiblePosition = new Vector2(0, 0);
    }

    private void SetupButtonListeners()
    {
        // Configura o botão que ativa o menu
        if (engineButton != null)
        {
            engineButton.onClick.AddListener(ToggleMenu);
        }
        else
        {
            Debug.LogWarning("EngineButton não atribuído! O menu precisará ser ativado manualmente.");
        }
        
        // Configura os botões internos da half view
        if (logoutButton != null)
        {
            Debug.Log("Configurando listener para o botão de logout");
            logoutButton.onClick.AddListener(OnLogoutClicked);
            
            // Garante que o botão esteja interagível
            logoutButton.interactable = true;
        }
        else
        {
            Debug.LogWarning("LogoutButton não atribuído!");
        }
            
        if (deleteAccountButton != null)
        {
            Debug.Log("Configurando listener para o botão de deletar conta");
            deleteAccountButton.onClick.AddListener(OnDeleteAccountClicked);
            
            // Garante que o botão esteja interagível
            deleteAccountButton.interactable = true;
        }
        else
        {
            Debug.LogWarning("DeleteAccountButton não atribuído!");
        }
            
        if (closeButton != null)
        {
            Debug.Log("Configurando listener para o botão de fechar");
            closeButton.onClick.AddListener(HideMenu);
            
            // Garante que o botão esteja interagível
            closeButton.interactable = true;
        }
        else
        {
            Debug.LogWarning("CloseButton não atribuído! O usuário precisará usar o engineButton para fechar.");
        }
    }

    private void SetupOverlay()
    {
        // Configura o overlay se existir
        if (darkOverlay != null)
        {
            Debug.Log("Configurando DarkOverlay existente");
            
            // Esconde inicialmente
            darkOverlay.SetActive(false);
            
            // Configura o overlay para cobrir toda a tela
            RectTransform overlayRect = darkOverlay.GetComponent<RectTransform>();
            if (overlayRect != null)
            {
                overlayRect.anchorMin = Vector2.zero;
                overlayRect.anchorMax = Vector2.one;
                overlayRect.offsetMin = Vector2.zero;
                overlayRect.offsetMax = Vector2.zero;
            }
            
            // Configura a cor do overlay
            Image overlayImage = darkOverlay.GetComponent<Image>();
            if (overlayImage != null)
            {
                Color color = overlayImage.color;
                color.a = overlayAlpha;
                overlayImage.color = color;
            }
            
            // Adiciona um botão para fechar quando clicado no overlay
            Button overlayButton = darkOverlay.GetComponent<Button>();
            if (overlayButton == null)
            {
                overlayButton = darkOverlay.AddComponent<Button>();
                ColorBlock colors = overlayButton.colors;
                colors.normalColor = new Color(0, 0, 0, 0);
                colors.highlightedColor = new Color(0, 0, 0, 0);
                colors.pressedColor = new Color(0, 0, 0, 0);
                colors.selectedColor = new Color(0, 0, 0, 0);
                overlayButton.colors = colors;
            }
            overlayButton.onClick.AddListener(HideMenu);
            
            // CORREÇÃO: Verifica se o DarkOverlay está na hierarquia correta
            // Se necessário, move para ficar antes da half view na hierarquia
            if (darkOverlay.transform.GetSiblingIndex() > menuPanel.transform.GetSiblingIndex())
            {
                Debug.Log("Corrigindo posição do DarkOverlay na hierarquia para ficar abaixo da half view");
                darkOverlay.transform.SetSiblingIndex(menuPanel.transform.GetSiblingIndex() - 1);
            }
            
            // Configura o Canvas do overlay, se necessário
            Canvas overlayCanvas = darkOverlay.GetComponent<Canvas>();
            if (overlayCanvas == null)
            {
                overlayCanvas = darkOverlay.AddComponent<Canvas>();
            }
            overlayCanvas.overrideSorting = true;
            overlayCanvas.sortingOrder = 99; // CORREÇÃO: Um pouco menos que o menu
        }
        else
        {
            Debug.Log("Criando DarkOverlay dinamicamente");
            // Cria um overlay dinamicamente se não existir
            GameObject newOverlay = new GameObject("DarkOverlay");
            newOverlay.transform.SetParent(transform);
            
            // Importante: posiciona o overlay antes do menuPanel na hierarquia
            newOverlay.transform.SetSiblingIndex(0);
            
            RectTransform overlayRect = newOverlay.AddComponent<RectTransform>();
            overlayRect.anchorMin = Vector2.zero;
            overlayRect.anchorMax = Vector2.one;
            overlayRect.offsetMin = Vector2.zero;
            overlayRect.offsetMax = Vector2.zero;
            
            Canvas overlayCanvas = newOverlay.AddComponent<Canvas>();
            overlayCanvas.overrideSorting = true;
            overlayCanvas.sortingOrder = 99;
            
            Image overlayImage = newOverlay.AddComponent<Image>();
            overlayImage.color = new Color(0, 0, 0, overlayAlpha);
            
            Button overlayButton = newOverlay.AddComponent<Button>();
            ColorBlock colors = overlayButton.colors;
            colors.normalColor = new Color(0, 0, 0, 0);
            colors.highlightedColor = new Color(0, 0, 0, 0);
            colors.pressedColor = new Color(0, 0, 0, 0);
            colors.selectedColor = new Color(0, 0, 0, 0);
            overlayButton.colors = colors;
            overlayButton.onClick.AddListener(HideMenu);
            
            newOverlay.SetActive(false);
            darkOverlay = newOverlay;
        }
    }

    private void CollectInteractableElements()
    {
        // Limpa as listas
        interactableElements.Clear();
        originalInteractableStates.Clear();
        
        // Coleta todos os elementos interativos na cena
        Selectable[] selectables = FindObjectsOfType<Selectable>(true);
        foreach (Selectable selectable in selectables)
        {
            // Não incluímos elementos da própria half view
            if (!IsPartOfHalfView(selectable.gameObject))
            {
                interactableElements.Add(selectable);
                originalInteractableStates.Add(selectable.interactable);
            }
        }
    }

    private bool IsPartOfHalfView(GameObject obj)
    {
        // Verifica se o objeto é parte da half view ou do overlay
        Transform current = obj.transform;
        while (current != null)
        {
            if (current.gameObject == gameObject || (darkOverlay != null && current.gameObject == darkOverlay))
            {
                return true;
            }
            current = current.parent;
        }
        return false;
    }

    public void ToggleMenu()
    {
        if (isVisible)
            HideMenu();
        else
            ShowMenu();
    }

    public void ShowMenu()
    {
        if (isVisible) return;
        Debug.Log("ShowMenu chamado");
        
        // Atualiza a lista de elementos interativos, pois podem ter mudado desde o Start
        CollectInteractableElements();
        
        // Desabilita interação em todos os elementos da cena principal
        DisableSceneInteraction();
        
        // Ativa o objeto para exibição
        gameObject.SetActive(true);
        
        // CORREÇÃO: Garante que os botões da half view estejam interagíveis
        EnsureHalfViewButtonsInteractable();
        
        // Ativa o overlay, se existir
        if (darkOverlay != null)
        {
            darkOverlay.SetActive(true);
        }
        
        // Ajusta o sorting order para garantir que fique na frente de tudo
        AdjustSortingOrderForVisibility(true);
        
        // Para qualquer animação em andamento
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
            
        // Inicia a animação de exibição
        animationCoroutine = StartCoroutine(AnimateMenu(hiddenPosition, visiblePosition));
        isVisible = true;
    }

    public void HideMenu()
    {
        if (!isVisible) return;
        Debug.Log("HideMenu chamado");
        
        // Reabilita interação em todos os elementos da cena principal
        EnableSceneInteraction();
        
        // Para qualquer animação em andamento
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
            
        // Inicia a animação de ocultação
        animationCoroutine = StartCoroutine(AnimateMenu(visiblePosition, hiddenPosition, true));
        isVisible = false;
        
        // Desativa o overlay, se existir
        if (darkOverlay != null)
        {
            darkOverlay.SetActive(false);
        }
        
        // Restaura o sorting order original
        AdjustSortingOrderForVisibility(false);
    }

    // CORREÇÃO: Método para garantir que os botões da half view estejam interagíveis
    private void EnsureHalfViewButtonsInteractable()
    {
        if (logoutButton != null)
        {
            logoutButton.interactable = true;
            Debug.Log("Botão de logout definido como interagível");
        }
        
        if (deleteAccountButton != null)
        {
            deleteAccountButton.interactable = true;
            Debug.Log("Botão de deletar conta definido como interagível");
        }
        
        if (closeButton != null)
        {
            closeButton.interactable = true;
            Debug.Log("Botão de fechar definido como interagível");
        }
    }

    // Desabilita a interação com todos os elementos coletados
    private void DisableSceneInteraction()
    {
        for (int i = 0; i < interactableElements.Count; i++)
        {
            if (interactableElements[i] != null)
            {
                originalInteractableStates[i] = interactableElements[i].interactable;
                interactableElements[i].interactable = false;
            }
        }
    }

    // Reabilita a interação com todos os elementos, restaurando seus estados originais
    private void EnableSceneInteraction()
    {
        for (int i = 0; i < interactableElements.Count; i++)
        {
            if (interactableElements[i] != null)
            {
                interactableElements[i].interactable = originalInteractableStates[i];
            }
        }
    }

    private void AdjustSortingOrderForVisibility(bool show)
    {
        if (menuCanvas != null)
        {
            if (show)
            {
                // Quando mostramos o menu, aumentamos seu sorting order
                menuCanvas.sortingOrder = 100;
                Debug.Log("Sorting order do menu definido como 100");
                
                // CORREÇÃO: Garante que o Canvas da half view esteja com o modo correto
                menuCanvas.overrideSorting = true;
                
                // Ajustamos o sorting order da bottomBar se for necessário
                if (bottomBarCanvas != null)
                {
                    bottomBarCanvas.sortingOrder = 98; // CORREÇÃO: Menos que o overlay
                    Debug.Log("Sorting order da bottomBar definido como 98");
                }
                
                // Ajusta o sorting order do overlay
                if (darkOverlay != null)
                {
                    Canvas overlayCanvas = darkOverlay.GetComponent<Canvas>();
                    if (overlayCanvas != null)
                    {
                        overlayCanvas.sortingOrder = 99;
                        Debug.Log("Sorting order do overlay definido como 99");
                    }
                }
            }
            else
            {
                // Quando escondemos o menu, restauramos o sorting order
                menuCanvas.sortingOrder = 0;
                
                // Restaura o sorting order original da bottomBar
                if (bottomBarCanvas != null)
                {
                    bottomBarCanvas.sortingOrder = originalBottomBarSortingOrder;
                }
            }
        }
    }

    private IEnumerator AnimateMenu(Vector2 startPos, Vector2 endPos, bool hideWhenDone = false)
    {
        if (menuPanel == null) yield break;
        
        float elapsedTime = 0;
        
        // Configura posição inicial
        menuPanel.anchoredPosition = startPos;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / animationDuration);
            float curveValue = animationCurve.Evaluate(normalizedTime);
            
            // Interpola a posição
            menuPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);
            
            yield return null;
        }
        
        // Garante posição final exata
        menuPanel.anchoredPosition = endPos;
        
        // Desativa o objeto se a animação for de ocultação
        if (hideWhenDone)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // CORREÇÃO: Garante novamente que os botões estejam interagíveis após a animação
            EnsureHalfViewButtonsInteractable();
        }
        
        animationCoroutine = null;
    }

    // Funções de callback para os botões - integradas com ProfileManager
    private void OnLogoutClicked()
    {
        Debug.Log("OnLogoutClicked chamado");
        
        if (profileManager != null)
        {
            Debug.Log("Chamando LogoutButton do ProfileManager");
            profileManager.LogoutButton();
        }
        else
        {
            Debug.LogError("Não foi possível realizar logout: ProfileManager não encontrado!");
        }
        
        HideMenu();
    }

    private void OnDeleteAccountClicked()
    {
        Debug.Log("OnDeleteAccountClicked chamado");
        
        if (profileManager != null)
        {
            Debug.Log("Chamando StartDeleteAccount do ProfileManager");
            profileManager.StartDeleteAccount();
        }
        else
        {
            Debug.LogError("Não foi possível iniciar exclusão de conta: ProfileManager não encontrado!");
        }
        
        HideMenu();
    }

    // Métodos públicos para controle externo
    public bool IsMenuVisible()
    {
        return isVisible;
    }
    
    // CORREÇÃO: Método para debug que pode ser chamado de outro script
    public void LogButtonsStatus()
    {
        Debug.Log("Status dos botões - Half View:");
        if (logoutButton != null)
            Debug.Log($"Logout Button - Interagível: {logoutButton.interactable}");
        else
            Debug.Log("Logout Button: NULL");
            
        if (deleteAccountButton != null)
            Debug.Log($"Delete Account Button - Interagível: {deleteAccountButton.interactable}");
        else
            Debug.Log("Delete Account Button: NULL");
            
        if (closeButton != null)
            Debug.Log($"Close Button - Interagível: {closeButton.interactable}");
        else
            Debug.Log("Close Button: NULL");
            
        Debug.Log($"Canvas Sort Order: {(menuCanvas != null ? menuCanvas.sortingOrder : -1)}");
        Debug.Log($"Canvas Override Sorting: {(menuCanvas != null ? menuCanvas.overrideSorting : false)}");
    }
}