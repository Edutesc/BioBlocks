using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    public Text scoreText;
    public string playerID = "id";
    public int playerScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (scoreText != null) 
            {
                var canvas = scoreText.transform.parent.gameObject;
                DontDestroyOnLoad(canvas);
            } 
            else
            {
                Debug.LogError("scoreText não está atribuído", this);
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Debug.Log("Iniciando a cena e verificando pontuação...");
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        playerScore = score;
        UpdateScoreText();
        SavePlayerScore(playerID);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else 
        {
           Debug.LogError("scoreText não está atribuída no ScoreManager.", this);
        }
    }

    public int GetScore()
    {
        return score; // Retorna a pontuação atual
    }

     public async void SavePlayerScore(string playerId)
    {
        Player player = new Player("Luciano", playerId, playerScore);
        FirebaseManager firebaseManager = FindAnyObjectByType<FirebaseManager>();
        if (firebaseManager != null)
        {
            await firebaseManager.SavePlayer(player);
            Debug.Log("chamando o firebase.saved");
        }
    }
}



