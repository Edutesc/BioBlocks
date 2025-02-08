
using System.Collections.Generic;
using Firebase.Firestore;

[System.Serializable]
public class AnsweredQuestionsList
{
    [FirestoreProperty]
    public string DatabankName { get; set; }
    [FirestoreProperty]
    public List<int> QuestionsList { get; set; } = new List<int>();

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            {"DatabankName", DatabankName},
            {"QuestionsList", QuestionsList}
        };
    }
}

public static class AnsweredQuestionsListStore
{
    // Dicionário que armazena as contagens por usuário e por banco de dados
    private static Dictionary<string, Dictionary<string, int>> _answeredQuestionsCountByUser =
        new Dictionary<string, Dictionary<string, int>>();

    // Método para obter contagens apenas para um usuário específico
    public static Dictionary<string, int> GetAnsweredQuestionsCountForUser(string userId)
    {
        if (_answeredQuestionsCountByUser.TryGetValue(userId, out var userCounts))
        {
            return userCounts;
        }
        return new Dictionary<string, int>();
    }

    // Método para atualizar a contagem para um usuário específico
    public static void UpdateAnsweredQuestionsCount(string userId, string databankName, int count)
    {
        if (!_answeredQuestionsCountByUser.ContainsKey(userId))
        {
            _answeredQuestionsCountByUser[userId] = new Dictionary<string, int>();
        }

        _answeredQuestionsCountByUser[userId][databankName] = count;
    }

    // Método para limpar as contagens de um usuário específico
    public static void ClearUserAnsweredQuestions(string userId)
    {
        if (_answeredQuestionsCountByUser.ContainsKey(userId))
        {
            _answeredQuestionsCountByUser.Remove(userId);
        }
    }

    // Método para limpar todas as contagens
    public static void ClearAll()
    {
        _answeredQuestionsCountByUser.Clear();
    }
}

