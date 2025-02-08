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
                userData.ProfileImageUrl ?? ""
            )).ToList();

            return rankings;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao buscar rankings: {e.Message}");
            throw;
        }
    }

    private async Task<List<UserData>> GetAllUsersData()
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
}