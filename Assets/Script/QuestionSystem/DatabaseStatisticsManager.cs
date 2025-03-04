using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuestionSystem;
using UnityEngine;

public class DatabaseStatisticsManager : MonoBehaviour
{
    private static DatabaseStatisticsManager instance;
    private bool isInitialized = false;
    private bool isInitializing = false;

    // Evento para notificar quando as estatísticas estiverem prontas
    public delegate void StatisticsReadyHandler();
    public static event StatisticsReadyHandler OnStatisticsReady;

    public static DatabaseStatisticsManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("FirebaseManager");
                if (go == null)
                {
                    go = new GameObject("FirebaseManager");
                }
                
                instance = go.GetComponent<DatabaseStatisticsManager>();
                if (instance == null)
                {
                    instance = go.AddComponent<DatabaseStatisticsManager>();
                }
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsInitialized => isInitialized;

    public async Task Initialize()
    {
        if (isInitialized || isInitializing) return;
        
        isInitializing = true;
        Debug.Log("Inicializando DatabaseStatisticsManager...");

        try
        {
            // Espera um frame para garantir que outros managers foram inicializados
            await Task.Yield();

            // Carrega estatísticas de todos os bancos de dados
            await LoadAllDatabaseStatistics();

            isInitialized = true;
            isInitializing = false;
            
            // Notifica que as estatísticas estão prontas
            OnStatisticsReady?.Invoke();
            
            Debug.Log("DatabaseStatisticsManager inicializado com sucesso");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro na inicialização do DatabaseStatisticsManager: {e.Message}");
            isInitializing = false;
            isInitialized = false;
        }
    }

    private async Task LoadAllDatabaseStatistics()
    {
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

        Dictionary<string, int> questionCounts = new Dictionary<string, int>();

        // Crie instâncias de cada banco de dados e conte as questões
        foreach (string databankName in allDatabases)
        {
            try
            {
                // Encontra o tipo do banco de dados
                Type databaseType = Type.GetType(databankName);
                if (databaseType == null)
                {
                    // Procurar em todos os assemblies
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        databaseType = assembly.GetType(databankName);
                        if (databaseType != null) break;
                    }
                }

                if (databaseType != null)
                {
                    // Cria uma instância temporária do banco
                    GameObject tempGO = new GameObject($"Temp_{databankName}");
                    var database = tempGO.AddComponent(databaseType) as IQuestionDatabase;
                    
                    if (database != null)
                    {
                        var questions = database.GetQuestions();
                        int count = questions != null ? questions.Count : 0;
                        
                        // Registra a contagem de questões
                        QuestionBankStatistics.SetTotalQuestions(databankName, count);
                        questionCounts[databankName] = count;
                        
                        Debug.Log($"Carregado banco {databankName}: {count} questões");
                    }
                    
                    // Limpa o GameObject temporário
                    Destroy(tempGO);
                }
                else
                {
                    Debug.LogWarning($"Não foi possível encontrar o tipo para {databankName}");
                    // Define um valor padrão
                    QuestionBankStatistics.SetTotalQuestions(databankName, 50);
                    questionCounts[databankName] = 50;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Erro ao carregar estatísticas para {databankName}: {e.Message}");
                // Define um valor padrão em caso de erro
                QuestionBankStatistics.SetTotalQuestions(databankName, 50);
                questionCounts[databankName] = 50;
            }
        }

        // Aguarda um pouco para garantir que as estatísticas sejam salvas
        await Task.Delay(100);
        
        // Log das estatísticas carregadas
        foreach (var kvp in questionCounts)
        {
            Debug.Log($"Estatísticas: {kvp.Key} tem {kvp.Value} questões");
        }
    }
}
