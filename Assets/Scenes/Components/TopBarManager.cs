using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// public class TopBarManager : MonoBehaviour
// {
//     [System.Serializable]
//     public class TopButton
//     {
//         public string buttonName;
//         public Button button;
//         public Image buttonImage;
//         public List<string> visibleInScenes = new List<string>();
//     }

//     [Header("Textos da TopBar")]
//     [SerializeField] private TMP_Text weekScore;
//     [SerializeField] private TMP_Text nickname;

//     [Header("Botões da TopBar")]
//     [SerializeField] private TopButton hubButton;
//     [SerializeField] private TopButton engineButton;

//     [Header("Configurações")]
//     [SerializeField] private string currentScene = "";
//     [SerializeField] private bool debugLogs = true;

//     [Header("Persistência")]
//     [SerializeField] private List<string> scenesWithoutTopBar = new List<string>() { "Login", "Splash" };

//     private Dictionary<string, TopButton> buttonsByName = new Dictionary<string, TopButton>();
//     private List<TopButton> allButtons = new List<TopButton>();
//     private static TopBarManager _instance;
//     private HalfViewComponent halfViewComponent;
//     private bool isSceneBeingLoaded = false;
//     private float lastVerificationTime = 0f;

//     public static TopBarManager Instance => _instance;

//     public string Title
//     {
//         get { return weekScore ? weekScore.text : ""; }
//         set { if (weekScore) weekScore.text = value; }
//     }

//     public string Subtitle
//     {
//         get { return nickname ? nickname.text : ""; }
//         set { if (nickname) nickname.text = value; }
//     }

//     private void Awake()
//     {
//         if (_instance != null && _instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         _instance = this;
//         DontDestroyOnLoad(gameObject);
//         InitializeButtons();

//         // Registrar para eventos de mudança de cena
//         SceneManager.sceneLoaded += OnSceneLoaded;

//         if (debugLogs) Debug.Log("TopBarManager inicializado");
//     }

//     private void Start()
//     {
//         RegisterWithNavigationManager();
        
//         // Configurar para a cena atual
//         string activeScene = SceneManager.GetActiveScene().name;
//         if (!string.IsNullOrEmpty(activeScene))
//         {
//             currentScene = activeScene;
//         }

//         // Atualizar estado para a cena inicial
//         UpdateTopBarState(currentScene);

//         // Registrar para mudanças de dados do usuário
//         UserDataStore.OnUserDataChanged += OnUserDataChanged;
//         UpdateFromCurrentUserData();
//     }

//     private void RegisterWithNavigationManager()
//     {
//         if (NavigationManager.Instance != null)
//         {
//             NavigationManager.Instance.OnSceneChanged += OnSceneChanged;
//             NavigationManager.Instance.OnNavigationComplete += OnNavigationComplete;
//             if (debugLogs) Debug.Log("TopBarManager: Registrado com o NavigationManager");
//         }
//         else
//         {
//             Debug.LogWarning("TopBarManager: NavigationManager não encontrado! Usando apenas eventos SceneManager.");
//         }
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         if (debugLogs) Debug.Log($"TopBarManager: Cena carregada diretamente: {scene.name}");
        
//         isSceneBeingLoaded = true;
//         currentScene = scene.name;
        
//         // Usar Invoke para garantir que seja processado após inicialização da cena
//         Invoke(nameof(HandleSceneLoadComplete), 0.1f);
//     }

//     private void HandleSceneLoadComplete()
//     {
//         UpdateTopBarState(currentScene);
//         isSceneBeingLoaded = false;

//         if (debugLogs) Debug.Log($"TopBarManager: Configuração completa para cena {currentScene}, visibilidade: {gameObject.activeSelf}");
//     }

//     private void OnSceneChanged(string sceneName)
//     {
//         // Ignorar se ainda estamos carregando uma cena
//         if (isSceneBeingLoaded)
//         {
//             if (debugLogs) Debug.Log("TopBarManager: Ignorando notificação durante carregamento de cena");
//             return;
//         }

//         if (debugLogs) Debug.Log($"TopBarManager: Notificação de mudança para cena {sceneName}");
//         currentScene = sceneName;
        
//         // Aplicar tratamento especial para QuestionScene
//         if (sceneName == "QuestionScene")
//         {
//             Debug.Log("TopBarManager: Detectada QuestionScene, aplicando tratamento especial");
//             EnsureTopBarVisibilityInScene(sceneName);
//         }
//         else
//         {
//             UpdateTopBarState(sceneName);
//         }

//         // Gerenciar componente HalfView
//         HandleHalfViewComponent(sceneName);
//     }

//     private void OnNavigationComplete(string sceneName)
//     {
//         // Limitar frequência de verificações
//         if (Time.time - lastVerificationTime < 0.5f) return;
//         lastVerificationTime = Time.time;

//         if (debugLogs) Debug.Log($"TopBarManager: OnNavigationComplete para cena {sceneName}");
        
//         // Verificação final após navegação completa
//         UpdateTopBarState(sceneName);
//     }

//     private void HandleHalfViewComponent(string sceneName)
//     {
//         if (sceneName != "ProfileScene")
//         {
//             halfViewComponent = null;
//         }
//         else if (halfViewComponent == null)
//         {
//             StartCoroutine(FindHalfViewMenuAfterDelay());
//         }
//     }

//     private IEnumerator FindHalfViewMenuAfterDelay()
//     {
//         yield return null;

//         halfViewComponent = FindFirstObjectByType<HalfViewComponent>(FindObjectsInactive.Include);

//         if (halfViewComponent != null)
//         {
//             if (debugLogs) Debug.Log("HalfViewMenu encontrado na ProfileScene");
//         }
//         else
//         {
//             Debug.LogWarning("HalfViewMenu não encontrado na ProfileScene!");
//         }
//     }

//     private void InitializeButtons()
//     {
//         // Limpar as coleções
//         allButtons.Clear();
//         buttonsByName.Clear();

//         // Inicializar botão Hub
//         InitializeButton(hubButton);
        
//         // Inicializar botão Engine
//         InitializeButton(engineButton);

//         // Configurar os eventos de clique
//         SetupButtonListeners();
//     }

//     private void InitializeButton(TopButton button)
//     {
//         if (button.button != null)
//         {
//             // Encontrar a imagem do botão se não estiver definida
//             if (button.buttonImage == null)
//                 button.buttonImage = button.button.GetComponent<Image>();

//             // Adicionar à lista e dicionário para acesso rápido
//             allButtons.Add(button);
//             buttonsByName[button.buttonName] = button;
//         }
//     }

//     private void SetupButtonListeners()
//     {
//         // Configurar o botão Hub
//         if (hubButton.button != null)
//         {
//             hubButton.button.onClick.RemoveAllListeners();
//             hubButton.button.onClick.AddListener(() =>
//             {
//                 if (debugLogs) Debug.Log($"Botão {hubButton.buttonName} clicado");
//                 if (NavigationManager.Instance != null)
//                 {
//                     NavigationManager.Instance.NavigateTo("ProfileScene");
//                 }
//                 else
//                 {
//                     Debug.LogError("NavigationManager não encontrado!");
//                 }
//             });
//         }

//         // Configurar o botão Engine
//         if (engineButton.button != null)
//         {
//             engineButton.button.onClick.RemoveAllListeners();
//             engineButton.button.onClick.AddListener(() =>
//             {
//                 if (debugLogs) Debug.Log($"Botão {engineButton.buttonName} clicado");

//                 // Usar o sistema de registro para ativar o HalfView da cena atual
//                 if (currentScene == "ProfileScene")
//                 {
//                     HalfViewRegistry.ShowHalfViewForCurrentScene();
//                 }
//             });
//         }
//     }

//     private void OnDestroy()
//     {
//         // Remover todos os listeners
//         if (NavigationManager.Instance != null)
//         {
//             NavigationManager.Instance.OnSceneChanged -= OnSceneChanged;
//             NavigationManager.Instance.OnNavigationComplete -= OnNavigationComplete;
//         }

//         SceneManager.sceneLoaded -= OnSceneLoaded;
//         UserDataStore.OnUserDataChanged -= OnUserDataChanged;

//         if (_instance == this)
//         {
//             _instance = null;
//         }
//     }

//     private void OnEnable()
//     {
//         if (NavigationManager.Instance != null)
//         {
//             NavigationManager.Instance.OnNavigationComplete += OnNavigationComplete;
//         }
//     }

//     private void OnDisable()
//     {
//         if (NavigationManager.Instance != null)
//         {
//             NavigationManager.Instance.OnNavigationComplete -= OnNavigationComplete;
//         }
//     }

//     private void OnUserDataChanged(UserData userData)
//     {
//         if (userData != null)
//         {
//             UpdateUserInfoDisplay(userData);
//             if (debugLogs) Debug.Log($"TopBarManager: Dados do usuário atualizados - WeekScore: {userData.WeekScore}, Nickname: {userData.NickName}");
//         }
//     }

//     private void UpdateUserInfoDisplay(UserData userData)
//     {
//         if (weekScore != null)
//         {
//             weekScore.text = userData.WeekScore.ToString();
//         }

//         if (nickname != null)
//         {
//             nickname.text = userData.NickName;
//         }
//     }

//     private void UpdateFromCurrentUserData()
//     {
//         UserData currentUserData = UserDataStore.CurrentUserData;
//         if (currentUserData != null)
//         {
//             UpdateUserInfoDisplay(currentUserData);

//             if (debugLogs) Debug.Log($"TopBarManager: Carregados dados iniciais - WeekScore: {currentUserData.WeekScore}, Nickname: {currentUserData.NickName}");
//         }
//         else if (debugLogs)
//         {
//             Debug.LogWarning("TopBarManager: CurrentUserData é null, não foi possível carregar dados iniciais");
//         }
//     }

//     // Método centralizado para atualizar o estado completo da TopBar
//     private void UpdateTopBarState(string sceneName)
//     {
//         // Atualizar o nome da cena atual
//         currentScene = sceneName;
        
//         // Atualizar visibilidade dos botões
//         UpdateButtonVisibility(sceneName);
        
//         // Ajustar visibilidade geral da TopBar
//         UpdateTopBarVisibility();
        
//         // Verificar e corrigir quaisquer problemas
//         EnsureTopBarIntegrity();
        
//         if (debugLogs) Debug.Log($"TopBarManager: Estado atualizado para cena {sceneName}");
//     }

//     // Método para atualizar a visibilidade dos botões
//     private void UpdateButtonVisibility(string sceneName)
//     {
//         if (debugLogs) Debug.Log($"Atualizando visibilidade dos botões para cena: {sceneName}");

//         foreach (var button in allButtons)
//         {
//             if (button != null && button.button != null && button.buttonImage != null)
//             {
//                 bool isActive = button.visibleInScenes.Contains(sceneName);

//                 // Tornar botão interativo apenas quando visível
//                 button.button.interactable = isActive;

//                 // Atualizar a transparência da imagem
//                 Color buttonColor = button.buttonImage.color;
//                 button.buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, isActive ? 1 : 0);

//                 if (debugLogs)
//                 {
//                     Debug.Log($"Botão: {button.buttonName} - Visível e interativo: {isActive} na cena {sceneName}");
//                 }
//             }
//         }
//     }

//     // Método para atualizar a visibilidade geral da TopBar
//     private void UpdateTopBarVisibility()
// {
//     bool shouldShowTopBar = !scenesWithoutTopBar.Contains(currentScene);

//     if (debugLogs)
//     {
//         Debug.Log($"TopBarManager: Ajustando visibilidade para cena {currentScene}");
//         Debug.Log($"TopBarManager: Deve mostrar TopBar = {shouldShowTopBar}");
//         Debug.Log($"TopBarManager: Estado atual = {gameObject.activeSelf}");
//     }

//     // Pegar referência ao filho TopBar
//     Transform topBarChild = transform.Find("TopBar");
    
//     // Aplicar visibilidade ao GameObject do TopBarManager (PersistentTopBar)
//     gameObject.SetActive(shouldShowTopBar);
    
//     // Se tiver um filho específico chamado TopBar, garantir que ele também está visível
//     if (topBarChild != null && shouldShowTopBar)
//     {
//         topBarChild.gameObject.SetActive(true);
//         if (debugLogs) Debug.Log($"TopBarManager: Visibilidade do filho TopBar ajustada para {shouldShowTopBar}");
//     }
    
//     if (debugLogs) Debug.Log($"TopBarManager: Visibilidade ajustada para {shouldShowTopBar}");
// }


//     // Método para garantir que todos os componentes estejam funcionando corretamente
//     private void EnsureTopBarIntegrity()
// {
//     // Verificar apenas se a TopBar deve estar visível
//     if (!gameObject.activeSelf) return;
    
//     // Verificar Canvas
//     Canvas canvas = GetComponent<Canvas>();
//     if (canvas != null && !canvas.enabled)
//     {
//         canvas.enabled = true;
//         if (debugLogs) Debug.Log("TopBarManager: Corrigido Canvas desativado");
//     }

//     // Verificar CanvasGroup
//     CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
//     if (canvasGroup != null)
//     {
//         bool needsFix = false;

//         if (canvasGroup.alpha < 1f)
//         {
//             canvasGroup.alpha = 1f;
//             needsFix = true;
//         }

//         if (!canvasGroup.interactable)
//         {
//             canvasGroup.interactable = true;
//             needsFix = true;
//         }

//         if (!canvasGroup.blocksRaycasts)
//         {
//             canvasGroup.blocksRaycasts = true;
//             needsFix = true;
//         }

//         if (needsFix && debugLogs)
//             Debug.Log("TopBarManager: Corrigidas propriedades do CanvasGroup");
//     }

//     // Verificar o filho específico TopBar
//     Transform topBarChild = transform.Find("TopBar");
//     if (topBarChild != null && !topBarChild.gameObject.activeSelf)
//     {
//         topBarChild.gameObject.SetActive(true);
//         if (debugLogs) Debug.Log("TopBarManager: Ativado filho TopBar que estava inativo");
//     }

//     // Verificar outros filhos (pode ser removido se não for mais necessário)
//     foreach (Transform child in transform)
//     {
//         if (!child.gameObject.activeSelf)
//         {
//             child.gameObject.SetActive(true);
//             if (debugLogs) Debug.Log($"TopBarManager: Ativado filho {child.name} que estava inativo");
//         }
//     }
// }

//     // Métodos públicos para manipulação externa

//     // Método para forçar a visibilidade da TopBar em uma cena específica
//     public void EnsureTopBarVisibilityInScene(string sceneName)
//     {
//         Debug.Log($"TopBarManager: Forçando visibilidade na cena {sceneName}");

//         // Remover da lista de exclusão se presente
//         if (scenesWithoutTopBar.Contains(sceneName))
//         {
//             scenesWithoutTopBar.Remove(sceneName);
//             Debug.Log($"TopBarManager: '{sceneName}' removida da lista de cenas sem TopBar");
//         }

//         // Se esta for a cena atual, atualizar imediatamente
//         if (currentScene == sceneName)
//         {
//             gameObject.SetActive(true);
//             EnsureTopBarIntegrity();
//             UpdateButtonVisibility(currentScene);
            
//             Debug.Log($"TopBarManager: Visibilidade forçada com sucesso na cena {sceneName}");
//         }
//     }

//     // Método para forçar atualização do estado da TopBar
//     public void ForceRefreshState()
//     {
//         if (debugLogs) Debug.Log("TopBarManager: Forçando atualização do estado");

//         string activeScene = SceneManager.GetActiveScene().name;
//         UpdateTopBarState(activeScene);
        
//         if (debugLogs) Debug.Log($"TopBarManager: Atualização forçada concluída. Visibilidade: {gameObject.activeSelf}");
//     }

//     // Método para definir textos da TopBar
//     public void SetTopBarTexts(string title, string subtitle = "")
//     {
//         Title = title;
//         Subtitle = subtitle;

//         if (debugLogs) Debug.Log($"TopBar textos definidos manualmente: Score = {title}, Nickname = {subtitle}");
//     }

//     // Métodos para gerenciar visibilidade dos botões em diferentes cenas
//     public void ManageButtonSceneVisibility(string buttonName, string sceneName, bool addVisibility)
//     {
//         // Tentar obter o botão pelo nome
//         if (!buttonsByName.TryGetValue(buttonName, out TopButton targetButton))
//         {
//             Debug.LogError($"Botão com nome '{buttonName}' não encontrado!");
//             Debug.Log($"Botões disponíveis: {string.Join(", ", buttonsByName.Keys)}");
//             return;
//         }

//         bool listChanged = false;

//         if (addVisibility)
//         {
//             // Adicionar cena se não existir
//             if (!targetButton.visibleInScenes.Contains(sceneName))
//             {
//                 targetButton.visibleInScenes.Add(sceneName);
//                 listChanged = true;
//             }
//         }
//         else
//         {
//             // Remover cena se existir
//             if (targetButton.visibleInScenes.Contains(sceneName))
//             {
//                 targetButton.visibleInScenes.Remove(sceneName);
//                 listChanged = true;
//             }
//         }

//         // Atualizar a visibilidade se a cena atual for afetada
//         if (listChanged && currentScene == sceneName)
//         {
//             UpdateButtonVisibility(currentScene);
//         }
//     }

//     // Método para adicionar cena à visibilidade de um botão
//     public void AddSceneToButtonVisibility(string buttonName, string sceneName)
//     {
//         ManageButtonSceneVisibility(buttonName, sceneName, true);
//     }

//     // Método para remover cena da visibilidade de um botão
//     public void RemoveSceneFromButtonVisibility(string buttonName, string sceneName)
//     {
//         ManageButtonSceneVisibility(buttonName, sceneName, false);
//     }

//     // Método para adicionar cena à lista daquelas sem TopBar
//     public void AddSceneWithoutTopBar(string sceneName)
//     {
//         if (!scenesWithoutTopBar.Contains(sceneName))
//         {
//             scenesWithoutTopBar.Add(sceneName);
            
//             // Se for a cena atual, atualizar visibilidade
//             if (currentScene == sceneName)
//             {
//                 UpdateTopBarVisibility();
//             }
//         }
//     }

//     // Método para remover cena da lista daquelas sem TopBar
//     public void RemoveSceneWithoutTopBar(string sceneName)
//     {
//         if (scenesWithoutTopBar.Contains(sceneName))
//         {
//             scenesWithoutTopBar.Remove(sceneName);
            
//             // Se for a cena atual, atualizar visibilidade
//             if (currentScene == sceneName)
//             {
//                 UpdateTopBarVisibility();
//             }
//         }
//     }

//     // Método para depuração e diagnóstico
//     public void DebugListButtons()
//     {
//         Debug.Log("=== TopBarManager - Estado Atual ===");
//         Debug.Log($"Cena atual: {currentScene}");
//         Debug.Log($"Visibilidade atual: {gameObject.activeSelf}");
//         Debug.Log($"Cenas sem TopBar: {string.Join(", ", scenesWithoutTopBar)}");

//         Canvas canvas = GetComponent<Canvas>();
//         CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

//         Debug.Log($"Canvas habilitado: {(canvas != null ? canvas.enabled : false)}");
//         Debug.Log($"CanvasGroup alpha: {(canvasGroup != null ? canvasGroup.alpha : 0)}");
//         Debug.Log($"CanvasGroup interactable: {(canvasGroup != null ? canvasGroup.interactable : false)}");

//         foreach (var button in allButtons)
//         {
//             Debug.Log($"Botão: {button.buttonName}");
//             Debug.Log($"Cenas visíveis: {string.Join(", ", button.visibleInScenes)}");
//         }

//         Debug.Log("=====================================");
//     }
// }

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

    // Sobrescrever a lista de cenas invisíveis com valor padrão específico
    [Header("Persistência")]
    [SerializeField] private List<string> scenesWithoutTopBar = new List<string>() 
    { 
        "LoginView", "RegisterView", "ResetDatabaseView" 
    };

    private Dictionary<string, TopButton> buttonsByName = new Dictionary<string, TopButton>();
    private List<TopButton> allButtons = new List<TopButton>();
    private static TopBarManager _instance;
    private HalfViewComponent halfViewComponent;
    private float lastVerificationTime = 0f;

    // Implementação das propriedades abstratas
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

        // Registrar para eventos de mudança de cena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    protected override void OnStart()
    {
        // Registrar para mudanças de dados do usuário
        UserDataStore.OnUserDataChanged += OnUserDataChanged;
        UpdateFromCurrentUserData();
    }

    protected override void OnCleanup()
    {
        // Remover todos os listeners específicos
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
        
        // Adicionar evento adicional específico da TopBar
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
        
        // Usar Invoke para garantir que seja processado após inicialização da cena
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
        // Aplicar tratamento especial para QuestionScene
        if (sceneName == "QuestionScene")
        {
            Debug.Log("TopBarManager: Detectada QuestionScene, aplicando tratamento especial");
            EnsureTopBarVisibilityInScene(sceneName);
        }

        // Gerenciar componente HalfView
        HandleHalfViewComponent(sceneName);
    }

    private void OnNavigationComplete(string sceneName)
    {
        // Limitar frequência de verificações
        if (Time.time - lastVerificationTime < 0.5f) return;
        lastVerificationTime = Time.time;

        if (debugLogs) Debug.Log($"TopBarManager: OnNavigationComplete para cena {sceneName}");
        
        // Verificação final após navegação completa
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
        // Limpar as coleções
        allButtons.Clear();
        buttonsByName.Clear();

        // Inicializar botão Hub
        InitializeButton(hubButton);
        
        // Inicializar botão Engine
        InitializeButton(engineButton);

        // Configurar os eventos de clique
        SetupButtonListeners();
    }

    private void InitializeButton(TopButton button)
    {
        if (button.button != null)
        {
            // Encontrar a imagem do botão se não estiver definida
            if (button.buttonImage == null)
                button.buttonImage = button.button.GetComponent<Image>();

            // Adicionar à lista e dicionário para acesso rápido
            allButtons.Add(button);
            buttonsByName[button.buttonName] = button;
        }
    }

    private void SetupButtonListeners()
    {
        // Configurar o botão Hub
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

        // Configurar o botão Engine
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

    private void OnEnable()
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

    // Sobrescrever o método da classe base para usar a lógica específica dos botões da TopBar
    protected override void UpdateButtonVisibility(string sceneName)
    {
        if (debugLogs) Debug.Log($"Atualizando visibilidade dos botões para cena: {sceneName}");

        foreach (var button in allButtons)
        {
            if (button != null && button.button != null && button.buttonImage != null)
            {
                bool isActive = button.visibleInScenes.Contains(sceneName);

                // Tornar botão interativo apenas quando visível
                button.button.interactable = isActive;

                // Atualizar a transparência da imagem
                Color buttonColor = button.buttonImage.color;
                button.buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, isActive ? 1 : 0);

                if (debugLogs)
                {
                    Debug.Log($"Botão: {button.buttonName} - Visível e interativo: {isActive} na cena {sceneName}");
                }
            }
        }
    }

    // Método para forçar a visibilidade da TopBar em uma cena específica
    public void EnsureTopBarVisibilityInScene(string sceneName)
    {
        Debug.Log($"TopBarManager: Forçando visibilidade na cena {sceneName}");

        // Remover da lista de exclusão se presente
        RemoveSceneWithoutBar(sceneName);

        // Se esta for a cena atual, atualizar imediatamente
        if (currentScene == sceneName)
        {
            gameObject.SetActive(true);
            EnsureBarIntegrity();
            UpdateButtonVisibility(currentScene);
            
            Debug.Log($"TopBarManager: Visibilidade forçada com sucesso na cena {sceneName}");
        }
    }

    // Método para definir textos da TopBar
    public void SetTopBarTexts(string title, string subtitle = "")
    {
        Title = title;
        Subtitle = subtitle;

        if (debugLogs) Debug.Log($"TopBar textos definidos manualmente: Score = {title}, Nickname = {subtitle}");
    }

    // Métodos para gerenciar visibilidade dos botões em diferentes cenas
    public void ManageButtonSceneVisibility(string buttonName, string sceneName, bool addVisibility)
    {
        // Tentar obter o botão pelo nome
        if (!buttonsByName.TryGetValue(buttonName, out TopButton targetButton))
        {
            Debug.LogError($"Botão com nome '{buttonName}' não encontrado!");
            Debug.Log($"Botões disponíveis: {string.Join(", ", buttonsByName.Keys)}");
            return;
        }

        bool listChanged = false;

        if (addVisibility)
        {
            // Adicionar cena se não existir
            if (!targetButton.visibleInScenes.Contains(sceneName))
            {
                targetButton.visibleInScenes.Add(sceneName);
                listChanged = true;
            }
        }
        else
        {
            // Remover cena se existir
            if (targetButton.visibleInScenes.Contains(sceneName))
            {
                targetButton.visibleInScenes.Remove(sceneName);
                listChanged = true;
            }
        }

        // Atualizar a visibilidade se a cena atual for afetada
        if (listChanged && currentScene == sceneName)
        {
            UpdateButtonVisibility(currentScene);
        }
    }

    // Método para adicionar cena à visibilidade de um botão
    public void AddSceneToButtonVisibility(string buttonName, string sceneName)
    {
        ManageButtonSceneVisibility(buttonName, sceneName, true);
    }

    // Método para remover cena da visibilidade de um botão
    public void RemoveSceneFromButtonVisibility(string buttonName, string sceneName)
    {
        ManageButtonSceneVisibility(buttonName, sceneName, false);
    }

    // Compatibilidade com métodos anteriores
    public void AddSceneWithoutTopBar(string sceneName)
    {
        AddSceneWithoutBar(sceneName);
    }

    public void RemoveSceneWithoutTopBar(string sceneName)
    {
        RemoveSceneWithoutBar(sceneName);
    }

    // Método para depuração e diagnóstico
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