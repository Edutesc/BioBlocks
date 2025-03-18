using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    [SerializeField] private List<string> scenesWithoutBottomBar = new List<string>() { "LoginView", "RegisterView, QuestionScene"};

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
        
        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        foreach (var buttonInfo in allButtons)
        {
            if (buttonInfo.button != null)
            {
                buttonInfo.button.onClick.RemoveAllListeners();
                string targetButtonName = buttonInfo.buttonName;
                string targetSceneName = buttonInfo.targetScene;
                
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
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged += OnSceneChanged;
            
            if (debugLogs) Debug.Log("Registrado com o NavigationManager");
        }
        else
        {
            Debug.LogWarning("NavigationManager não encontrado! A BottomBar pode não funcionar corretamente.");
        }

        string activeScene = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(activeScene))
        {
            currentScene = activeScene;
        }

        UpdateButtonDisplay(currentScene);
        AdjustVisibilityForCurrentScene();
    }
    
    private void OnDestroy()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged -= OnSceneChanged;
        }

        if (_instance == this)
        {
            _instance = null;
        }
    }
    
    private void OnSceneChanged(string sceneName)
    {
        if (debugLogs) Debug.Log($"BottomBarManager: Cena mudou para {sceneName}");
        
        currentScene = sceneName;
        UpdateButtonDisplay(currentScene);
        AdjustVisibilityForCurrentScene();
    }

    private void AdjustVisibilityForCurrentScene()
    {
        bool shouldShowBottomBar = !scenesWithoutBottomBar.Contains(currentScene);
        gameObject.SetActive(shouldShowBottomBar);
        
        if (debugLogs) Debug.Log($"BottomBar visibilidade na cena {currentScene}: {shouldShowBottomBar}");
    }

    public void UpdateButtonDisplay(string sceneName)
    {
        if (debugLogs) Debug.Log($"Atualizando BottomBar para cena: {sceneName}");

        foreach (var button in allButtons)
        {
            if (button != null && button.normalIcon != null && button.filledIcon != null)
            {
                bool isActiveButton = (button.targetScene == sceneName);
                button.normalIcon.gameObject.SetActive(!isActiveButton);
                button.filledIcon.gameObject.SetActive(isActiveButton);
                
                if (debugLogs && isActiveButton)
                {
                    Debug.Log($"Ativado botão: {button.buttonName} para cena {sceneName}");
                }
            }
        }
    }

    public void AddSceneWithoutBottomBar(string sceneName)
    {
        if (!scenesWithoutBottomBar.Contains(sceneName))
        {
            scenesWithoutBottomBar.Add(sceneName);
        }
    }

    public void RemoveSceneWithoutBottomBar(string sceneName)
    {
        if (scenesWithoutBottomBar.Contains(sceneName))
        {
            scenesWithoutBottomBar.Remove(sceneName);
        }
    }
}