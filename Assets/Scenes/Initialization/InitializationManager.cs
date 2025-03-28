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
    if (globalSpinnerPrefab != null)
    {
        // Instanciar o spinner como filho do Canvas principal para garantir a renderização correta
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
        
        // Verificar se o spinner tem um Canvas
        Canvas spinnerCanvas = spinnerObject.GetComponent<Canvas>();
        if (spinnerCanvas != null)
        {
            // Garantir que está no modo ScreenSpaceOverlay
            spinnerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // Garantir que está em uma ordem de sorting alta
            spinnerCanvas.sortingOrder = 32767; // Valor máximo
        }
    }
    else
    {
        globalSpinner = LoadingSpinnerComponent.Instance;
    }
    
    if (globalSpinner != null)
    {
        // Chamar o método de verificação e correção
        globalSpinner.VerifyAndFixSpinnerVisibility();
    }
}

    private void Start()
    {
        SetupUI();
        StartInitialization();
    }

    private void SetupUI()
    {
        retryPanel.SetActive(false);
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

            if (isAuthenticated && userDataLoaded)
            {
                globalSpinner.ShowSpinnerUntilSceneLoaded("PathwayScene");
                SceneManager.LoadScene("PathwayScene");
            }
            else
            {
                globalSpinner.ShowSpinnerUntilSceneLoaded("LoginView");
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
            globalSpinner.HideSpinner();
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
        retryPanel.SetActive(true);
        errorText.text = message;
    }
}