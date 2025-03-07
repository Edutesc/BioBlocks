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
    }

    /// <summary>
    /// Navega para a cena especificada.
    /// </summary>
    /// <param name="sceneName">Nome da cena para navegar</param>
    /// <param name="sceneData">Dados opcionais para passar para a cena</param>
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
            
            // Carregar a cena
            SceneManager.LoadScene(sceneName);
            
            if (debugLogs)
                Debug.Log($"NavigationManager: Cena carregada com sucesso: {sceneName}");
            
            // Notificar explicitamente sobre a mudança (além do evento sceneLoaded)
            OnSceneChanged?.Invoke(sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError($"NavigationManager: Erro ao carregar cena {sceneName}: {e.Message}");
        }
    }
    
    /// <summary>
    /// Handler para botões de navegação da BottomBar.
    /// Este método deve ser chamado pelo evento OnClick dos botões.
    /// </summary>
    /// <param name="buttonName">Nome do botão ou cena para navegação</param>
    public void OnNavigationButtonClicked(string buttonName)
    {
        if (debugLogs)
            Debug.Log($"NavigationManager: Botão clicado - {buttonName}");
        
        NavigateTo(buttonName);
    }
    
    /// <summary>
    /// Adiciona ou atualiza um mapeamento de botão para cena
    /// </summary>
    public void AddButtonSceneMapping(string buttonName, string sceneName)
    {
        buttonSceneMapping[buttonName] = sceneName;
    }
}