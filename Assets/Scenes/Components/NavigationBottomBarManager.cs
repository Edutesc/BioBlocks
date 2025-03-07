using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia a NavigationBottomBar, controlando os ícones e interagindo com o NavigationManager.
/// Também mantem a persistência da BottomBar entre cenas.
/// </summary>
public class NavigationBottomBarManager : MonoBehaviour
{
    [System.Serializable]
    public class NavButton
    {
        public string buttonName;
        public string targetScene;
        public Button button;
        public Image normalIcon;
        public Image filledIcon;
    }

    [Header("Botões de Navegação")]
    [SerializeField] private NavButton homeButton;
    [SerializeField] private NavButton rankingButton;
    [SerializeField] private NavButton favoritesButton;
    [SerializeField] private NavButton medalsButton;
    [SerializeField] private NavButton profileButton;

    [Header("Configurações")]
    [SerializeField] private string currentScene = "";
    [SerializeField] private bool debugLogs = true;
    
    [Header("Persistência")]
    [SerializeField] private List<string> scenesWithoutBottomBar = new List<string>() { "Login", "Splash" };

    private List<NavButton> allButtons = new List<NavButton>();
    private static NavigationBottomBarManager _instance;

    public static NavigationBottomBarManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        // Configurar singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        // Não destruir ao carregar novas cenas
        DontDestroyOnLoad(gameObject);
        
        // Inicializar a lista de botões
        InitializeButtons();
        
        if (debugLogs) Debug.Log("NavigationBottomBarManager inicializado");
    }

    private void InitializeButtons()
    {
        // Limpar a lista de botões
        allButtons.Clear();
        
        // Adicionar os botões à lista
        if (homeButton.button != null) allButtons.Add(homeButton);
        if (rankingButton.button != null) allButtons.Add(rankingButton);
        if (favoritesButton.button != null) allButtons.Add(favoritesButton);
        if (medalsButton.button != null) allButtons.Add(medalsButton);
        if (profileButton.button != null) allButtons.Add(profileButton);
        
        // Configurar os listeners dos botões
        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        // Remover os listeners existentes e adicionar novos
        foreach (var buttonInfo in allButtons)
        {
            if (buttonInfo.button != null)
            {
                buttonInfo.button.onClick.RemoveAllListeners();
                
                // Capturar o buttonName em uma variável local para o closure
                string targetButtonName = buttonInfo.buttonName;
                string targetSceneName = buttonInfo.targetScene;
                
                // Adicionar o listener que usa o NavigationManager para navegar
                buttonInfo.button.onClick.AddListener(() => 
                {
                    if (debugLogs) Debug.Log($"Botão {targetButtonName} clicado, navegando para {targetSceneName}");
                    
                    if (NavigationManager.Instance != null)
                    {
                        NavigationManager.Instance.NavigateTo(targetSceneName);
                    }
                    else
                    {
                        Debug.LogError("NavigationManager não encontrado! Certifique-se de que está presente na cena.");
                    }
                });
            }
        }
    }

    private void Start()
    {
        // Obter referência ao NavigationManager e registrar para eventos
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged += OnSceneChanged;
            
            if (debugLogs) Debug.Log("Registrado com o NavigationManager");
        }
        else
        {
            Debug.LogWarning("NavigationManager não encontrado! A BottomBar pode não funcionar corretamente.");
        }

        // Determinar a cena atual com base na cena carregada atual
        string activeScene = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(activeScene))
        {
            currentScene = activeScene;
        }

        // Atualizar a exibição inicial dos botões
        UpdateButtonDisplay(currentScene);
        
        // Verificar se esta cena deve mostrar a BottomBar
        AdjustVisibilityForCurrentScene();
    }
    
    private void OnDestroy()
    {
        // Desregistrar do evento de mudança de cena
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged -= OnSceneChanged;
        }

        // Limpar singleton
        if (_instance == this)
        {
            _instance = null;
        }
    }
    
    /// <summary>
    /// Callback para quando a cena muda
    /// </summary>
    private void OnSceneChanged(string sceneName)
    {
        if (debugLogs) Debug.Log($"BottomBarManager: Cena mudou para {sceneName}");
        
        // Atualizar a cena atual
        currentScene = sceneName;
        
        // Atualizar os ícones da bottombar
        UpdateButtonDisplay(currentScene);
        
        // Ajustar visibilidade da BottomBar para a cena atual
        AdjustVisibilityForCurrentScene();
    }
    
    /// <summary>
    /// Ajusta a visibilidade da BottomBar com base na cena atual
    /// </summary>
    private void AdjustVisibilityForCurrentScene()
    {
        // Verificar se a cena atual está na lista de cenas sem BottomBar
        bool shouldShowBottomBar = !scenesWithoutBottomBar.Contains(currentScene);
        
        // Mostrar ou ocultar a BottomBar
        gameObject.SetActive(shouldShowBottomBar);
        
        if (debugLogs) Debug.Log($"BottomBar visibilidade na cena {currentScene}: {shouldShowBottomBar}");
    }

    /// <summary>
    /// Atualiza a exibição dos botões com base na cena atual.
    /// </summary>
    /// <param name="sceneName">Nome da cena atual</param>
    public void UpdateButtonDisplay(string sceneName)
    {
        if (debugLogs) Debug.Log($"Atualizando BottomBar para cena: {sceneName}");

        foreach (var button in allButtons)
        {
            if (button != null && button.normalIcon != null && button.filledIcon != null)
            {
                // Um botão está ativo se a cena atual corresponder à sua cena alvo
                bool isActiveButton = (button.targetScene == sceneName);
                
                // Mostrar apenas o ícone apropriado
                button.normalIcon.gameObject.SetActive(!isActiveButton);
                button.filledIcon.gameObject.SetActive(isActiveButton);
                
                if (debugLogs && isActiveButton)
                {
                    Debug.Log($"Ativado botão: {button.buttonName} para cena {sceneName}");
                }
            }
        }
    }

    /// <summary>
    /// Adicionar uma cena à lista de cenas que não devem mostrar a BottomBar
    /// </summary>
    public void AddSceneWithoutBottomBar(string sceneName)
    {
        if (!scenesWithoutBottomBar.Contains(sceneName))
        {
            scenesWithoutBottomBar.Add(sceneName);
        }
    }
    
    /// <summary>
    /// Remover uma cena da lista de cenas que não devem mostrar a BottomBar
    /// </summary>
    public void RemoveSceneWithoutBottomBar(string sceneName)
    {
        if (scenesWithoutBottomBar.Contains(sceneName))
        {
            scenesWithoutBottomBar.Remove(sceneName);
        }
    }
}