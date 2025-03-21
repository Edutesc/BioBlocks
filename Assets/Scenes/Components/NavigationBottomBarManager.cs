using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NavigationBottomBarManager : BarsManager
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

    [Header("Persistência")]
    [SerializeField]
    private List<string> scenesWithoutBottomBar = new List<string>()
    {
        "LoginView",
        "RegisterView",
        "QuestionScene",
        "ResetDatabaseView"
    };

    private List<NavButton> allButtons = new List<NavButton>();
    private static NavigationBottomBarManager _instance;
    protected override string BarName => "PersistentBottomBar";
    protected override string BarChildName => "BottomBar";

    public static NavigationBottomBarManager Instance
    {
        get { return _instance; }
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
        base.scenesWithoutBar.Clear();
        foreach (var scene in scenesWithoutBottomBar)
        {
            base.scenesWithoutBar.Add(scene);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        allButtons.Clear();
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

    protected override void AdjustVisibilityForCurrentScene()
    {
        bool shouldShowBar = !scenesWithoutBar.Contains(currentScene);

        if (debugLogs)
        {
            Debug.Log($"[BottomBar Debug] Cena atual: '{currentScene}'");
            Debug.Log($"[BottomBar Debug] Cenas sem barra: {string.Join(", ", scenesWithoutBar)}");
            Debug.Log($"[BottomBar Debug] A cena atual está na lista? {scenesWithoutBar.Contains(currentScene)}");
            Debug.Log($"[BottomBar Debug] shouldShowBar = {shouldShowBar}");
        }

        Transform barChild = transform.Find(BarChildName);

        if (!shouldShowBar)
        {
            if (barChild != null)
            {
                barChild.gameObject.SetActive(false);
                if (debugLogs) Debug.Log($"[BottomBar Debug] Desativando {BarChildName} na cena {currentScene}");
            }
            else
            {
                Debug.LogWarning($"[BottomBar Debug] Filho '{BarChildName}' não encontrado!");
            }

            UpdateCanvasElements(false);

            if (debugLogs) Debug.Log($"[BottomBar Debug] Forçando desativação da BottomBar na cena {currentScene}");
        }
        else
        {
            if (barChild != null)
            {
                barChild.gameObject.SetActive(true);
                if (debugLogs) Debug.Log($"[BottomBar Debug] Ativando {BarChildName} na cena {currentScene}");
            }
            else
            {
                Debug.LogWarning($"[BottomBar Debug] Filho '{BarChildName}' não encontrado!");
            }

            UpdateCanvasElements(true);
        }

        if (debugLogs) Debug.Log($"BottomBar visibilidade na cena {currentScene}: {shouldShowBar}");
    }

    protected override void UpdateButtonVisibility(string sceneName)
    {
        UpdateButtonDisplay(sceneName);
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
        AddSceneWithoutBar(sceneName);
    }

    public void RemoveSceneWithoutBottomBar(string sceneName)
    {
        RemoveSceneWithoutBar(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        bool shouldShow = !scenesWithoutBar.Contains(sceneName);
        SetBarVisibility(shouldShow);

        if (shouldShow && gameObject.activeSelf)
        {
            UpdateButtonDisplay(sceneName);
        }

        Debug.Log($"{BarName}: Visibilidade após carregamento de cena: {gameObject.activeSelf}");
    }

    protected override void OnCleanup()
    {
        base.OnCleanup();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}