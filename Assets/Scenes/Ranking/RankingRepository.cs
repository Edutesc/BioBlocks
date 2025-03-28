using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class RankingRepository : IRankingRepository
{
    public async Task<UserData> GetCurrentUserDataAsync()
    {
        if (AuthenticationRepository.Instance.Auth.CurrentUser == null)
        {
            Debug.LogError("Usuário não está autenticado");
            return null;
        }

        string userId = AuthenticationRepository.Instance.Auth.CurrentUser.UserId;
        return await FirestoreRepository.Instance.GetUserData(userId);
    }

    public async Task<List<Ranking>> GetRankingsAsync()
    {
        try
        {
            var usersData = await GetAllUsersData();

            List<Ranking> rankings = usersData.Select(userData => new Ranking(
                userData.NickName,
                userData.Score,
                userData.WeekScore,
                userData.ProfileImageUrl ?? ""
            )).ToList();

            // Log para depuração
            Debug.Log($"GetRankingsAsync - Amostra de rankings:");
            for (int i = 0; i < Math.Min(3, rankings.Count); i++)
            {
                Debug.Log($"Usuário: {rankings[i].userName}, Score: {rankings[i].userScore}, WeekScore: {rankings[i].userWeekScore}");
            }

            return rankings;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao buscar rankings: {e.Message}");
            throw;
        }
    }

    public async Task<List<Ranking>> GetWeekRankingsAsync()
    {
        try
        {
            var usersData = await GetAllUsersData();

            List<Ranking> rankings = usersData.Select(userData => new Ranking(
                userData.NickName,
                userData.Score,
                userData.WeekScore,
                userData.ProfileImageUrl ?? ""
            )).ToList();

            return rankings;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao buscar rankings semanais: {e.Message}");
            throw;
        }
    }

    public async Task<List<UserData>> GetAllUsersData()
    {
        try
        {
            return await FirestoreRepository.Instance.GetAllUsersData();
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao buscar dados dos usuários: {e.Message}");
            throw;
        }
    }

    public async Task UpdateUserWeekScoreAsync(string userId, int additionalScore)
    {
        try
        {
            await FirestoreRepository.Instance.UpdateUserWeekScore(userId, additionalScore);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao atualizar WeekScore: {e}");
            throw;
        }
    }

    public async Task ResetAllWeeklyScoresAsync()
    {
        try
        {
            await FirestoreRepository.Instance.ResetAllWeeklyScores();
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao resetar scores semanais: {e}");
            throw;
        }
    }
}