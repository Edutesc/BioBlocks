using System.Threading.Tasks;
using System.Collections.Generic;

public interface IRankingRepository
{
    Task<UserData> GetCurrentUserDataAsync();
    Task<List<Ranking>> GetRankingsAsync();
}

