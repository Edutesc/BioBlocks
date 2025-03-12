using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class PathwayManager : MonoBehaviour
{
    private void Start()
    {
#if DEBUG
        Debug.Log($"PathwayManager initialized in {BioBlocksSettings.ENVIRONMENT} mode");
#endif

        if (UserDataStore.CurrentUserData != null)
        {
            FirestoreRepository.Instance.ListenToUserData(
                UserDataStore.CurrentUserData.UserId,
                null,
                null,
                null
            );

            UserDataStore.OnUserDataChanged += OnUserDataChanged;

            InitializeTopBar();

            AnsweredQuestionsManager.OnAnsweredQuestionsUpdated += HandleAnsweredQuestionsUpdated;

            if (DatabaseStatisticsManager.Instance.IsInitialized)
            {
                UpdateAnsweredQuestionsPercentages();
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        }
    }

    private void InitializeTopBar()
    {
        if (TopBarManager.Instance != null)
        {
            Debug.Log("Configurando TopBar na PathwayManager...");
            TopBarManager.Instance.AddSceneToButtonVisibility("HubButton", "ProfileScene");
            TopBarManager.Instance.AddSceneToButtonVisibility("EngineButton", "ProfileScene");
            TopBarManager.Instance.DebugListButtons();
            Debug.Log("TopBar configurada na PathwayManager");
        }
        else
        {
            Debug.LogWarning("TopBarManager não encontrado na cena!");
        }
    }

    private IEnumerator InitializeDatabaseStatistics()
    {
        yield return null;

        Debug.Log("PathwayManager iniciando inicialização das estatísticas");
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
        Debug.Log("PathwayManager: Estatísticas prontas, atualizando porcentagens");
        UpdateAnsweredQuestionsPercentages();
        DatabaseStatisticsManager.OnStatisticsReady -= OnDatabaseStatisticsReady;
    }

    private void OnDestroy()
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
        AnsweredQuestionsManager.OnAnsweredQuestionsUpdated -= HandleAnsweredQuestionsUpdated;
        DatabaseStatisticsManager.OnStatisticsReady -= OnDatabaseStatisticsReady;
    }

    private void HandleAnsweredQuestionsUpdated(Dictionary<string, int> answeredCounts)
    {
        if (this == null) return;

        Debug.Log("Received update from AnsweredQuestionsManager");

        if (UserDataStore.CurrentUserData != null)
        {
            string userId = UserDataStore.CurrentUserData.UserId;
            foreach (var kvp in answeredCounts)
            {
                AnsweredQuestionsListStore.UpdateAnsweredQuestionsCount(userId, kvp.Key, kvp.Value);
            }
        }

        UpdateAnsweredQuestionsPercentages();
    }

    private void OnUserDataChanged(UserData userData)
    {
        // Pode incluir aqui qualquer evento a partir da atualização dos dados do usuário
    }

    private void UpdateAnsweredQuestionsPercentages()
    {
        if (UserDataStore.CurrentUserData == null) return;

        string userId = UserDataStore.CurrentUserData.UserId;
        var userCounts = AnsweredQuestionsListStore.GetAnsweredQuestionsCountForUser(userId);

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

        foreach (string databankName in allDatabases)
        {
            int count = userCounts.ContainsKey(databankName) ? userCounts[databankName] : 0;
            int totalQuestions = QuestionBankStatistics.GetTotalQuestions(databankName);
            if (totalQuestions <= 0) totalQuestions = 50;

            int percentageAnswered = totalQuestions > 0 ? (count * 100) / totalQuestions : 0;

            percentageAnswered = Mathf.Min(percentageAnswered, 100);

            string objectName = $"{databankName}PorcentageText";
            GameObject textObject = GameObject.Find(objectName);

            if (textObject != null)
            {
                TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
                if (tmpText != null)
                {
                    tmpText.text = $"{percentageAnswered}%";
                    Debug.Log($"{databankName}PorcentageText updated to {percentageAnswered}% ({count}/{totalQuestions})");
                }

                CircularProgressIndicator progressIndicator = textObject.GetComponentInParent<CircularProgressIndicator>();
                if (progressIndicator != null)
                {
                    progressIndicator.SetProgress(percentageAnswered);
                }
            }
        }
    }

    public void Navigate(string sceneName)
    {
        Debug.Log($"Navigating to {sceneName}");
        NavigationManager.Instance.NavigateTo(sceneName);
    }
}