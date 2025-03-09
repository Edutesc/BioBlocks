
[System.Serializable]
public class Ranking
{
    public string userName { get; private set; }
    public int userScore { get; private set; }
    public int userWeekScore { get; private set; }
    public string profileImageUrl { get; private set; }

    // Construtor original para manter compatibilidade com código existente
    public Ranking(string userName, int userScore, string profileImageUrl)
    {
        this.userName = userName;
        this.userScore = userScore;
        this.userWeekScore = 0; // Valor padrão
        this.profileImageUrl = profileImageUrl;
    }

    // Novo construtor que inclui o score semanal
    public Ranking(string userName, int userScore, int userWeekScore, string profileImageUrl)
    {
        this.userName = userName;
        this.userScore = userScore;
        this.userWeekScore = userWeekScore;
        this.profileImageUrl = profileImageUrl;
    }
}
