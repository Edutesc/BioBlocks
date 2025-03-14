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
    private HalfViewMenu halfViewMenu;

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

        if (debugLogs) Debug.Log("TopBarManager inicializado");
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
                if (currentScene == "ProfileScene")
                {
                    ActivateHalfViewMenu();
                }
            });
        }
    }

    private void ActivateHalfViewMenu()
    {
        if (halfViewMenu == null)
        {
            // Try to find it regardless of active state
            halfViewMenu = FindFirstObjectByType<HalfViewMenu>(FindObjectsInactive.Include);

            if (halfViewMenu == null)
            {
                Debug.LogWarning("HalfViewMenu não encontrado na cena ProfileScene!");
                return;
            }
        }

        halfViewMenu.gameObject.SetActive(true);
        halfViewMenu.ShowMenu();

        if (debugLogs) Debug.Log("HalfViewMenu ativado pela TopBar");
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
        UserDataStore.OnUserDataChanged += OnUserDataChanged;
        UpdateFromCurrentUserData();
    }

    private void OnDestroy()
    {
        if (NavigationManager.Instance != null)
        {
            NavigationManager.Instance.OnSceneChanged -= OnSceneChanged;
        }

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
        UpdateButtonVisibility(currentScene);
        AdjustVisibilityForCurrentScene();

        if (sceneName != "ProfileScene")
        {
            halfViewMenu = null;
        }
        else
        {
            StartCoroutine(FindHalfViewMenuAfterDelay());
        }
    }

    private System.Collections.IEnumerator FindHalfViewMenuAfterDelay()
    {
        yield return null;

        halfViewMenu = FindFirstObjectByType<HalfViewMenu>(FindObjectsInactive.Include);

        if (halfViewMenu != null)
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
        gameObject.SetActive(shouldShowTopBar);

        if (debugLogs) Debug.Log($"TopBar visibilidade na cena {currentScene}: {shouldShowTopBar}");
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
        Debug.Log("=== TopBarManager - Lista de Botões ===");

        Debug.Log($"hubButton: {hubButton.buttonName}");
        Debug.Log($"Cenas visíveis para hubButton: {string.Join(", ", hubButton.visibleInScenes)}");

        Debug.Log($"engineButton: {engineButton.buttonName}");
        Debug.Log($"Cenas visíveis para engineButton: {string.Join(", ", engineButton.visibleInScenes)}");

        Debug.Log("=====================================");
    }
}