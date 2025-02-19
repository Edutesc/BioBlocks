using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System;

public class UserFeedbackManager : MonoBehaviour
{
    [Header("UI Sections")]
    [SerializeField] private GameObject userProfileSection;
    [SerializeField] private GameObject appExperienceSection;
    [SerializeField] private GameObject learningImpactSection;
    [SerializeField] private GameObject engagementSection;
    [SerializeField] private GameObject suggestionsSection;

    [Header("Navigation")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button submitButton;

    private int currentSection = 0;
    private GameObject[] sections;
    private FeedbackData feedbackData;

    private void Start()
    {
        sections = new GameObject[] 
        {
            userProfileSection,
            appExperienceSection,
            learningImpactSection,
            engagementSection,
            suggestionsSection
        };

        feedbackData = new FeedbackData();
        SetupNavigation();
        ShowCurrentSection();
    }

    private void SetupNavigation()
    {
        nextButton.onClick.AddListener(NextSection);
        previousButton.onClick.AddListener(PreviousSection);
        submitButton.onClick.AddListener(SubmitFeedback);
        
        submitButton.gameObject.SetActive(false);
    }

    private void ShowCurrentSection()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].SetActive(i == currentSection);
        }

        previousButton.gameObject.SetActive(currentSection > 0);
        nextButton.gameObject.SetActive(currentSection < sections.Length - 1);
        submitButton.gameObject.SetActive(currentSection == sections.Length - 1);
    }

    private void NextSection()
    {
        if (currentSection < sections.Length - 1)
        {
            currentSection++;
            ShowCurrentSection();
        }
    }

    private void PreviousSection()
    {
        if (currentSection > 0)
        {
            currentSection--;
            ShowCurrentSection();
        }
    }

    private async void SubmitFeedback()
    {
        try
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            await db.Collection("feedback").AddAsync(feedbackData.ToDictionary());
            Debug.Log("Feedback submitted successfully!");
            // Mostrar mensagem de sucesso e voltar para a cena principal
        }
        catch (Exception e)
        {
            Debug.LogError($"Error submitting feedback: {e.Message}");
            // Mostrar mensagem de erro
        }
    }

    // Métodos para coletar respostas das diferentes seções
    public void SetUserProfileData(bool previousExperience, int familiarityLevel)
    {
        feedbackData.PreviousExperience = previousExperience;
        feedbackData.FamiliarityLevel = familiarityLevel;
    }

    public void SetAppExperienceData(int easeOfUse, int interfaceRating, string technicalIssues)
    {
        feedbackData.EaseOfUse = easeOfUse;
        feedbackData.InterfaceRating = interfaceRating;
        feedbackData.TechnicalIssues = technicalIssues;
    }

    // ... métodos similares para outras seções
}
