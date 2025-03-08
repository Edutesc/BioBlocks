using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class ProfileManager : MonoBehaviour
{
    [Header("UserData UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text emailText;
    [SerializeField] private TMP_Text createdTimeText;
    [SerializeField] private CanvasGroup userDataTable;

    [Header("Delete Account")]
    [SerializeField] private Button deleteAccountButton;
    [SerializeField] private TextMeshProUGUI deleteAccountButtonText;
    [SerializeField] private DeleteAccountPanel deleteAccountPanel;
    private bool firestoreDeleted = false;
    private bool storageDeleted = false;

    [Header("Configurações de Overlay")]
    [SerializeField] private GameObject deleteAccountDarkOverlay;
    [SerializeField] private float overlayAlpha = 0.6f;

    [Header("ReAuthentication")]
    [SerializeField] private ReAuthenticationUI reAuthUI;

    private UserData currentUserData;

    private void Start()
    {
        if (deleteAccountPanel == null)
        {
            Debug.LogError("DeleteAccountPanel não está atribuído no ProfileManager!");
        }
        else
        {
            Debug.Log("DeleteAccountPanel encontrado no ProfileManager");
        }
        if (deleteAccountDarkOverlay != null)
        {
            deleteAccountDarkOverlay.SetActive(false);
        }

        InitializeAccountManager();
    }

    private void InitializeAccountManager()
    {
        currentUserData = UserDataStore.CurrentUserData;
        Debug.Log($"CurrentUserData: {(currentUserData != null ? "Loaded" : "Null")}");

        if (currentUserData != null)
        {
            UpdateUI();
            FirestoreRepository.Instance.ListenToUserData(
                currentUserData.UserId,
                (newScore) =>
                {
                    // Atualizar o score na UI
                    if (scoreText != null)
                    {
                        scoreText.text = $"{newScore} XP";
                    }
                },
                (answeredQuestions) =>
                {
                    Debug.Log("ProfileManager: Recebeu atualização de questões respondidas via listener");

                    if (DatabaseStatisticsManager.Instance.IsInitialized)
                    {
                        DisplayAnsweredQuestionsCount();
                    }
                }
            );

            if (DatabaseStatisticsManager.Instance.IsInitialized)
            {
                DisplayAnsweredQuestionsCount();
            }
            else
            {
                DatabaseStatisticsManager.OnStatisticsReady += OnDatabaseStatisticsReady;
                StartCoroutine(InitializeDatabaseStatistics());
            }
        }
        else
        {
            Debug.LogError("User data not loaded. Redirecting to Login.");
        }
    }

    private IEnumerator InitializeDatabaseStatistics()
    {
        yield return null;

        Debug.Log("ProfileManager iniciando inicialização das estatísticas");
        var task = DatabaseStatisticsManager.Instance.Initialize();

        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            Debug.LogError($"Erro ao inicializar estatísticas: {task.Exception}");
        }
    }

    private void OnDatabaseStatisticsReady()
    {
        Debug.Log("ProfileManager: Estatísticas prontas, exibindo contagem de questões");
        DisplayAnsweredQuestionsCount();
        DatabaseStatisticsManager.OnStatisticsReady -= OnDatabaseStatisticsReady;
    }

    private void OnDisable()
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
        AnsweredQuestionsManager.OnAnsweredQuestionsUpdated -= HandleAnsweredQuestionsUpdated;
        DatabaseStatisticsManager.OnStatisticsReady -= OnDatabaseStatisticsReady;

        GameObject darkOverlay = GameObject.Find("DarkOverlay");
        if (darkOverlay != null)
        {
            darkOverlay.SetActive(false);
            Debug.Log("DarkOverlay desativado via OnDisable");
        }

    }

    private void OnUserDataChanged(UserData userData)
    {
        currentUserData = userData;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentUserData == null)
        {
            Debug.LogError("Tentando atualizar UI com userData null");
            return;
        }

        nickNameText.text = currentUserData.NickName;
        scoreText.text = $"{currentUserData.Score} XP";
        nameText.text = currentUserData.Name;
        emailText.text = currentUserData.Email;
        createdTimeText.text = $"Conta criada em {currentUserData.GetFormattedCreatedTime()}";
    }

    public void Navigate(string sceneName)
    {
        Debug.Log($"Navigating to {sceneName}");
        NavigationManager.Instance.NavigateTo(sceneName);
    }

    public void LogoutButton()
    {
        StartCoroutine(LogoutAsync().AsCoroutine());
    }

    public async Task LogoutAsync()
    {
        try
        {
            string currentUserId = UserDataStore.CurrentUserData?.UserId;
            UserDataStore.CurrentUserData = null;
            AnsweredQuestionsManager.Instance.ResetManager();
            if (!string.IsNullOrEmpty(currentUserId))
            {
                AnsweredQuestionsListStore.ClearUserAnsweredQuestions(currentUserId);
            }

            await AuthenticationRepository.Instance.LogoutAsync();
            Debug.Log("Logout realizado com sucesso");

            Navigate("LoginView");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro ao realizar logout: {ex.Message}");
            throw;
        }
    }

    public void StartDeleteAccount()
    {
        Debug.Log("StartDeleteAccount chamado");

        if (deleteAccountPanel == null)
        {
            Debug.LogError("DeleteAccountPanel é null!");
            return;
        }

        // Verificar primeiro se o DarkOverlay já está ativo
        bool overlayWasActive = false;
        if (deleteAccountDarkOverlay != null)
        {
            overlayWasActive = deleteAccountDarkOverlay.activeSelf;

            // Ativar apenas se ainda não estiver ativo
            if (!overlayWasActive)
            {
                deleteAccountDarkOverlay.SetActive(true);
                Debug.Log("DarkOverlay ativado para confirmação de exclusão");
            }
            else
            {
                Debug.Log("DarkOverlay já estava ativo, mantendo-o");
            }

            // Ajustar o sorting order em qualquer caso
            Canvas overlayCanvas = deleteAccountDarkOverlay.GetComponent<Canvas>();
            if (overlayCanvas != null)
            {
                overlayCanvas.sortingOrder = 109; // Logo abaixo do DeleteAccountCanvas
            }
        }

        userDataTable.alpha = 0;
        userDataTable.interactable = false;
        userDataTable.blocksRaycasts = false;

        deleteAccountPanel.ShowPanel();
    }

    public void CancelDeleteAccount()
    {
        Debug.Log("CancelDeleteAccount chamado");
        if (deleteAccountPanel == null)
        {
            Debug.LogError("DeleteAccountPanel é null!");
            return;
        }

        // Desativar os overlays
        if (deleteAccountDarkOverlay != null)
        {
            deleteAccountDarkOverlay.SetActive(false);
        }

        GameObject halfViewOverlay = GameObject.Find("HalfViewDarkOverlay");
        if (halfViewOverlay != null)
        {
            halfViewOverlay.SetActive(false);
        }

        // NOVO: Reabilitar interações com os elementos da cena
        ReenableSceneInteractions();

        // Restore visibility
        userDataTable.alpha = 1;
        userDataTable.interactable = true;
        userDataTable.blocksRaycasts = true;

        deleteAccountPanel.HidePanel();
    }

    private void ReenableSceneInteractions()
    {
        // Encontra todos os elementos interativos da cena
        Selectable[] selectables = FindObjectsOfType<Selectable>(true);
        foreach (Selectable selectable in selectables)
        {
            // Reativa todos os elementos (exceto os que devem ficar desativados por design)
            if (!ShouldStayDisabled(selectable.gameObject))
            {
                selectable.interactable = true;
            }
        }

        Debug.Log("Todas as interações da cena reabilitadas");
    }

    private bool ShouldStayDisabled(GameObject obj)
    {
        // Aqui você pode adicionar lógica para elementos que devem permanecer desativados
        // Por exemplo, botões que só devem estar ativos em certas condições

        // Por padrão, todos os elementos são reabilitados
        return false;
    }

    public void DeleteAccountButton()
    {
        StartCoroutine(DeleteAccountAsync().AsCoroutine());
    }

    public async Task DeleteAccountAsync(bool isRetry = false)
    {
        Debug.Log($"Starting DeleteAccountAsync... (isRetry: {isRetry})");
        if (currentUserData.UserId == null)
        {
            Debug.LogError("The user is not connected.");
            return;
        }

        string userId = currentUserData.UserId;

        try
        {
            if (deleteAccountButton != null) deleteAccountButton.interactable = false;
            if (deleteAccountButtonText != null) deleteAccountButtonText.text = "Verificando...";

            if (!isRetry)
            {
                // 1. Deletar documento do Firestore
                if (!firestoreDeleted)
                {
                    Debug.Log("Iniciando deleção do documento do Firestore...");
                    try
                    {
                        await DeleteUserDocumentAsync(userId);
                        Debug.Log("Documento do Firestore deletado com sucesso");
                        firestoreDeleted = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Erro ao deletar Firestore: {ex.Message}");
                    }
                }

                // 2. Deletar imagem do Storage
                if (!storageDeleted && !string.IsNullOrEmpty(currentUserData.ProfileImageUrl))
                {
                    Debug.Log("Deletando imagem do Storage...");
                    try
                    {
                        await StorageRepository.Instance.DeleteProfileImageAsync(currentUserData.ProfileImageUrl);
                        Debug.Log("Imagem do Storage deletada com sucesso");
                        storageDeleted = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Erro ao deletar Storage: {ex.Message}");
                    }
                }
            }

            // 3. Tentar deletar o usuário do Authentication
            Debug.Log("Deletando usuário do Authentication...");
            try
            {
                await AuthenticationRepository.Instance.DeleteUser(userId);
                Debug.Log("Usuário deletado do Authentication com sucesso");

                // Sucesso total
                if (deleteAccountButtonText != null) deleteAccountButtonText.text = "Até a próxima!";
                deleteAccountPanel.HidePanel();

                if (deleteAccountButtonText != null) deleteAccountButtonText.text = "Até a próxima!";
                deleteAccountPanel.HidePanel();

                // Desativar overlays
                if (deleteAccountDarkOverlay != null)
                {
                    deleteAccountDarkOverlay.SetActive(false);
                }

                GameObject halfViewOverlay = GameObject.Find("HalfViewDarkOverlay");
                if (halfViewOverlay != null)
                {
                    halfViewOverlay.SetActive(false);
                }
                
                ReenableSceneInteractions();

                Navigate("LoginView");
            }
            catch (ReauthenticationRequiredException)
            {
                Debug.Log("Reautenticação necessária para deletar usuário");

                if (deleteAccountButton != null)
                {
                    deleteAccountButton.interactable = true;
                    deleteAccountButtonText.text = "Deletar Conta";
                }

                // Mostrar painel de reautenticação
                deleteAccountPanel.HidePanel();
                Debug.Log($"Mostrando painel de reautenticação para email: {currentUserData.Email}");
                reAuthUI.ShowReAuthPanel(currentUserData.Email, async () =>
                {
                    Debug.Log("Reautenticação bem-sucedida, tentando deletar apenas Authentication...");
                    await DeleteAccountAsync(true);
                });
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao deletar conta: {ex.Message}");
            if (deleteAccountButton != null)
            {
                deleteAccountButton.interactable = true;
                deleteAccountButtonText.text = "Novamente";
            }
        }
    }

    private async Task DeleteUserDocumentAsync(string userId)
    {
        try
        {
            await FirestoreRepository.Instance.DeleteDocument("Users", userId);
            Debug.Log($"Documento do usuário {userId} deletado com sucesso");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro ao deletar documento do usuário {userId}: {ex.Message}");
            throw;
        }
    }

    private void DisplayAnsweredQuestionsCount()
    {
        if (currentUserData == null || string.IsNullOrEmpty(currentUserData.UserId))
        {
            Debug.LogError("Usuário não encontrado ou ID inválido");
            return;
        }

        var userAnsweredQuestions = AnsweredQuestionsListStore.GetAnsweredQuestionsCountForUser(currentUserData.UserId);

        // Lista de todos os bancos de dados possíveis
        string[] allDatabases = new string[]
        {
        "AcidBaseBufferQuestionDatabase",
        "AminoacidQuestionDatabase",
        "BiochemistryIntroductionQuestionDatabase",
        "CarbohydratesQuestionDatabase",
        "EnzymeQuestionDatabase",
        "LipidsQuestionDatabase",
        "MembranesQuestionDatabase",
        "NucleicAcidsQuestionDatabase",
        "ProteinQuestionDatabase",
        "WaterQuestionDatabase"
        };

        // Atualizar todos os bancos de dados
        foreach (string databankName in allDatabases)
        {
            int answeredCount = userAnsweredQuestions.ContainsKey(databankName) ? userAnsweredQuestions[databankName] : 0;

            // Obter o número total de questões de forma dinâmica
            int totalQuestions = QuestionBankStatistics.GetTotalQuestions(databankName);
            if (totalQuestions <= 0) totalQuestions = 50; // Valor padrão se não houver estatísticas

            string objectName = $"{databankName}CountText";
            GameObject textObject = GameObject.Find(objectName);

            if (textObject == null)
            {
                Debug.LogWarning($"Não foi possível encontrar o GameObject: {objectName}");
                continue;
            }

            TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
            if (tmpText == null)
            {
                Debug.LogWarning($"Componente TextMeshProUGUI não encontrado no GameObject: {objectName}");
                continue;
            }

            // Exibir no formato X/Y para melhor compreensão
            tmpText.text = $"{answeredCount}/{totalQuestions}";
            Debug.Log($"Usuário {currentUserData.UserId} - Banco de Dados: {databankName}, Questões Respondidas: {answeredCount}/{totalQuestions}");
        }
    }

    private void OnEnable()
    {
        UserDataStore.OnUserDataChanged += OnUserDataChanged;
        AnsweredQuestionsManager.OnAnsweredQuestionsUpdated += HandleAnsweredQuestionsUpdated;
    }

    private void HandleAnsweredQuestionsUpdated(Dictionary<string, int> answeredCounts)
    {
        if (this == null) return; // Proteção contra chamadas após destruição do objeto

        Debug.Log("ProfileManager: Recebeu atualização do AnsweredQuestionsManager");

        DisplayAnsweredQuestionsCount();
    }

}