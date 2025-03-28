using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuestionSystem;
using System;
using System.Linq;

public class QuestionLoadManager : MonoBehaviour
{
    private List<Question> questions;
    public string databankName;
    private bool isInitialized = false;
    public string DatabankName => databankName;

    private async void Start()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        if (isInitialized) return;

        try
        {
            await WaitForAnsweredQuestionsManager();
            isInitialized = true;
            Debug.Log("QuestionLoadManager inicializado com sucesso");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao inicializar QuestionLoadManager: {e.Message}");
        }
    }

    private async Task WaitForAnsweredQuestionsManager()
    {
        int maxAttempts = 10;
        int currentAttempt = 0;
        
        while (currentAttempt < maxAttempts)
        {
            if (AnsweredQuestionsManager.Instance != null && 
                AnsweredQuestionsManager.Instance.IsManagerInitialized)
            {
                Debug.Log("AnsweredQuestionsManager encontrado e inicializado");
                return;
            }

            Debug.Log($"Tentativa {currentAttempt + 1} de {maxAttempts} para encontrar AnsweredQuestionsManager inicializado");
            await Task.Delay(500);
            currentAttempt++;
        }

        throw new Exception("AnsweredQuestionsManager não foi inicializado após várias tentativas");
    }

    public async Task<List<Question>> LoadQuestionsForSet(QuestionSet targetSet)
    {
        try
        {
            if (!isInitialized)
            {
                await Initialize();
            }

            Debug.Log($"Tentando carregar questões para o conjunto: {targetSet}");
            
            IQuestionDatabase database = FindQuestionDatabase(targetSet);
            
            if (database == null)
            {
                Debug.LogError($"Nenhum database encontrado para o QuestionSet: {targetSet}");
                return new List<Question>();
            }

            return await LoadQuestionsFromDatabase(database);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro em LoadQuestionsForSet: {e.Message}\nStackTrace: {e.StackTrace}");
            return new List<Question>();
        }
    }

    private IQuestionDatabase FindQuestionDatabase(QuestionSet targetSet)
    {
        try
        {
            MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            
            foreach (MonoBehaviour behaviour in allBehaviours)
            {
                if (behaviour is IQuestionDatabase database)
                {
                    Debug.Log($"Encontrado database do tipo: {database.GetType().Name}");
                    if (database.GetQuestionSetType() == targetSet)
                    {
                        return database;
                    }
                }
            }

            return null;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao procurar database: {e.Message}");
            return null;
        }
    }

    private async Task<List<Question>> LoadQuestionsFromDatabase(IQuestionDatabase database)
    {
        if (database == null)
        {
            Debug.LogError("Database é null em LoadQuestionsFromDatabase");
            return new List<Question>();
        }

        try
        {
            if (!AnsweredQuestionsManager.Instance.IsManagerInitialized)
            {
                Debug.LogError("AnsweredQuestionsManager não está inicializado");
                return new List<Question>();
            }

            List<Question> allQuestions = database.GetQuestions();
            
            if (allQuestions == null || allQuestions.Count == 0)
            {
                Debug.LogError("Database retornou lista nula ou vazia de questões");
                return new List<Question>();
            }

            Debug.Log($"Obtidas {allQuestions.Count} questões do database");

            if (string.IsNullOrEmpty(databankName))
            {
                databankName = database.GetDatabankName();
                Debug.Log($"Nome do banco definido: {databankName}");
            }
            
            // Registrar o número total de questões neste banco de dados
            int totalQuestions = allQuestions.Count;
            QuestionBankStatistics.SetTotalQuestions(databankName, totalQuestions);
            Debug.Log($"Total de questões em {databankName}: {totalQuestions}");

            List<string> answeredQuestions = await AnsweredQuestionsManager.Instance
                .FetchUserAnsweredQuestionsInTargetDatabase(databankName);

            Debug.Log($"Encontradas {answeredQuestions?.Count ?? 0} questões já respondidas");

            List<Question> unansweredQuestions = allQuestions
                .Where(q => q != null && !answeredQuestions.Contains(q.questionNumber.ToString()))
                .ToList();

            Debug.Log($"Restam {unansweredQuestions.Count} questões não respondidas de um total de {totalQuestions}");
            
            // Se todas as questões já foram respondidas, retorna todas para permitir revisão
            if (unansweredQuestions.Count == 0)
            {
                Debug.Log($"Todas as {totalQuestions} questões já foram respondidas. Retornando todas para revisão.");
                questions = allQuestions;
                return questions;
            }
            
            questions = unansweredQuestions;
            return questions;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro em LoadQuestionsFromDatabase: {e.Message}\nStackTrace: {e.StackTrace}");
            return new List<Question>();
        }
    }
}