using UnityEngine;
using System.Collections.Generic;

public static class QuestionBankStatistics
{
    // Dicionário que armazena o número total de questões por banco de dados
    private static Dictionary<string, int> totalQuestionsPerDatabase = new Dictionary<string, int>();
    
    // Método para definir o número total de questões para um banco de dados específico
    public static void SetTotalQuestions(string databaseName, int count)
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            Debug.LogError("Nome do banco de dados inválido");
            return;
        }
        
        totalQuestionsPerDatabase[databaseName] = count;
        Debug.Log($"Banco de dados '{databaseName}' atualizado com {count} questões no total");
    }
    
    // Método para obter o número total de questões de um banco de dados
    public static int GetTotalQuestions(string databaseName)
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            Debug.LogError("Nome do banco de dados inválido");
            return 0;
        }
        
        if (totalQuestionsPerDatabase.TryGetValue(databaseName, out int count))
        {
            return count;
        }
        
        Debug.LogWarning($"Número total de questões para '{databaseName}' não encontrado. Retornando 0.");
        return 0;
    }
    
    // Verifica se todas as questões de um banco de dados foram respondidas
    public static bool AreAllQuestionsAnswered(string databaseName, int answeredCount)
    {
        int totalQuestions = GetTotalQuestions(databaseName);
        return totalQuestions > 0 && answeredCount >= totalQuestions;
    }
}