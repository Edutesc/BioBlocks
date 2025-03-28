using System.Collections.Generic;
using System.Threading.Tasks;

public class MockRankingRepository : IRankingRepository
{
    private List<Ranking> mockRankings;
    private UserData mockUserData;
    
    public MockRankingRepository()
    {
        InitializeMockData();
    }
    
    private void InitializeMockData()
    {
        // Criar dados mock
        mockRankings = new List<Ranking>
        {
            new Ranking("Asoka", 1000, ""),
            new Ranking("Zico", 850, ""),
            new Ranking("Naruto", 700, ""),
            new Ranking("Yoda", 600, ""),
            new Ranking("Capitain Kirk", 500, ""),
            // Adicione mais jogadores mock conforme necessário
        };
        
        mockUserData = new UserData
        {
            NickName = "CurrentPlayer",
            Score = 600,
            ProfileImageUrl = "mock_url_current"
        };
    }
    
    public Task<List<Ranking>> GetRankingsAsync()
    {
        return Task.FromResult(mockRankings);
    }
    
    public Task<UserData> GetCurrentUserDataAsync()
    {
        return Task.FromResult(mockUserData);
    }
}

