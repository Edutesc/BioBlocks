using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeekRankingRowUI : RankingRowUI
{
    [Header("Week Ranking Components")]
    [SerializeField] private TMP_Text weekScoreText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private LayoutElement weekScoreLayout;
    [SerializeField] private LayoutElement totalScoreLayout;

    protected override void ConfigureLayoutElements()
    {
        base.ConfigureLayoutElements();

        if (weekScoreLayout != null)
        {
            weekScoreLayout.preferredWidth = 100;
            weekScoreLayout.minWidth = 100;
        }

        if (totalScoreLayout != null)
        {
            totalScoreLayout.preferredWidth = 100;
            totalScoreLayout.minWidth = 100;
        }
    }

    public void SetupWeekRanking(int rank, string userName, int weekScore, int totalScore, string profileImageUrl, bool isCurrentUser)
    {
        // Call base setup first
        base.Setup(rank, userName, weekScore, profileImageUrl, isCurrentUser);
        
        // Override the score text to be specific about it being a week score
        if (scoreText != null)
            scoreText.text = $"{weekScore} XP";
            
        // Set the total score
        if (totalScoreText != null)
            totalScoreText.text = $"{totalScore} XP";
    }
}
