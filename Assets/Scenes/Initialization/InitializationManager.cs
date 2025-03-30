using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Firebase;

public class InitializationManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject retryPanel;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text errorText;

    [Header("Configuration")]
    [SerializeField] private float minimumLoadingTime = 2.0f;

    [Header("Global Loading Spinner")]
    [SerializeField] private GameObject globalSpinnerPrefab;

    private LoadingSpinnerComponent globalSpinner;

    private void Awake()
    {
        BioBlocksSettings.Instance.IsDebugMode();
#if DEBUG
        Debug.Log($"Bioblocks initialized in {BioBlocksSettings.ENVIRONMENT} mode");
#endif

        InitializeGlobalSpinner();
    }

    private void InitializeGlobalSpinner()
    {
        try
        {
            // If we already have a spinner instance, use that one
            globalSpinner = LoadingSpinnerComponent.Instance;
            
            // If we're supposed to use a prefab instead of the singleton instance
            if (globalSpinner == null && globalSpinnerPrefab != null)
            {
                // Create a parent canvas if needed
                Canvas mainCanvas = FindObjectOfType<Canvas>();
                GameObject spinnerObject;
                
                if (mainCanvas != null)
                {
                    spinnerObject = Instantiate(globalSpinnerPrefab, mainCanvas.transform);
                }
                else
                {
                    spinnerObject = Instantiate(globalSpinnerPrefab);
                }
                
                spinnerObject.name = "GlobalLoadingSpinner";
                DontDestroyOnLoad(spinnerObject);
                
                globalSpinner = spinnerObject.GetComponent<LoadingSpinnerComponent>();
            }
        }
        catch (Exception e)
        {
            // Log error but don't fail initialization
            Debug.LogError($"Error initializing spinner: {e.Message}");
        }
    }

    private void Start()
    {
        SetupUI();
        StartInitialization();
    }

    private void SetupUI()
    {
        if (retryPanel != null)
            retryPanel.SetActive(false);
            
        if (progressBar != null)
            progressBar.fillAmount = 0f;
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
            // Show spinner safely using a try-catch block to avoid errors
            try
            {
                if (globalSpinner != null)
                {
                    globalSpinner.ShowSpinner();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error showing spinner: {e.Message}");
            }

            UpdateStatus("Inicializando Firebase...");
            await InitializeFirebaseServices();
            UpdateProgress(0.3f);

            UpdateStatus("Verificando autenticação...");
            bool isAuthenticated = await CheckAuthentication();
            UpdateProgress(0.5f);

            bool userDataLoaded = false;
            if (isAuthenticated)
            {
                UpdateStatus("Carregando dados do usuário...");
                userDataLoaded = await LoadUserData();
                UpdateProgress(0.7f);

                if (userDataLoaded)
                {
                    UpdateStatus("Carregando bancos de questões...");
                    await DatabaseStatisticsManager.Instance.Initialize();
                    UpdateProgress(0.9f);
                }
            }

            float elapsed = Time.time - startTime;
            if (elapsed < minimumLoadingTime)
            {
                await Task.Delay(Mathf.RoundToInt((minimumLoadingTime - elapsed) * 1000));
            }

            // Use a try-catch to handle spinner operations safely
            try
            {
                if (isAuthenticated && userDataLoaded)
                {
                    if (globalSpinner != null)
                    {
                        globalSpinner.ShowSpinnerUntilSceneLoaded("PathwayScene");
                    }
                    SceneManager.LoadScene("PathwayScene");
                }
                else
                {
                    if (globalSpinner != null)
                    {
                        globalSpinner.ShowSpinnerUntilSceneLoaded("LoginView");
                    }
                    SceneManager.LoadScene("LoginView");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error with spinner during scene transition: {e.Message}");
                // Continue with scene loading even if spinner fails
                if (isAuthenticated && userDataLoaded)
                {
                    SceneManager.LoadScene("PathwayScene");
                }
                else
                {
                    SceneManager.LoadScene("LoginView");
                }
            }
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.LogError($"Detailed initialization error: {e.Message}\nStackTrace: {e.StackTrace}");
#else
            Debug.LogError("An initialization error occurred.");
#endif
            // Hide spinner safely
            try
            {
                if (globalSpinner != null)
                {
                    globalSpinner.HideSpinner();
                }
            }
            catch { }
            
            ShowError("Falha na inicialização. Por favor, verifique sua conexão e tente novamente.");
        }
    }

    private async Task InitializeFirebaseServices()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus != DependencyStatus.Available)
        {
            throw new System.Exception($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }

        await AuthenticationRepository.Instance.InitializeAsync();
        FirestoreRepository.Instance.Initialize();
        StorageRepository.Instance.Initialize();
    }

    private async Task<bool> LoadUserData()
    {
        try
        {
            var user = AuthenticationRepository.Instance.Auth.CurrentUser;
            if (user != null)
            {
                var userData = await FirestoreRepository.Instance.GetUserData(user.UserId);
                if (userData == null)
                {
                    return false;
                }
                else
                {
                    UserDataStore.CurrentUserData = userData;
                    return true;
                }
            }

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
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reloading user: {e.Message}");
                return false;
            }
        }

        return false;
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
        }
    }

    private void ShowError(string message)
    {
        if (retryPanel != null)
        {
            retryPanel.SetActive(true);
            
            if (errorText != null)
            {
                errorText.text = message;
            }
        }
    }
}