using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BarsManager : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] protected string currentScene = "";
    [SerializeField] protected bool debugLogs = true;

    [Header("Persistência")]
    [SerializeField] protected List<string> scenesWithoutBar = new List<string>();

    protected bool isSceneBeingLoaded = false;
    protected abstract string BarName { get; }
    protected abstract string BarChildName { get; }

    protected virtual void Awake()
    {
        ConfigureSingleton();
        DontDestroyOnLoad(gameObject);
        OnAwake();

        if (debugLogs) Debug.Log($"{BarName} inicializado");
    }

    protected virtual void Start()
    {
        RegisterWithNavigationManager();
        string activeScene = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(activeScene))
        {
            currentScene = activeScene;
        }
        
        UpdateBarState(currentScene);
        OnStart();
    }

    protected virtual void OnDestroy()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged -= OnSceneChanged;
        }

        OnCleanup();
    }

    protected virtual void OnEnable()
    {
        currentScene = SceneManager.GetActiveScene().name;
        AdjustVisibilityForCurrentScene();
        
        if (debugLogs) Debug.Log($"{BarName} OnEnable: verificando visibilidade para cena {currentScene}");
    }

    protected abstract void ConfigureSingleton();
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnCleanup() { }

    protected virtual void RegisterWithNavigationManager()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged += OnSceneChanged;
            if (debugLogs) Debug.Log($"{BarName}: Registrado com o NavigationManager");
        }
        else
        {
            Debug.LogWarning($"{BarName}: NavigationManager não encontrado!");
        }
    }

    protected virtual void OnSceneChanged(string sceneName)
    {
        if (isSceneBeingLoaded)
        {
            if (debugLogs) Debug.Log($"{BarName}: Ignorando notificação durante carregamento de cena");
            return;
        }

        if (debugLogs) Debug.Log($"{BarName}: Cena mudou para {sceneName}");
        currentScene = sceneName;
        OnSceneChangedSpecific(sceneName);
        UpdateBarState(currentScene);
    }
    
    protected virtual void OnSceneChangedSpecific(string sceneName) { }
    protected virtual void UpdateBarState(string sceneName)
    {
        currentScene = sceneName;
        UpdateButtonVisibility(sceneName);
        AdjustVisibilityForCurrentScene();
        EnsureBarIntegrity();
        
        if (debugLogs) Debug.Log($"{BarName}: Estado atualizado para cena {sceneName}");
    }

    protected virtual void UpdateButtonVisibility(string sceneName) { }
    protected virtual void AdjustVisibilityForCurrentScene()
    {
        bool shouldShowBar = !scenesWithoutBar.Contains(currentScene);
        
        if (debugLogs)
        {
            Debug.Log($"{BarName}: Ajustando visibilidade para cena {currentScene}");
            Debug.Log($"{BarName}: Deve mostrar barra = {shouldShowBar}");
            Debug.Log($"{BarName}: Estado atual = {gameObject.activeSelf}");
            Debug.Log($"{BarName}: Cenas sem barra: {string.Join(", ", scenesWithoutBar)}");
        }

        Transform barChild = transform.Find(BarChildName);
        gameObject.SetActive(shouldShowBar);

        if (barChild != null && shouldShowBar)
        {
            barChild.gameObject.SetActive(true);
            if (debugLogs) Debug.Log($"{BarName}: Visibilidade do filho {BarChildName} ajustada para {shouldShowBar}");
        }
        
        UpdateCanvasElements(shouldShowBar);
        
        if (debugLogs) Debug.Log($"{BarName}: Visibilidade ajustada para {shouldShowBar}");
    }
    
    protected virtual void UpdateCanvasElements(bool shouldShow)
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = shouldShow;
        }

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = shouldShow ? 1f : 0f;
            canvasGroup.interactable = shouldShow;
            canvasGroup.blocksRaycasts = shouldShow;
        }
    }

    protected virtual void EnsureBarIntegrity()
    {
        if (!gameObject.activeSelf) return;
        
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && !canvas.enabled)
        {
            canvas.enabled = true;
            if (debugLogs) Debug.Log($"{BarName}: Corrigido Canvas desativado");
        }

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
                Debug.Log($"{BarName}: Corrigidas propriedades do CanvasGroup");
        }

        Transform barChild = transform.Find(BarChildName);
        if (barChild != null && !barChild.gameObject.activeSelf)
        {
            barChild.gameObject.SetActive(true);
            if (debugLogs) Debug.Log($"{BarName}: Ativado filho {BarChildName} que estava inativo");
        }
    }

    public virtual void AddSceneWithoutBar(string sceneName)
    {
        if (!scenesWithoutBar.Contains(sceneName))
        {
            scenesWithoutBar.Add(sceneName);
            
            // Se for a cena atual, atualizar visibilidade
            if (currentScene == sceneName)
            {
                AdjustVisibilityForCurrentScene();
            }
            
            if (debugLogs) Debug.Log($"{BarName}: Adicionada cena '{sceneName}' à lista de exclusão");
        }
    }

    public virtual void RemoveSceneWithoutBar(string sceneName)
    {
        if (scenesWithoutBar.Contains(sceneName))
        {
            scenesWithoutBar.Remove(sceneName);
            
            if (currentScene == sceneName)
            {
                AdjustVisibilityForCurrentScene();
            }
            
            if (debugLogs) Debug.Log($"{BarName}: Removida cena '{sceneName}' da lista de exclusão");
        }
    }
    
    public virtual void ForceVisibilityCheck()
    {
        currentScene = SceneManager.GetActiveScene().name;
        AdjustVisibilityForCurrentScene();
        
        if (debugLogs) Debug.Log($"{BarName}: Verificação de visibilidade forçada para cena {currentScene}");
    }
    
    public virtual void ForceRefreshState()
    {
        if (debugLogs) Debug.Log($"{BarName}: Forçando atualização do estado");

        string activeScene = SceneManager.GetActiveScene().name;
        UpdateBarState(activeScene);
        
        if (debugLogs) Debug.Log($"{BarName}: Atualização forçada concluída. Visibilidade: {gameObject.activeSelf}");
    }
}
