
[System.Serializable]
public class Ranking
{
    public string userName { get; private set; }
    public int userScore { get; private set; }
    public string profileImageUrl { get; private set; }

    public Ranking(string userName, int userScore, string profileImageUrl)
    {
        this.userName = userName;
        this.userScore = userScore;
        this.profileImageUrl = profileImageUrl;
    }

}