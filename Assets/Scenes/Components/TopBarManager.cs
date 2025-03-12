using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NavigationTopBarManager : MonoBehaviour
{
    [System.Serializable]
    public class TopButton
    {
        public string buttonName;
        public Button button;
        public List<string> visibleInScenes = new List<string>(); // Lista de cenas onde este botão deve aparecer
    }

    [Header("Textos da TopBar")]
    [SerializeField] private TMP_Text weekScore;
    [SerializeField] private TMP_Text nickname;

    [Header("Botões da TopBar")]
    [SerializeField] private TopButton hubButton; 
    [SerializeField] private TopButton engineButton;

    [Header("Configurações")]
    [SerializeField] private string currentScene = "";
    [SerializeField] private bool debugLogs = true;
    
    [Header("Persistência")]
    [SerializeField] private List<string> scenesWithoutTopBar = new List<string>() { "Login", "Splash" };

    private List<TopButton> allButtons = new List<TopButton>();
    private static NavigationTopBarManager _instance;

    public static NavigationTopBarManager Instance
    {
        get { return _instance; }
    }

    public string Title 
    {
        get { return weekScore ? weekScore.text : ""; }
        set { if (weekScore) weekScore.text = value; }
    }
    
    public string Subtitle 
    {
        get { return nickname ? nickname.text : ""; }
        set { if (nickname) nickname.text = value; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeButtons();
        
        if (debugLogs) Debug.Log("NavigationTopBarManager inicializado");
    }

    private void InitializeButtons()
    {
        allButtons.Clear();
        if (hubButton.button != null) allButtons.Add(hubButton);
        if (engineButton.button != null) allButtons.Add(engineButton);
        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        if (hubButton.button != null)
        {
            hubButton.button.onClick.RemoveAllListeners();
            hubButton.button.onClick.AddListener(() => 
            {
                if (debugLogs) Debug.Log($"Botão {hubButton.buttonName} clicado");
                if (NavigationManager.Instance != null)
                {
                        // Implementar a lógica do botão, você pode personalizá-la conforme necessário
                }
                else
                {
                    Debug.LogError("NavigationManager não encontrado!");
                }
            });
        }
        
        if (engineButton.button != null)
        {
            engineButton.button.onClick.RemoveAllListeners();
            engineButton.button.onClick.AddListener(() => 
            {
                if (debugLogs) Debug.Log($"Botão {engineButton.buttonName} clicado");
                
                // Implementar a lógica do botão, você pode personalizá-la conforme necessário
            });
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
            Debug.LogWarning("NavigationManager não encontrado! A TopBar pode não funcionar corretamente.");
        }

        string activeScene = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(activeScene))
        {
            currentScene = activeScene;
        }

        UpdateButtonVisibility(currentScene);
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
        if (debugLogs) Debug.Log($"TopBarManager: Cena mudou para {sceneName}");
        currentScene = sceneName;
        UpdateButtonVisibility(currentScene);
        AdjustVisibilityForCurrentScene();
    }

    private void AdjustVisibilityForCurrentScene()
    {
        bool shouldShowTopBar = !scenesWithoutTopBar.Contains(currentScene);
        gameObject.SetActive(shouldShowTopBar);
        
        if (debugLogs) Debug.Log($"TopBar visibilidade na cena {currentScene}: {shouldShowTopBar}");
    }

    public void UpdateButtonVisibility(string sceneName)
    {
        if (debugLogs) Debug.Log($"Atualizando TopBar para cena: {sceneName}");

        foreach (var button in allButtons)
        {
            if (button != null && button.button != null)
            {
                bool isVisible = button.visibleInScenes.Contains(sceneName);
                button.button.gameObject.SetActive(isVisible);
                
                if (debugLogs)
                {
                    Debug.Log($"Botão: {button.buttonName} - Visível: {isVisible} na cena {sceneName}");
                }
            }
        }
    }

    public void SetTopBarTexts(string title, string subtitle = "")
    {
        if (weekScore != null)
        {
            weekScore.text = title;
        }
        
        if (nickname != null)
        {
            nickname.text = subtitle;
        }
        
        if (debugLogs) Debug.Log($"TopBar textos definidos: Título = {title}, Subtítulo = {subtitle}");
    }

    public void AddSceneToButtonVisibility(string buttonName, string sceneName)
    {
        var button = allButtons.Find(b => b.buttonName == buttonName);
        if (button != null && !button.visibleInScenes.Contains(sceneName))
        {
            button.visibleInScenes.Add(sceneName);

            if (currentScene == sceneName)
            {
                UpdateButtonVisibility(currentScene);
            }
        }
    }
    
    public void RemoveSceneFromButtonVisibility(string buttonName, string sceneName)
    {
        var button = allButtons.Find(b => b.buttonName == buttonName);
        if (button != null && button.visibleInScenes.Contains(sceneName))
        {
            button.visibleInScenes.Remove(sceneName);

            if (currentScene == sceneName)
            {
                UpdateButtonVisibility(currentScene);
            }
        }
    }

    public void AddSceneWithoutTopBar(string sceneName)
    {
        if (!scenesWithoutTopBar.Contains(sceneName))
        {
            scenesWithoutTopBar.Add(sceneName);
        }
    }
    
    public void RemoveSceneWithoutTopBar(string sceneName)
    {
        if (scenesWithoutTopBar.Contains(sceneName))
        {
            scenesWithoutTopBar.Remove(sceneName);
        }
    }
}
