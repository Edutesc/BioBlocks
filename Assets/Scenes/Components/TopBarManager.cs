using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TopBarManager : BarsManager
{
    [System.Serializable]
    public class TopButton
    {
        public string buttonName;
        public Button button;
        public Image buttonImage;
        public List<string> visibleInScenes = new List<string>();
    }

    [Header("Textos da TopBar")]
    [SerializeField] private TMP_Text weekScore;
    [SerializeField] private TMP_Text nickname;

    [Header("Botões da TopBar")]
    [SerializeField] private TopButton hubButton;
    [SerializeField] private TopButton engineButton;

    [Header("Persistência")]
    [SerializeField]
    private List<string> scenesWithoutTopBar = new List<string>()
    {
        "LoginView", "RegisterView", "ResetDatabaseView"
    };

    private Dictionary<string, TopButton> buttonsByName = new Dictionary<string, TopButton>();
    private List<TopButton> allButtons = new List<TopButton>();
    private static TopBarManager _instance;
    private HalfViewComponent halfViewComponent;
    private float lastVerificationTime = 0f;
    protected override string BarName => "PersistentTopBar";
    protected override string BarChildName => "TopBar";

    public static TopBarManager Instance => _instance;

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

    protected override void ConfigureSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    protected override void OnAwake()
    {
        base.scenesWithoutBar = new List<string>(scenesWithoutTopBar);

        foreach (var scene in scenesWithoutBar)
        {
            if (!base.scenesWithoutBar.Contains(scene))
            {
                base.scenesWithoutBar.Add(scene);
            }
        }

        InitializeButtons();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnStart()
    {
        UserDataStore.OnUserDataChanged += OnUserDataChanged;
        UpdateFromCurrentUserData();
    }

    protected override void OnCleanup()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnNavigationComplete -= OnNavigationComplete;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;

        if (_instance == this)
        {
            _instance = null;
        }
    }

    protected override void RegisterWithNavigationManager()
    {
        base.RegisterWithNavigationManager();

        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnNavigationComplete += OnNavigationComplete;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (debugLogs) Debug.Log($"TopBarManager: Cena carregada diretamente: {scene.name}");

        isSceneBeingLoaded = true;
        currentScene = scene.name;
        Invoke(nameof(HandleSceneLoadComplete), 0.1f);
    }

    private void HandleSceneLoadComplete()
    {
        UpdateBarState(currentScene);
        isSceneBeingLoaded = false;

        if (debugLogs) Debug.Log($"TopBarManager: Configuração completa para cena {currentScene}, visibilidade: {gameObject.activeSelf}");
    }

    protected override void OnSceneChangedSpecific(string sceneName)
    {
        if (sceneName == "QuestionScene")
        {
            Debug.Log("TopBarManager: Detectada QuestionScene, aplicando tratamento especial");
            EnsureTopBarVisibilityInScene(sceneName);
        }

        HandleHalfViewComponent(sceneName);
    }

    private void OnNavigationComplete(string sceneName)
    {
        if (Time.time - lastVerificationTime < 0.5f) return;
        lastVerificationTime = Time.time;

        if (debugLogs) Debug.Log($"TopBarManager: OnNavigationComplete para cena {sceneName}");
        UpdateBarState(sceneName);
    }

    private void HandleHalfViewComponent(string sceneName)
    {
        if (sceneName != "ProfileScene")
        {
            halfViewComponent = null;
        }
        else if (halfViewComponent == null)
        {
            StartCoroutine(FindHalfViewMenuAfterDelay());
        }
    }

    private IEnumerator FindHalfViewMenuAfterDelay()
    {
        yield return null;

        halfViewComponent = FindFirstObjectByType<HalfViewComponent>(FindObjectsInactive.Include);

        if (halfViewComponent != null)
        {
            if (debugLogs) Debug.Log("HalfViewMenu encontrado na ProfileScene");
        }
        else
        {
            Debug.LogWarning("HalfViewMenu não encontrado na ProfileScene!");
        }
    }

    private void InitializeButtons()
    {
        allButtons.Clear();
        buttonsByName.Clear();
        InitializeButton(hubButton);
        InitializeButton(engineButton);
        SetupButtonListeners();
    }

    private void InitializeButton(TopButton button)
    {
        if (button.button != null)
        {
            if (button.buttonImage == null)
                button.buttonImage = button.button.GetComponent<Image>();

            allButtons.Add(button);
            buttonsByName[button.buttonName] = button;
        }
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
                    NavigationManager.Instance.NavigateTo("ProfileScene");
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

                if (currentScene == "ProfileScene")
                {
                    HalfViewRegistry.ShowHalfViewForCurrentScene();
                }
            });
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnNavigationComplete += OnNavigationComplete;
        }
    }

    private void OnDisable()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnNavigationComplete -= OnNavigationComplete;
        }
    }

    private void OnUserDataChanged(UserData userData)
    {
        if (userData != null)
        {
            UpdateUserInfoDisplay(userData);
            if (debugLogs) Debug.Log($"TopBarManager: Dados do usuário atualizados - WeekScore: {userData.WeekScore}, Nickname: {userData.NickName}");
        }
    }

    private void UpdateUserInfoDisplay(UserData userData)
    {
        if (weekScore != null)
        {
            weekScore.text = userData.WeekScore.ToString();
        }

        if (nickname != null)
        {
            nickname.text = userData.NickName;
        }
    }

    private void UpdateFromCurrentUserData()
    {
        UserData currentUserData = UserDataStore.CurrentUserData;
        if (currentUserData != null)
        {
            UpdateUserInfoDisplay(currentUserData);

            if (debugLogs) Debug.Log($"TopBarManager: Carregados dados iniciais - WeekScore: {currentUserData.WeekScore}, Nickname: {currentUserData.NickName}");
        }
        else if (debugLogs)
        {
            Debug.LogWarning("TopBarManager: CurrentUserData é null, não foi possível carregar dados iniciais");
        }
    }

    protected override void UpdateButtonVisibility(string sceneName)
    {
        if (debugLogs) Debug.Log($"Atualizando visibilidade dos botões para cena: {sceneName}");

        foreach (var button in allButtons)
        {
            if (button != null && button.button != null && button.buttonImage != null)
            {
                bool isActive = button.visibleInScenes.Contains(sceneName);
                button.button.interactable = isActive;
                Color buttonColor = button.buttonImage.color;
                button.buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, isActive ? 1 : 0);

                if (debugLogs)
                {
                    Debug.Log($"Botão: {button.buttonName} - Visível e interativo: {isActive} na cena {sceneName}");
                }
            }
        }
    }

    public void EnsureTopBarVisibilityInScene(string sceneName)
    {
        Debug.Log($"TopBarManager: Forçando visibilidade na cena {sceneName}");
        RemoveSceneWithoutBar(sceneName);

        if (currentScene == sceneName)
        {
            gameObject.SetActive(true);
            EnsureBarIntegrity();
            UpdateButtonVisibility(currentScene);

            Debug.Log($"TopBarManager: Visibilidade forçada com sucesso na cena {sceneName}");
        }
    }

    public void SetTopBarTexts(string title, string subtitle = "")
    {
        Title = title;
        Subtitle = subtitle;

        if (debugLogs) Debug.Log($"TopBar textos definidos manualmente: Score = {title}, Nickname = {subtitle}");
    }

    public void ManageButtonSceneVisibility(string buttonName, string sceneName, bool addVisibility)
    {
        if (!buttonsByName.TryGetValue(buttonName, out TopButton targetButton))
        {
            Debug.LogError($"Botão com nome '{buttonName}' não encontrado!");
            Debug.Log($"Botões disponíveis: {string.Join(", ", buttonsByName.Keys)}");
            return;
        }

        bool listChanged = false;

        if (addVisibility)
        {
            if (!targetButton.visibleInScenes.Contains(sceneName))
            {
                targetButton.visibleInScenes.Add(sceneName);
                listChanged = true;
            }
        }
        else
        {
            if (targetButton.visibleInScenes.Contains(sceneName))
            {
                targetButton.visibleInScenes.Remove(sceneName);
                listChanged = true;
            }
        }

        if (listChanged && currentScene == sceneName)
        {
            UpdateButtonVisibility(currentScene);
        }
    }

    public void AddSceneToButtonVisibility(string buttonName, string sceneName)
    {
        ManageButtonSceneVisibility(buttonName, sceneName, true);
    }

    public void RemoveSceneFromButtonVisibility(string buttonName, string sceneName)
    {
        ManageButtonSceneVisibility(buttonName, sceneName, false);
    }

    public void AddSceneWithoutTopBar(string sceneName)
    {
        AddSceneWithoutBar(sceneName);
    }

    public void RemoveSceneWithoutTopBar(string sceneName)
    {
        RemoveSceneWithoutBar(sceneName);
    }

    public void DebugListButtons()
    {
        Debug.Log("=== TopBarManager - Estado Atual ===");
        Debug.Log($"Cena atual: {currentScene}");
        Debug.Log($"Visibilidade atual: {gameObject.activeSelf}");
        Debug.Log($"Cenas sem TopBar: {string.Join(", ", scenesWithoutBar)}");

        Canvas canvas = GetComponent<Canvas>();
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        Debug.Log($"Canvas habilitado: {(canvas != null ? canvas.enabled : false)}");
        Debug.Log($"CanvasGroup alpha: {(canvasGroup != null ? canvasGroup.alpha : 0)}");
        Debug.Log($"CanvasGroup interactable: {(canvasGroup != null ? canvasGroup.interactable : false)}");

        foreach (var button in allButtons)
        {
            Debug.Log($"Botão: {button.buttonName}");
            Debug.Log($"Cenas visíveis: {string.Join(", ", button.visibleInScenes)}");
        }

        Debug.Log("=====================================");
    }
}