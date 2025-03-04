using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Firebase;
using System;

public class InitializationManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject retryPanel;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image loadingSpinner;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Button retryButton;

    [Header("Configuration")]
    [SerializeField] private float minimumLoadingTime = 2.0f;
    [SerializeField] private float spinnerRotationSpeed = 100f;

    private void Awake()
    {
        // Garante que o BioBlocksSettings seja inicializado primeiro
        BioBlocksSettings.Instance.IsDebugMode();
#if DEBUG
        Debug.Log($"Bioblocks initialized in {BioBlocksSettings.ENVIRONMENT} mode");
#endif
    }

    private void Start()
    {
        SetupUI();
        StartInitialization();
    }

    private void SetupUI()
    {
        loadingPanel.SetActive(true);
        retryPanel.SetActive(false);
        progressBar.fillAmount = 0f;

        retryButton.onClick.AddListener(() =>
        {
            retryPanel.SetActive(false);
            loadingPanel.SetActive(true);
            StartInitialization();
        });
    }

    private async void StartInitialization()
    {
#if DEBUG
        Debug.Log($"Starting app initialization in {BioBlocksSettings.ENVIRONMENT} mode...");
        Debug.Log($"App Version: {BioBlocksSettings.VERSION}");
#endif
        Debug.Log("Starting app initialization...");
        float startTime = Time.time;

        try
        {
            // Inicializar Firebase e seus repositories
            UpdateStatus("Inicializando Firebase...");
            await InitializeFirebaseServices();
            UpdateProgress(0.3f);

            // Verificar autenticação
            UpdateStatus("Verificando autenticação...");
            bool isAuthenticated = await CheckAuthentication();
            UpdateProgress(0.5f);

            // Carregar dados do usuário se autenticado
            bool userDataLoaded = false;
            if (isAuthenticated)
            {
                UpdateStatus("Carregando dados do usuário...");
                userDataLoaded = await LoadUserData();
                UpdateProgress(0.7f);

                if (userDataLoaded)
                {
                    // Inicializar estatísticas dos bancos de dados
                    UpdateStatus("Carregando bancos de questões...");
                    await DatabaseStatisticsManager.Instance.Initialize();
                    UpdateProgress(0.9f);
                }
            }

            // Garantir tempo mínimo de loading
            float elapsed = Time.time - startTime;
            if (elapsed < minimumLoadingTime)
            {
                await Task.Delay(Mathf.RoundToInt((minimumLoadingTime - elapsed) * 1000));
            }

            if (isAuthenticated && userDataLoaded)
            {
                SceneManager.LoadScene("PathwayScene");
            }
            else
            {
                SceneManager.LoadScene("LoginView");
            }
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.LogError($"Detailed initialization error: {e.Message}\nStackTrace: {e.StackTrace}");
#else
            Debug.LogError("An initialization error occurred.");
#endif
            ShowError("Falha na inicialização. Por favor, verifique sua conexão e tente novamente.");
        }
    }

    private async Task InitializeFirebaseServices()
    {
        // Verificar dependências do Firebase
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus != DependencyStatus.Available)
        {
            throw new System.Exception($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }

        // Inicializar Authentication
        await AuthenticationRepository.Instance.InitializeAsync();
        Debug.Log("Firebase Authentication initialized successfully");

        // Inicializar Firestore
        FirestoreRepository.Instance.Initialize();
        Debug.Log("Firestore initialized successfully");

        // Inicializar Storage
        StorageRepository.Instance.Initialize();
        Debug.Log("Storage initialized successfully");
    }

    private async Task<bool> LoadUserData()
    {
        try
        {
            var user = AuthenticationRepository.Instance.Auth.CurrentUser;
            if (user != null)
            {
                Debug.Log($"Attempting to load data for user: {user.UserId}");
                var userData = await FirestoreRepository.Instance.GetUserData(user.UserId);
                if (userData == null)
                {
                    Debug.Log("User authenticated, but no data found. Redirecting to Login.");
                    return false;
                }
                else
                {
                    UserDataStore.CurrentUserData = userData;
                    Debug.Log($"User data loaded: Name: {userData.NickName}, Id: {userData.UserId}, Score: {userData.Score}");
                    return true;
                }
            }
            Debug.Log("No user signed in, cannot load user data");
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading user data: {e.Message}\nStackTrace: {e.StackTrace}");
            throw;
        }
    }

    private async Task<bool> CheckAuthentication()
    {
        var user = AuthenticationRepository.Instance.Auth.CurrentUser;
        if (user != null)
        {
            try
            {
                await user.ReloadAsync();
                Debug.Log($"User authenticated successfully. UserId: {user.UserId}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reloading user: {e.Message}");
                return false;
            }
        }
        Debug.Log("No user currently signed in");
        return false;
    }

    private void Update()
    {
        if (loadingSpinner != null && loadingPanel.activeSelf)
        {
            loadingSpinner.transform.Rotate(0f, 0f, -spinnerRotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = progress;
            Debug.Log($"progressBar was here: {progressBar}");
            Debug.Log($"progressBar.fillAmount was here: {progressBar.fillAmount}");
        }
    }

    private void ShowError(string message)
    {
        loadingPanel.SetActive(false);
        retryPanel.SetActive(true);
        errorText.text = message;
    }
}