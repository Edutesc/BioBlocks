using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class PathwayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text nameText;

    private void Start()
    {
#if DEBUG
        Debug.Log($"PathwayManager initialized in {BioBlocksSettings.ENVIRONMENT} mode");
#endif

        if (UserDataStore.CurrentUserData != null)
        {
            UpdateUI(UserDataStore.CurrentUserData);
            FirestoreRepository.Instance.ListenToUserData(UserDataStore.CurrentUserData.UserId, UpdateScoreUI);
            UserDataStore.OnUserDataChanged += OnUserDataChanged;

            // Inscreve no evento de atualização de questões respondidas
            AnsweredQuestionsManager.OnAnsweredQuestionsUpdated += HandleAnsweredQuestionsUpdated;

            // Verifica se as estatísticas estão disponíveis
            if (DatabaseStatisticsManager.Instance.IsInitialized)
            {
                // Se já estiver inicializado, atualiza imediatamente
                UpdateAnsweredQuestionsPercentages();
            }
            else
            {
                // Se não estiver inicializado, registra para o evento e inicializa
                DatabaseStatisticsManager.OnStatisticsReady += OnDatabaseStatisticsReady;

                // Inicia a inicialização se ainda não foi iniciada
                StartCoroutine(InitializeDatabaseStatistics());
            }
        }
        else
        {
            Debug.LogError("User data not loaded. Redirecting to Login.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        }
    }

    private IEnumerator InitializeDatabaseStatistics()
    {
        // Espera um frame para garantir que outros componentes estejam prontos
        yield return null;

        Debug.Log("PathwayManager iniciando inicialização das estatísticas");
        var task = DatabaseStatisticsManager.Instance.Initialize();

        // Aguarda a conclusão da inicialização
        while (!task.IsCompleted)
        {
            yield return null;
        }

        // Se ocorreu um erro, registra
        if (task.IsFaulted)
        {
            Debug.LogError($"Erro ao inicializar estatísticas: {task.Exception}");
        }
    }

    private void OnDatabaseStatisticsReady()
    {
        // Este método é chamado quando as estatísticas estão prontas
        Debug.Log("PathwayManager: Estatísticas prontas, atualizando porcentagens");
        UpdateAnsweredQuestionsPercentages();

        // Cancela o registro do evento após o uso
        DatabaseStatisticsManager.OnStatisticsReady -= OnDatabaseStatisticsReady;
    }

    private void OnDestroy()
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
        AnsweredQuestionsManager.OnAnsweredQuestionsUpdated -= HandleAnsweredQuestionsUpdated;
        DatabaseStatisticsManager.OnStatisticsReady -= OnDatabaseStatisticsReady;
    }

    // Método atualizado para corresponder ao delegate
    private void HandleAnsweredQuestionsUpdated(Dictionary<string, int> answeredCounts)
    {
        if (this == null) return; // Proteção contra chamadas após destruição do objeto

        Debug.Log("Received update from AnsweredQuestionsManager");

        // Atualiza o AnsweredQuestionsListStore com os novos valores
        if (UserDataStore.CurrentUserData != null)
        {
            string userId = UserDataStore.CurrentUserData.UserId;
            foreach (var kvp in answeredCounts)
            {
                AnsweredQuestionsListStore.UpdateAnsweredQuestionsCount(userId, kvp.Key, kvp.Value);
            }
        }

        // Atualiza a UI
        UpdateAnsweredQuestionsPercentages();
    }

    private void OnUserDataChanged(UserData userData)
    {
        UpdateUI(userData);
    }

    private void UpdateUI(UserData userData)
    {
#if DEBUG
        Debug.Log($"Updating UI for user: {userData.NickName}");
#endif
        nameText.text = $"{userData.NickName}";
        scoreText.text = $"{userData.Score}";
    }

    private void UpdateScoreUI(int newScore)
    {
        scoreText.text = $"{newScore} XP";
        if (UserDataStore.CurrentUserData != null)
        {
            UserDataStore.CurrentUserData.Score = newScore;
        }
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

            // Obter o número total de questões dinamicamente
            int totalQuestions = QuestionBankStatistics.GetTotalQuestions(databankName);
            if (totalQuestions <= 0) totalQuestions = 50; // Valor padrão se não houver estatísticas

            // Calcular a porcentagem com base no total real
            int percentageAnswered = totalQuestions > 0 ? (count * 100) / totalQuestions : 0;

            // Garantir que a porcentagem não exceda 100%
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
            }
        }
    }

    public void Navigate(string sceneName)
    {
        Debug.Log($"Navigating to {sceneName}");
        NavigationManager.Instance.NavigateTo(sceneName);
    }
}