using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public interface IQuestionDatabase
{
    List<Question> GetQuestions();
}

public class QuestionManagerScript : MonoBehaviour
{
    public GameObject questionPrefab;
    public Transform parentTransform;
    public string nextSceneName;
    public Text feedbackText; // Texto para feedback
    public Text scoreText; // Texto para exibir a pontuação

    private List<Question> currentQuestions;
    private int currentQuestionIndex = 0;
    private int currentAttempts = 0; // Contador de tentativas

    private void Start()
    {
        LoadQuestionsForCurrentScene();

        if (currentQuestions != null && currentQuestions.Count > 0)
        {
            ShowQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.LogError("No questions available.");
        }

        if (feedbackText != null)
        {
            feedbackText.text = ""; // Limpa feedback ao iniciar
        }
    }

    private void LoadQuestionsForCurrentScene()
    {
        // Usando FindObjectsByType
        MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (MonoBehaviour behaviour in allBehaviours)
        {
            if (behaviour is IQuestionDatabase database)
            {
                currentQuestions = database.GetQuestions();
                return; // Para após encontrar o primeiro banco de dados válido
            }
        }

        Debug.LogWarning("No QuestionDatabase found in the scene.");
        currentQuestions = new List<Question>();
    }

    void ShowQuestion(int index)
    {
        currentAttempts = 0; // Reinicia as tentativas para nova pergunta

        GameObject questionInstance = Instantiate(questionPrefab, parentTransform);
        Question currentQuestion = currentQuestions[index];

        Text questionText = questionInstance.transform.Find("QuestionText").GetComponent<Text>();
        questionText.text = currentQuestion.questionText;

        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            Button answerButton = questionInstance.transform.Find("Answer" + (i + 1)).GetComponent<Button>();
            answerButton.GetComponentInChildren<Text>().text = currentQuestion.answers[i];

            int capturedIndex = i; // Captura o índice da resposta
            answerButton.onClick.AddListener(() => CheckAnswer(capturedIndex, questionInstance));
        }
    }

    public void CheckAnswer(int selectedAnswerIndex, GameObject questionInstance)
    {
        currentAttempts++; // Incrementa tentativas

        if (selectedAnswerIndex == currentQuestions[currentQuestionIndex].correctIndex)
        {
            // Chama para calcular a pontuação
            CalculateScore(); 
            ShowFeedback("Resposta correta!");
            Destroy(questionInstance);
            currentQuestionIndex++;

            if (currentQuestionIndex < currentQuestions.Count)
            {
                Invoke(nameof(DisplayNextQuestion), 2f); // Espera 2 segundos antes de exibir próxima pergunta
            }
            else
            {
                Invoke(nameof(LoadNextScene), 2f); // Volta para a cena inicial após duas seg
            }
        }
        else
        {
            ShowFeedback("Resposta errada! Tente novamente.");
        }
    }

    private void SavePlayerScore()
    {
        // Aqui você pode definir um ID do jogador, por exemplo, usando o nome do jogador ou outra lógica.
        string playerId = "player1"; // Substitua pelo método apropriado para obter o ID do jogador, se necessário.
        ScoreManager.instance.SavePlayerScore(playerId); // Salva a pontuação após responder a última pergunta
        Invoke(nameof(LoadNextScene), 2f); // Volta para a cena inicial após salvar
    }

    private void CalculateScore()
    {
        int scoreToAdd = 0; // Inicializa a quantidade a adicionar

        // Determina a pontuação com base no número de tentativas
        switch (currentAttempts)
        {
            case 1:
                scoreToAdd = 10; // 10 pontos para a primeira tentativa
                break;
            case 2:
                scoreToAdd = 8; // 8 pontos para a segunda tentativa
                break;
            case 3:
                scoreToAdd = 5; // 5 pontos para a terceira tentativa
                break;
            case 4:
                scoreToAdd = 1; // 1 ponto para a quarta tentativa
                break;
            default:
                break;
        }

        ScoreManager.instance.AddScore(scoreToAdd); // Atualiza o ScoreManager com a nova pontuação
        UpdateScoreText(); // Atualiza a pontuação na UI após a resposta correta
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + ScoreManager.instance.GetScore(); // Atualiza a exibição do score
        }
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message; // Mostra feedback
        }
    }

    private void DisplayNextQuestion()
    {
        ShowQuestion(currentQuestionIndex);
        feedbackText.text = ""; // Limpa mensagem de feedback
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName); // Navega para a próxima cena
    }
}

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] answers;
    public int correctIndex;
}

