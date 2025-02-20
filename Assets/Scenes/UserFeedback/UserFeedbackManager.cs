using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class UserFeedbackManager : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private Transform questionsContainer;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject submitButton;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject textQuestionPrefab;
    [SerializeField] private GameObject ratingQuestionPrefab;
    [SerializeField] private GameObject toggleQuestionPrefab;
    
    [Header("Navegação")]
    [SerializeField] private int questionsPerPage = 3;
    [SerializeField] private bool groupByCategory = true;
    
    private IFeedbackDatabase currentDatabase;
    private Dictionary<string, object> feedbackResults = new Dictionary<string, object>();
    private List<IFeedbackQuestionController> questionControllers = new List<IFeedbackQuestionController>();
    private int currentPageIndex = 0;
    private List<FeedbackQuestion> allQuestions = new List<FeedbackQuestion>();
    
    private void Start()
    {
        // Encontra o banco de dados de feedback na cena
        currentDatabase = FindFirstObjectByType<AppUsabilityFeedbackDatabase>();
        if (currentDatabase == null)
        {
            Debug.LogError("Nenhum banco de dados de feedback encontrado na cena!");
            return;
        }
        
        allQuestions = currentDatabase.GetQuestions();
        InstantiateQuestions();
        UpdateNavigationButtons();
    }
    
    private void InstantiateQuestions()
    {
        // Limpa qualquer questão anterior
        foreach (Transform child in questionsContainer)
        {
            Destroy(child.gameObject);
        }
        questionControllers.Clear();
        
        List<FeedbackQuestion> sortedQuestions = allQuestions;
        if (groupByCategory)
        {
            // Organiza as perguntas por categoria
            sortedQuestions.Sort((a, b) => string.Compare(a.category, b.category));
        }
        
        // Instancia as questões
        foreach (var question in sortedQuestions)
        {
            GameObject prefab = null;
            
            switch (question.feedbackAnswerType)
            {
                case FeedbackAnswerType.Text:
                    prefab = textQuestionPrefab;
                    break;
                case FeedbackAnswerType.Rating:
                    prefab = ratingQuestionPrefab;
                    break;
                case FeedbackAnswerType.Toggle:
                    prefab = toggleQuestionPrefab;
                    break;
            }
            
            if (prefab != null)
            {
                GameObject questionObj = Instantiate(prefab, questionsContainer);
                IFeedbackQuestionController controller = questionObj.GetComponent<IFeedbackQuestionController>();
                if (controller != null)
                {
                    controller.SetupQuestion(question);
                    questionControllers.Add(controller);
                }
            }
        }
        
        // Mostra apenas as questões da página atual
        UpdateQuestionsVisibility();
    }
    
    private void UpdateQuestionsVisibility()
    {
        int startIndex = currentPageIndex * questionsPerPage;
        
        for (int i = 0; i < questionControllers.Count; i++)
        {
            bool isVisible = (i >= startIndex && i < startIndex + questionsPerPage);
            questionControllers[i].SetVisible(isVisible);
        }
    }
    
    private void UpdateNavigationButtons()
    {
        prevButton.SetActive(currentPageIndex > 0);
        
        bool isLastPage = (currentPageIndex + 1) * questionsPerPage >= questionControllers.Count;
        nextButton.SetActive(!isLastPage);
        submitButton.SetActive(isLastPage);
    }
    
    public void NextPage()
    {
        if (ValidateCurrentPage())
        {
            CollectCurrentPageAnswers();
            currentPageIndex++;
            UpdateQuestionsVisibility();
            UpdateNavigationButtons();
        }
    }
    
    public void PreviousPage()
    {
        currentPageIndex--;
        UpdateQuestionsVisibility();
        UpdateNavigationButtons();
    }
    
    private bool ValidateCurrentPage()
    {
        int startIndex = currentPageIndex * questionsPerPage;
        int endIndex = Mathf.Min(startIndex + questionsPerPage, questionControllers.Count);
        
        for (int i = startIndex; i < endIndex; i++)
        {
            if (!questionControllers[i].Validate())
            {
                return false;
            }
        }
        
        return true;
    }
    
    private void CollectCurrentPageAnswers()
    {
        int startIndex = currentPageIndex * questionsPerPage;
        int endIndex = Mathf.Min(startIndex + questionsPerPage, questionControllers.Count);
        
        for (int i = startIndex; i < endIndex; i++)
        {
            var result = questionControllers[i].GetResult();
            feedbackResults[result.Key] = result.Value;
        }
    }
    
    public async void SubmitFeedback()
    {
        if (ValidateCurrentPage())
        {
            CollectCurrentPageAnswers();
            
            // Adiciona metadados
            feedbackResults["submissionDate"] = DateTime.UtcNow;
            feedbackResults["databaseName"] = currentDatabase.GetDatabaseName();
            
            try
            {
                FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
                await db.Collection("feedback").AddAsync(feedbackResults);
                Debug.Log("Feedback enviado com sucesso!");
                
                // Mostra tela de agradecimento ou volta para menu principal
                ShowThankYouScreen();
            }
            catch (Exception e)
            {
                Debug.LogError($"Erro ao enviar feedback: {e.Message}");
                ShowErrorScreen();
            }
        }
    }
    
    private void ShowThankYouScreen()
    {
        // Implementar lógica para mostrar tela de agradecimento
    }
    
    private void ShowErrorScreen()
    {
        // Implementar lógica para mostrar erro
    }
}
