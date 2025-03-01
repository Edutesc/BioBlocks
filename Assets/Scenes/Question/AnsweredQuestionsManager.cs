
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;

public class AnsweredQuestionsManager : MonoBehaviour
{
    private static AnsweredQuestionsManager instance;
    private string userId;
    private bool isInitialized = false;

    public delegate void AnsweredQuestionsUpdatedHandler(Dictionary<string, int> answeredCounts);
    public static event AnsweredQuestionsUpdatedHandler OnAnsweredQuestionsUpdated;

    public static AnsweredQuestionsManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("FirebaseManager");
                if (go != null)
                {
                    instance = go.GetComponent<AnsweredQuestionsManager>();
                    if (instance == null)
                    {
                        instance = go.AddComponent<AnsweredQuestionsManager>();
                    }
                }
                else
                {
                    Debug.LogError("FirebaseManager GameObject não encontrado!");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        Debug.Log("Attempting to awake...");
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        if (isInitialized) return;

        try
        {
            // Espera um frame para garantir que outros managers foram inicializados
            await Task.Yield();

            if (!FirestoreRepository.Instance.IsInitialized)
            {
                Debug.LogError("FirestoreRepository não está inicializado");
                return;
            }

            if (!AuthenticationRepository.Instance.IsUserLoggedIn())
            {
                Debug.LogError("Nenhum usuário está autenticado");
                return;
            }

            userId = AuthenticationRepository.Instance.Auth.CurrentUser.UserId;
            Debug.Log($"Inicializando AnsweredQuestionsManager para usuário: {userId}");

            await FetchUserAnsweredQuestions();
            isInitialized = true;
            Debug.Log("AnsweredQuestionsManager inicializado com sucesso");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro na inicialização do AnsweredQuestionsManager: {e.Message}");
            isInitialized = false;
        }
    }

    public async Task<List<string>> FetchUserAnsweredQuestionsInTargetDatabase(string target)
    {
        List<string> answeredQuestions = new List<string>();

        try
        {
            if (!isInitialized)
            {
                await Initialize();
                if (!isInitialized)
                {
                    Debug.LogError("Falha ao inicializar AnsweredQuestionsManager");
                    return answeredQuestions;
                }
            }

            UserData userData = await FirestoreRepository.Instance.GetUserData(userId);

            if (userData != null &&
                userData.AnsweredQuestions != null &&
                userData.AnsweredQuestions.ContainsKey(target))
            {
                answeredQuestions = userData.AnsweredQuestions[target]
                    .Select(q => q.ToString())
                    .ToList();

                Debug.Log($"Encontradas {answeredQuestions.Count} questões respondidas para {target}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao buscar questões respondidas para {target}: {e.Message}");
        }

        return answeredQuestions;
    }

    private async Task FetchUserAnsweredQuestions()
    {
        try
        {
            UserData userData = await FirestoreRepository.Instance.GetUserData(userId);

            if (userData != null && userData.AnsweredQuestions != null)
            {
                Dictionary<string, int> answeredCounts = new Dictionary<string, int>();

                foreach (var kvp in userData.AnsweredQuestions)
                {
                    string databankName = kvp.Key;
                    List<int> questionsList = kvp.Value;

                    if (questionsList != null)
                    {
                        // Remover possíveis duplicatas
                        var distinctQuestions = questionsList.Distinct().ToList();

                        // Se houver diferença entre as listas, log para debug
                        if (distinctQuestions.Count != questionsList.Count)
                        {
                            Debug.LogWarning($"Encontradas {questionsList.Count - distinctQuestions.Count} questões duplicadas em {databankName}");
                            Debug.LogWarning($"Lista original: {string.Join(", ", questionsList)}");
                            Debug.LogWarning($"Lista sem duplicatas: {string.Join(", ", distinctQuestions)}");
                        }

                        int count = distinctQuestions.Count;

                        // Validar se o count não excede o limite
                        if (count > 50)
                        {
                            Debug.LogError($"ERRO: Número de questões ({count}) excede o limite de 50 para {databankName}");
                            count = 50; // Forçar limite máximo
                        }

                        answeredCounts[databankName] = count;
                        AnsweredQuestionsListStore.UpdateAnsweredQuestionsCount(userId, databankName, count);
                        Debug.Log($"Atualizado {databankName}: {count} questões respondidas (Lista: {string.Join(", ", distinctQuestions)})");
                    }
                }

                Debug.Log($"Disparando evento OnAnsweredQuestionsUpdated com {answeredCounts.Count} bancos de dados");
                OnAnsweredQuestionsUpdated?.Invoke(answeredCounts);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao buscar dados do usuário: {e.Message}");
            throw;
        }
    }

    // Método público para forçar atualização
    public async Task ForceUpdate()
    {
        try
        {
            Debug.Log("Iniciando ForceUpdate do AnsweredQuestionsManager");

            if (!isInitialized)
            {
                await Initialize();
                if (!isInitialized)
                {
                    Debug.LogError("Falha ao inicializar durante ForceUpdate");
                    return;
                }
            }

            await FetchUserAnsweredQuestions();
            Debug.Log("ForceUpdate concluído com sucesso");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro em ForceUpdate: {e.Message}");
        }
    }
    
    private void UpdateUI(Dictionary<string, int> answeredCounts)
    {
        if (SceneManager.GetActiveScene().name != "PathwayScene")
        {
            return;
        }

        var userCounts = AnsweredQuestionsListStore.GetAnsweredQuestionsCountForUser(userId);

        foreach (var kvp in userCounts)
        {
            string databankName = kvp.Key;
            int count = kvp.Value;

            // Obter o número total de questões neste banco de dados
            int totalQuestions = QuestionBankStatistics.GetTotalQuestions(databankName);

            // Se não tiver informações do banco de dados, usar o valor padrão (para compatibilidade com bancos de dados antigos)
            if (totalQuestions <= 0)
            {
                totalQuestions = 50; // Valor padrão
                Debug.LogWarning($"Número total de questões para {databankName} não encontrado. Usando valor padrão: {totalQuestions}");
            }

            // Garantir que a contagem não exceda o total
            count = Mathf.Min(count, totalQuestions);

            int percentageAnswered = (int)Math.Floor((count * 100.0) / totalQuestions);

            // Garantir que a porcentagem não exceda 100%
            percentageAnswered = Math.Min(percentageAnswered, 100);

            string objectName = $"{databankName}PorcentageText";
            GameObject textObject = GameObject.Find(objectName);

            if (textObject == null)
            {
                Debug.LogError($"Não foi possível encontrar o GameObject: {objectName}");
                continue;
            }

            TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
            if (tmpText == null)
            {
                Debug.LogError($"Componente TextMeshProUGUI não encontrado no GameObject: {objectName}");
                continue;
            }

            tmpText.text = $"{percentageAnswered}%";
            Debug.Log($"DatabankName: {databankName}, Count: {count}/{totalQuestions}, Percentage: {percentageAnswered}%");
        }

        // Verificar e atualizar bancos de dados sem respostas
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
            if (!userCounts.ContainsKey(databankName))
            {
                string objectName = $"{databankName}PorcentageText";
                GameObject textObject = GameObject.Find(objectName);

                if (textObject != null)
                {
                    TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
                    if (tmpText != null)
                    {
                        tmpText.text = "0%";
                        Debug.Log($"{databankName}PorcentageText definido como 0%");
                    }
                }
            }
        }
    }

    public async Task<bool> HasRemainingQuestions(string currentDatabase, List<string> currentQuestionList)
    {
        try
        {
            // Tentar inicializar se ainda não estiver inicializado
            if (!isInitialized)
            {
                await Initialize();
                if (!isInitialized)
                {
                    Debug.LogError("Falha ao inicializar AnsweredQuestionsManager");
                    return false;
                }
            }

            List<string> answeredQuestions = await FetchUserAnsweredQuestionsInTargetDatabase(currentDatabase);
            bool hasRemaining = currentQuestionList.Except(answeredQuestions).Any();

            Debug.Log($"Verificação de questões restantes para {currentDatabase}: " +
                     $"Total={currentQuestionList.Count}, " +
                     $"Respondidas={answeredQuestions.Count}, " +
                     $"Restantes={hasRemaining}");

            return hasRemaining;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao verificar questões restantes: {e.Message}");
            return false;
        }
    }

    public void ResetManager()
    {
        isInitialized = false;
        userId = null;
    }

    public bool IsManagerInitialized => isInitialized;
}