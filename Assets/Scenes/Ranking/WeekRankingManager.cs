using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class WeekRankingManager : RankingManager
{
    [Header("Week Ranking References")]
    [SerializeField] private GameObject weekRankingRowPrefab; // Use custom prefab with week/total score
    [SerializeField] private TMP_Text weekDatesText;

    private DateTime weekStartDate;
    private DateTime weekEndDate;

    protected override void Start() // Use override corretamente
    {
        if (weekRankingRowPrefab == null)
        {
            Debug.LogWarning("WeekRankingRowPrefab não foi configurado, usando RankingRowPrefab padrão");
            weekRankingRowPrefab = rankingRowPrefab;
        }

        if (rankingRowPrefab == null || rankingTableContent == null || scrollRect == null)
        {
            Debug.LogError("WeekRankingManager: Referências obrigatórias não configuradas!");
            return;
        }

        CalculateWeekDates();
        UpdateWeekDatesUI();
        InitializeRepository();
    }

    private void CalculateWeekDates()
    {
        // Get current date in Brasília time
        TimeZoneInfo brasiliaTimeZone;
        try
        {
            brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            Debug.LogWarning("Fuso horário de Brasília não encontrado, usando UTC");
            brasiliaTimeZone = TimeZoneInfo.Utc;
        }

        DateTime nowInBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);
        
        // Calculate start of week (Sunday 00:00)
        int daysFromSunday = (int)nowInBrasilia.DayOfWeek;
        weekStartDate = nowInBrasilia.Date.AddDays(-daysFromSunday);
        
        // Calculate end of week (Saturday 23:59:59)
        weekEndDate = weekStartDate.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
        
        Debug.Log($"Semana atual: {weekStartDate:dd/MM/yyyy} - {weekEndDate:dd/MM/yyyy}");
    }

    private void UpdateWeekDatesUI()
    {
        if (weekDatesText != null)
        {
            weekDatesText.text = $"Semana: {weekStartDate:dd/MM/yyyy} - {weekEndDate:dd/MM/yyyy}";
        }
    }

    protected override void UpdateUI()
    {
        if (currentUserData == null) return;

        headerNameText.text = currentUserData.NickName;
        headerScoreText.text = $"{currentUserData.WeekScore} XP";
    }

    public override async Task FetchRankings()
    {
        try
        {
            // Get rankings with week scores
            var allUsersData = await ((RankingRepository)rankingRepository).GetAllUsersData();
            
            rankings = allUsersData.Select(userData => new Ranking(
                userData.NickName,
                userData.Score,
                userData.WeekScore,
                userData.ProfileImageUrl ?? ""
            )).ToList();

            Debug.Log($"Total de rankings semanais obtidos: {rankings.Count}");

            if (rankings.Count > 0)
            {
                // Sort by week score instead of total score
                rankings = rankings.OrderByDescending(r => r.userWeekScore).ToList();
                Debug.Log("Rankings ordenados por score semanal");

                for (int i = 0; i < Math.Min(5, rankings.Count); i++)
                {
                    Debug.Log($"Top {i + 1}: {rankings[i].userName} - {rankings[i].userWeekScore}XP (Total: {rankings[i].userScore}XP)");
                }

                UpdateRankingTable();
            }
            else
            {
                Debug.LogWarning("Nenhum ranking semanal foi adicionado à lista!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao buscar rankings semanais: {e.Message}");
            rankings = new List<Ranking>();
        }
    }

    protected override void UpdateRankingTable()
    {
        if (rankingTableContent == null)
        {
            Debug.LogError("rankingTableContent é null!");
            return;
        }

        Debug.Log($"Atualizando ranking table com {rankings.Count} rankings semanais");
        
        // Limpar linhas existentes
        foreach (Transform child in rankingTableContent)
        {
            Destroy(child.gameObject);
        }

        var top20Rankings = rankings.Take(20).ToList();

        // Criar as primeiras 20 linhas
        for (int i = 0; i < top20Rankings.Count; i++)
        {
            var ranking = top20Rankings[i];
            bool isCurrentUser = ranking.userName == currentUserData.NickName;

            // Só aplicar estilo de currentUser se não estiver no top 3
            bool applyCurrentUserStyle = isCurrentUser && (i + 1) > 3;
            CreateWeekRankingRow(i + 1, ranking, applyCurrentUserStyle);
        }

        // Se o usuário atual não estiver no top 20, adicionar uma linha extra no final
        if (!top20Rankings.Any(r => r.userName == currentUserData.NickName))
        {
            int currentUserRank = rankings.FindIndex(r => r.userName == currentUserData.NickName) + 1;
            var currentUserRanking = rankings.Find(r => r.userName == currentUserData.NickName);

            if (currentUserRanking != null && currentUserRank > 20)
            {
                // Adicionar linha separadora (...)
                GameObject separatorGO = Instantiate(weekRankingRowPrefab, rankingTableContent);
                var separatorUI = separatorGO.GetComponent<WeekRankingRowUI>();
                if (separatorUI != null)
                {
                    // Configurar linha de separação visual como no ranking original
                    separatorUI.SetupWeekRanking(currentUserRank, currentUserRanking.userName,
                        currentUserRanking.userWeekScore, currentUserRanking.userScore,
                        currentUserRanking.profileImageUrl, true);
                }
            }
        }

        // Forçar atualização do layout
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rankingTableContent);

        // Rolar para mostrar a linha do usuário atual
        if (scrollRect != null)
        {
            StartCoroutine(ScrollToCurrentUser());
        }
    }

    private void CreateWeekRankingRow(int rank, Ranking ranking, bool isCurrentUser)
    {
        GameObject rowGO = Instantiate(weekRankingRowPrefab, rankingTableContent);
        var rowUI = rowGO.GetComponent<WeekRankingRowUI>();
        if (rowUI != null)
        {
            rowUI.SetupWeekRanking(rank, ranking.userName, ranking.userWeekScore,
                ranking.userScore, ranking.profileImageUrl, isCurrentUser);
        }
        else
        {
            Debug.LogError("WeekRankingRowUI component not found on prefab!");
            
            // Fallback to regular RankingRowUI if available
            var regularRowUI = rowGO.GetComponent<RankingRowUI>();
            if (regularRowUI != null)
            {
                regularRowUI.Setup(rank, ranking.userName, ranking.userWeekScore,
                    ranking.profileImageUrl, isCurrentUser);
            }
        }
    }
}
