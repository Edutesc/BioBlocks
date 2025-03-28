using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    private static NavigationManager _instance;
    public event Action<string> OnSceneChanged;
    public event Action<string> OnNavigationComplete;
    private float lastSceneLoadTime = 0f;

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
                _instance = FindFirstObjectByType<NavigationManager>();

                if (_instance == null)
                {
                    var go = new GameObject("NavigationManager");
                    go.transform.SetParent(null);
                    _instance = go.AddComponent<NavigationManager>();
                    DontDestroyOnLoad(go);

                    if (go.GetComponent<NavigationManager>().debugLogs)
                        Debug.Log("NavigationManager: Nova instância criada");
                }
                else
                {
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
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (debugLogs)
            Debug.Log("NavigationManager inicializado");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (debugLogs)
            Debug.Log($"NavigationManager: Cena carregada - {scene.name}");

        OnSceneChanged?.Invoke(scene.name);
        OnNavigationComplete?.Invoke(scene.name);
    }

    public void NavigateTo(string sceneName, Dictionary<string, object> sceneData = null)
    {
        if (debugLogs)
            Debug.Log($"NavigationManager: Navegando para: {sceneName}");

        try
        {
            if (buttonSceneMapping.ContainsKey(sceneName))
            {
                if (debugLogs)
                    Debug.Log($"NavigationManager: Convertendo {sceneName} para {buttonSceneMapping[sceneName]}");

                sceneName = buttonSceneMapping[sceneName];
            }

            if (sceneData != null && SceneDataManager.Instance != null)
            {
                SceneDataManager.Instance.SetData(sceneData);
            }

            lastSceneLoadTime = Time.time;
            SceneManager.LoadScene(sceneName);

            if (debugLogs)
                Debug.Log($"NavigationManager: Cena carregada com sucesso: {sceneName}");

            OnSceneChanged?.Invoke(sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError($"NavigationManager: Erro ao carregar cena {sceneName}: {e.Message}");
        }
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