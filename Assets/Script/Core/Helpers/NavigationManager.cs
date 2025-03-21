using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

/// <summary>
/// Gerenciador central de navegação que coordena a mudança de cenas 
/// e a comunicação com a BottomBar. Este manager deve ser carregado em todas as cenas.
/// </summary>
public class NavigationManager : MonoBehaviour
{
    private static NavigationManager _instance;
    public event Action<string> OnSceneChanged;
    public event Action<string> OnNavigationComplete;
    private float lastSceneLoadTime = 0f;
    private bool checkingUIState = false;

    private Dictionary<string, string> buttonSceneMapping = new Dictionary<string, string>()
    {
        { "HomeButton", "PathwayScene" },
        { "RankingButton", "RankingScene" },
        { "FavoritesButton", "RankingScene" },
        { "MedalsButton", "ProfileScene" },
        { "ProfileButton", "ProfileScene" }
    };

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    public static NavigationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Primeiro procura uma instância existente
                _instance = FindFirstObjectByType<NavigationManager>();

                if (_instance == null)
                {
                    // Se não encontrar, cria uma nova na raiz
                    var go = new GameObject("NavigationManager");
                    go.transform.SetParent(null); // Garante que está na raiz
                    _instance = go.AddComponent<NavigationManager>();
                    DontDestroyOnLoad(go);

                    if (go.GetComponent<NavigationManager>().debugLogs)
                        Debug.Log("NavigationManager: Nova instância criada");
                }
                else
                {
                    // Se encontrou uma instância, garante que está na raiz
                    _instance.transform.SetParent(null);
                    DontDestroyOnLoad(_instance.gameObject);

                    if (_instance.debugLogs)
                        Debug.Log("NavigationManager: Instância existente encontrada");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        transform.SetParent(null); // Garante que está na raiz
        DontDestroyOnLoad(gameObject);

        // Registrar para o evento de cena carregada
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (debugLogs)
            Debug.Log("NavigationManager inicializado");
    }

    private void OnDestroy()
    {
        // Limpar eventos
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (debugLogs)
            Debug.Log($"NavigationManager: Cena carregada - {scene.name}");

        // Notificar sobre a mudança de cena
        OnSceneChanged?.Invoke(scene.name);

        // Agendar verificação de persistência de UI após o carregamento
        // Apenas se já passou pelo menos 0.5s desde a última navegação
        // para evitar múltiplas verificações em sucessão rápida
        if (Time.time - lastSceneLoadTime > 0.5f)
        {
            Invoke(nameof(EnsureUIConsistency), 0.2f);
        }
    }

    public void NavigateTo(string sceneName, Dictionary<string, object> sceneData = null)
    {
        if (debugLogs)
            Debug.Log($"NavigationManager: Navegando para: {sceneName}");

        try
        {
            // Verificar se existe um mapeamento para o nome do botão
            if (buttonSceneMapping.ContainsKey(sceneName))
            {
                if (debugLogs)
                    Debug.Log($"NavigationManager: Convertendo {sceneName} para {buttonSceneMapping[sceneName]}");

                sceneName = buttonSceneMapping[sceneName];
            }

            // Configurar dados da cena, se fornecidos
            if (sceneData != null && SceneDataManager.Instance != null)
            {
                SceneDataManager.Instance.SetData(sceneData);
            }

            // Registrar o tempo da carga da cena para verificações posteriores
            lastSceneLoadTime = Time.time;

            // Carregar a cena
            SceneManager.LoadScene(sceneName);

            if (debugLogs)
                Debug.Log($"NavigationManager: Cena carregada com sucesso: {sceneName}");

            // Notificar explicitamente sobre a mudança (além do evento sceneLoaded)
            OnSceneChanged?.Invoke(sceneName);

            // Agendar verificação de persistência de UI após o carregamento
           // Invoke(nameof(EnsureUIConsistency), 0.2f);
        }
        catch (Exception e)
        {
            Debug.LogError($"NavigationManager: Erro ao carregar cena {sceneName}: {e.Message}");
        }
    }


    private void EnsureUIConsistency()
    {
        // Evitar verificações simultâneas
        if (checkingUIState) return;
        checkingUIState = true;

        try
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (debugLogs)
                Debug.Log($"NavigationManager: Verificando consistência da UI na cena {currentSceneName}");

            // Verificar TopBarManager
            TopBarManager topBar = FindFirstObjectByType<TopBarManager>();
            if (topBar != null)
            {
                // Lista de cenas onde a TopBar não deve aparecer
                List<string> scenesWithoutTopBar = GetField<List<string>>(topBar, "scenesWithoutTopBar") ?? new List<string>();
                bool shouldShowTopBar = !scenesWithoutTopBar.Contains(currentSceneName);

                if (debugLogs)
                    Debug.Log($"NavigationManager: TopBar deve ser {(shouldShowTopBar ? "visível" : "oculta")} em {currentSceneName}");

                // Aplicar visibilidade
                topBar.gameObject.SetActive(shouldShowTopBar);

                // Garantir que os componentes estejam corretos
                if (shouldShowTopBar)
                {
                    // Ativar Canvas e CanvasGroup
                    Canvas canvas = topBar.GetComponent<Canvas>();
                    if (canvas != null) canvas.enabled = true;

                    CanvasGroup canvasGroup = topBar.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = 1f;
                        canvasGroup.interactable = true;
                        canvasGroup.blocksRaycasts = true;
                    }

                    // Ativar filhos diretamente
                    foreach (Transform child in topBar.transform)
                    {
                        if (!child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(true);
                            if (debugLogs)
                                Debug.Log($"NavigationManager: Reativado filho da TopBar: {child.name}");
                        }
                    }
                }
            }

            // Verificar NavigationBottomBarManager
            NavigationBottomBarManager bottomBar = FindFirstObjectByType<NavigationBottomBarManager>();
            if (bottomBar != null)
            {
                // Lista de cenas onde a BottomBar não deve aparecer
                List<string> scenesWithoutBottomBar = GetField<List<string>>(bottomBar, "scenesWithoutBottomBar") ?? new List<string>();
                bool shouldShowBottomBar = !scenesWithoutBottomBar.Contains(currentSceneName);

                if (debugLogs)
                    Debug.Log($"NavigationManager: BottomBar deve ser {(shouldShowBottomBar ? "visível" : "oculta")} em {currentSceneName}");

                // Aplicar visibilidade
                bottomBar.gameObject.SetActive(shouldShowBottomBar);

                // Se usar Canvas ou CanvasGroup, garantir consistência
                if (!shouldShowBottomBar)
                {
                    Canvas canvas = bottomBar.GetComponent<Canvas>();
                    if (canvas != null) canvas.enabled = false;

                    CanvasGroup canvasGroup = bottomBar.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = 0f;
                        canvasGroup.interactable = false;
                        canvasGroup.blocksRaycasts = false;
                    }
                }
            }

            // Notificar que a verificação de UI está completa
            OnNavigationComplete?.Invoke(currentSceneName);

            if (debugLogs)
                Debug.Log($"NavigationManager: Verificação de consistência de UI concluída para {currentSceneName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"NavigationManager: Erro durante verificação de UI: {e.Message}");
        }
        finally
        {
            checkingUIState = false;
        }
    }

    private T GetField<T>(object obj, string fieldName)
    {
        if (obj == null) return default;

        var field = obj.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            return (T)field.GetValue(obj);
        }

        return default;
    }

    public void OnNavigationButtonClicked(string buttonName)
    {
        if (debugLogs)
            Debug.Log($"NavigationManager: Botão clicado - {buttonName}");

        NavigateTo(buttonName);
    }

    public void AddButtonSceneMapping(string buttonName, string sceneName)
    {
        buttonSceneMapping[buttonName] = sceneName;
    }
}