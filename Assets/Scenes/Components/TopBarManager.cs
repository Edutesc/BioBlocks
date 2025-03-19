using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TopBarManager : MonoBehaviour
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

    [Header("Configurações")]
    [SerializeField] private string currentScene = "";
    [SerializeField] private bool debugLogs = true;
    [SerializeField] private Color inactiveButtonColor = new Color(1, 1, 1, 0);

    [Header("Persistência")]
    [SerializeField] private List<string> scenesWithoutTopBar = new List<string>() { "Login", "Splash" };

    private List<TopButton> allButtons = new List<TopButton>();
    private static TopBarManager _instance;
    private HalfViewComponent halfViewComponent;
    private bool isSceneBeingLoaded = false;
    private bool isInTransition = false;
    private float lastVerificationTime = 0f;


    public static TopBarManager Instance
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

        // Registrar para eventos de mudança de cena diretamente
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (debugLogs) Debug.Log("TopBarManager inicializado");
    }

    private void Start()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged += OnNavigationManagerSceneChanged;
            if (debugLogs) Debug.Log("TopBarManager: Registrado com o NavigationManager");
        }
        else
        {
            Debug.LogWarning("TopBarManager: NavigationManager não encontrado! Usando apenas eventos SceneManager.");
        }

        string activeScene = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(activeScene))
        {
            currentScene = activeScene;
        }

        UpdateButtonVisibility(currentScene);
        AdjustVisibilityForCurrentScene();

        // Validar estado após configuração inicial
        ValidateTopBarState();

        UserDataStore.OnUserDataChanged += OnUserDataChanged;
        UpdateFromCurrentUserData();
    }

    private void OnNavigationManagerSceneChanged(string sceneName)
    {
        if (debugLogs) Debug.Log($"TopBarManager: NavigationManager notificou mudança para cena {sceneName}");

        if (isSceneBeingLoaded)
        {
            if (debugLogs) Debug.Log("TopBarManager: Ignorando notificação durante carregamento de cena");
            return;
        }

        currentScene = sceneName;
        UpdateButtonVisibility(currentScene);
        AdjustVisibilityForCurrentScene();

        // Validar estado após mudança de cena
        Invoke("ValidateTopBarState", 0.1f);

        if (sceneName != "ProfileScene")
        {
            halfViewComponent = null;
        }
        else
        {
            StartCoroutine(FindHalfViewMenuAfterDelay());
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (debugLogs) Debug.Log($"TopBarManager: Cena carregada diretamente: {scene.name}");
        currentScene = scene.name;

        // Verificar visibilidade depois que a cena for carregada completamente
        isSceneBeingLoaded = true;

        // Usar o Invoke para garantir que isso aconteça depois que tudo esteja pronto
        Invoke("HandleSceneLoadComplete", 0.1f);
    }

    private void HandleSceneLoadComplete()
    {
        UpdateButtonVisibility(currentScene);
        AdjustVisibilityForCurrentScene();
        isSceneBeingLoaded = false;

        // Verifica se a canvas e os objetos filhos estão corretos
        ValidateTopBarState();

        if (debugLogs) Debug.Log($"TopBarManager: Configuração completa para cena {currentScene}, visibilidade: {gameObject.activeSelf}");
    }

    private void ValidateTopBarState()
    {
        bool shouldBeVisible = !scenesWithoutTopBar.Contains(currentScene);

        if (shouldBeVisible)
        {
            // Verificar Canvas
            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null && !canvas.enabled)
            {
                canvas.enabled = true;
                if (debugLogs) Debug.Log("TopBarManager: Corrigido Canvas desativado");
            }

            // Verificar CanvasGroup
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                if (canvasGroup.alpha < 1f)
                {
                    canvasGroup.alpha = 1f;
                    if (debugLogs) Debug.Log("TopBarManager: Corrigido alpha do CanvasGroup");
                }

                if (!canvasGroup.interactable || !canvasGroup.blocksRaycasts)
                {
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    if (debugLogs) Debug.Log("TopBarManager: Corrigida interatividade do CanvasGroup");
                }
            }

            // Verificar filhos
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                    if (debugLogs) Debug.Log($"TopBarManager: Ativado filho {child.name} que estava inativo");
                }
            }
        }
    }

    private void InitializeButtons()
    {
        allButtons.Clear();

        if (hubButton.button != null)
        {
            if (hubButton.buttonImage == null)
                hubButton.buttonImage = hubButton.button.GetComponent<Image>();

            allButtons.Add(hubButton);
        }

        if (engineButton.button != null)
        {
            if (engineButton.buttonImage == null)
                engineButton.buttonImage = engineButton.button.GetComponent<Image>();

            allButtons.Add(engineButton);
        }

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

                // Usar o sistema de registro para ativar o HalfView da cena atual
                if (currentScene == "ProfileScene")
                {
                    HalfViewRegistry.ShowHalfViewForCurrentScene();
                }
            });
        }
    }

    private void OnDestroy()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged -= OnNavigationManagerSceneChanged;
        }

        // Remover o listener do SceneManager
        SceneManager.sceneLoaded -= OnSceneLoaded;

        UserDataStore.OnUserDataChanged -= OnUserDataChanged;

        if (_instance == this)
        {
            _instance = null;
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

    private void OnSceneChanged(string sceneName)
    {
        if (debugLogs) Debug.Log($"TopBarManager: Cena mudou para {sceneName}");
        currentScene = sceneName;

        if (sceneName == "QuestionScene")
        {
            Debug.Log("TopBarManager: Detectada QuestionScene, aplicando tratamento especial");
            ForceVisibilityInScene(sceneName);
        }
        else
        {
            UpdateButtonVisibility(currentScene);
            AdjustVisibilityForCurrentScene();
        }


        UpdateButtonVisibility(currentScene);
        AdjustVisibilityForCurrentScene();

        if (sceneName != "ProfileScene")
        {
            halfViewComponent = null;
        }
        else
        {
            StartCoroutine(FindHalfViewMenuAfterDelay());
        }
    }

    private System.Collections.IEnumerator FindHalfViewMenuAfterDelay()
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

    private void AdjustVisibilityForCurrentScene()
    {
        bool shouldShowTopBar = !scenesWithoutTopBar.Contains(currentScene);

        if (debugLogs)
        {
            Debug.Log($"TopBarManager: Ajustando visibilidade para cena {currentScene}");
            Debug.Log($"TopBarManager: Deve mostrar TopBar = {shouldShowTopBar}");
            Debug.Log($"TopBarManager: Estado atual = {gameObject.activeSelf}");
        }

        // Aplicar a visibilidade com abordagem completa
        if (shouldShowTopBar != gameObject.activeSelf)
        {
            gameObject.SetActive(shouldShowTopBar);

            // Se deve mostrar, garantir que todos os componentes estão ativos
            if (shouldShowTopBar)
            {
                Canvas canvas = GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvas.enabled = true;
                }

                CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }

                // Ativar filhos diretos
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }

            if (debugLogs) Debug.Log($"TopBarManager: Visibilidade ajustada para {shouldShowTopBar}");
        }
        else if (shouldShowTopBar)
        {
            // Mesmo que já esteja no estado correto, garantir que os componentes estão ativos
            EnsureComponentsAreActive();
        }
    }

    public void UpdateButtonVisibility(string sceneName)
    {
        if (debugLogs) Debug.Log($"Atualizando TopBar para cena: {sceneName}");

        foreach (var button in allButtons)
        {
            if (button != null && button.button != null && button.buttonImage != null)
            {
                bool isActive = button.visibleInScenes.Contains(sceneName);

                // Tornar botão interativo apenas quando visível
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

        if (debugLogs) Debug.Log($"TopBar textos definidos manualmente: Score = {title}, Nickname = {subtitle}");
    }

    public void AddSceneToButtonVisibility(string buttonName, string sceneName)
    {
        TopButton targetButton = null;
        if (hubButton.buttonName == buttonName)
        {
            targetButton = hubButton;
        }
        else if (engineButton.buttonName == buttonName)
        {
            targetButton = engineButton;
        }

        if (targetButton != null)
        {
            if (!targetButton.visibleInScenes.Contains(sceneName))
            {
                targetButton.visibleInScenes.Add(sceneName);

                if (currentScene == sceneName)
                {
                    UpdateButtonVisibility(currentScene);
                }
            }
        }
        else
        {
            Debug.LogError($"Botão com nome '{buttonName}' não encontrado!");
            Debug.Log($"Botões disponíveis: hubButton='{hubButton.buttonName}', engineButton='{engineButton.buttonName}'");
        }
    }

    public void RemoveSceneFromButtonVisibility(string buttonName, string sceneName)
    {
        TopButton targetButton = null;
        if (hubButton.buttonName == buttonName)
        {
            targetButton = hubButton;
        }

        else if (engineButton.buttonName == buttonName)
        {
            targetButton = engineButton;
        }

        if (targetButton != null)
        {
            if (targetButton.visibleInScenes.Contains(sceneName))
            {
                targetButton.visibleInScenes.Remove(sceneName);

                if (currentScene == sceneName)
                {
                    UpdateButtonVisibility(currentScene);
                }
            }
        }
        else
        {
            Debug.LogError($"Botão com nome '{buttonName}' não encontrado!");
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

    public void DebugListButtons()
    {
        Debug.Log("=== TopBarManager - Estado Atual ===");
        Debug.Log($"Cena atual: {currentScene}");
        Debug.Log($"Visibilidade atual: {gameObject.activeSelf}");
        Debug.Log($"Cenas sem TopBar: {string.Join(", ", scenesWithoutTopBar)}");

        Canvas canvas = GetComponent<Canvas>();
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        Debug.Log($"Canvas habilitado: {(canvas != null ? canvas.enabled : false)}");
        Debug.Log($"CanvasGroup alpha: {(canvasGroup != null ? canvasGroup.alpha : 0)}");
        Debug.Log($"CanvasGroup interactable: {(canvasGroup != null ? canvasGroup.interactable : false)}");

        Debug.Log($"hubButton: {hubButton.buttonName}");
        Debug.Log($"Cenas visíveis para hubButton: {string.Join(", ", hubButton.visibleInScenes)}");

        Debug.Log($"engineButton: {engineButton.buttonName}");
        Debug.Log($"Cenas visíveis para engineButton: {string.Join(", ", engineButton.visibleInScenes)}");

        Debug.Log("=====================================");
    }

    public void ForceVisibilityInScene(string sceneName)
    {
        Debug.Log($"TopBarManager: Forçando visibilidade na cena {sceneName}");

        // Verificar se a cena está na lista de exclusão e remover se estiver
        if (scenesWithoutTopBar.Contains(sceneName))
        {
            scenesWithoutTopBar.Remove(sceneName);
            Debug.Log($"TopBarManager: '{sceneName}' removida da lista de cenas sem TopBar");
        }

        // Se a cena atual for a que queremos forçar a visibilidade
        if (currentScene == sceneName)
        {
            // Garantir que o gameObject esteja ativo
            gameObject.SetActive(true);

            // Ativar todos os componentes relevantes
            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.enabled = true;
            }

            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            // Atualizar a visibilidade dos botões para esta cena
            UpdateButtonVisibility(currentScene);

            Debug.Log($"TopBarManager: Visibilidade forçada com sucesso na cena {sceneName}");
        }
    }

    public void ForceRefreshVisibility()
    {
        if (debugLogs) Debug.Log("TopBarManager: Forçando atualização da visibilidade");

        string activeScene = SceneManager.GetActiveScene().name;
        currentScene = activeScene;

        UpdateButtonVisibility(currentScene);
        AdjustVisibilityForCurrentScene();
        ValidateTopBarState();

        if (debugLogs) Debug.Log($"TopBarManager: Atualização forçada concluída. Visibilidade: {gameObject.activeSelf}");
    }

    private void OnEnable()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnNavigationComplete += OnNavigationComplete;

            if (debugLogs) Debug.Log("TopBarManager: Registrado para evento OnNavigationComplete");
        }
    }

    private void OnDisable()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnNavigationComplete -= OnNavigationComplete;
        }
    }

    // Resposta à verificação final de navegação
    private void OnNavigationComplete(string sceneName)
    {
        // Garantir que não estamos processando muitas verificações rapidamente
        if (Time.time - lastVerificationTime < 0.5f) return;
        lastVerificationTime = Time.time;

        if (debugLogs) Debug.Log($"TopBarManager: OnNavigationComplete para cena {sceneName}");

        // Verificar se este cenário deve mostrar a TopBar
        bool shouldShowTopBar = !scenesWithoutTopBar.Contains(sceneName);

        // Se a TopBar não estiver no estado correto, aplicar correção
        if (gameObject.activeSelf != shouldShowTopBar)
        {
            if (debugLogs) Debug.Log($"TopBarManager: Corrigindo visibilidade na conclusão da navegação. Deve ser: {shouldShowTopBar}");
            gameObject.SetActive(shouldShowTopBar);
        }

        // Se deve mostrar, garantir que os componentes estão corretos
        if (shouldShowTopBar)
        {
            EnsureComponentsAreActive();
        }
    }

    // Método auxiliar para garantir componentes ativos
    private void EnsureComponentsAreActive()
    {
        // Verificar Canvas
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && !canvas.enabled)
        {
            canvas.enabled = true;
            if (debugLogs) Debug.Log("TopBarManager: Reativado Canvas");
        }

        // Verificar CanvasGroup
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            bool needsFix = false;

            if (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha = 1f;
                needsFix = true;
            }

            if (!canvasGroup.interactable)
            {
                canvasGroup.interactable = true;
                needsFix = true;
            }

            if (!canvasGroup.blocksRaycasts)
            {
                canvasGroup.blocksRaycasts = true;
                needsFix = true;
            }

            if (needsFix && debugLogs)
                Debug.Log("TopBarManager: Corrigidas propriedades do CanvasGroup");
        }

        // Verificar filhos diretos
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                if (debugLogs) Debug.Log($"TopBarManager: Reativado filho {child.name}");
            }
        }
    }

    // Método público para forçar verificação do estado
    public void ForceStatusCheck()
    {
        if (debugLogs) Debug.Log("TopBarManager: Verificação forçada do estado");

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        bool shouldShowTopBar = !scenesWithoutTopBar.Contains(currentScene);

        if (debugLogs) Debug.Log($"TopBarManager: Deve ser {(shouldShowTopBar ? "visível" : "oculta")} na cena {currentScene}");

        // Ajustar visibilidade
        if (gameObject.activeSelf != shouldShowTopBar)
        {
            gameObject.SetActive(shouldShowTopBar);
            if (debugLogs) Debug.Log($"TopBarManager: Visibilidade ajustada para {shouldShowTopBar}");
        }

        // Se deve estar visível, garantir que componentes estão ativos
        if (shouldShowTopBar)
        {
            EnsureComponentsAreActive();
        }

        // Atualizar estado dos botões
        UpdateButtonVisibility(currentScene);
    }

}