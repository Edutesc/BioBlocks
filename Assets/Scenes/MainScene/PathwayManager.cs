using UnityEngine;
using System.Collections.Generic;
using TMPro;

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
            FirestoreRepository.Instance.ListenToUserScore(UserDataStore.CurrentUserData.UserId, UpdateScoreUI);
            UserDataStore.OnUserDataChanged += OnUserDataChanged;
            
            // Inscreve no evento de atualização de questões respondidas
            AnsweredQuestionsManager.OnAnsweredQuestionsUpdated += HandleAnsweredQuestionsUpdated;
            
            UpdateAnsweredQuestionsPercentages();
        }
        else
        {
            Debug.LogError("User data not loaded. Redirecting to Login.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        }
    }

    private void OnDestroy()
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
        // Remove a inscrição do evento quando o objeto for destruído
        AnsweredQuestionsManager.OnAnsweredQuestionsUpdated -= HandleAnsweredQuestionsUpdated;
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
            int totalQuestions = 50;
            int percentageAnswered = (count * 100) / totalQuestions;

            string objectName = $"{databankName}PorcentageText";
            GameObject textObject = GameObject.Find(objectName);

            if (textObject != null)
            {
                TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
                if (tmpText != null)
                {
                    tmpText.text = $"{percentageAnswered}%";
                    Debug.Log($"{databankName}PorcentageText updated to {percentageAnswered}%");
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