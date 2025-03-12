using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

public class RankingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject rankingRowPrefab;
    [SerializeField] protected RectTransform rankingTableContent;
    [SerializeField] protected ScrollRect scrollRect;

    [Header("UI References")]
    [SerializeField] protected TMP_Text headerNameText;
    [SerializeField] protected TMP_Text headerScoreText;

    [Header("Week Reset Information")]
    [SerializeField] private TMP_Text weekResetCountdownText;
    private WeekResetCountdown resetCountdown;

    protected UserData currentUserData;
    protected List<Ranking> rankings;
    protected IRankingRepository rankingRepository;

    protected virtual void Start()
    {
        if (rankingRowPrefab == null || rankingTableContent == null || scrollRect == null)
        {
            Debug.LogError("RankingManager: Referências obrigatórias não configuradas!");
            return;
        }

        InitializeRepository();
        InitializeWeekResetCountdown();
    }

    private void InitializeWeekResetCountdown()
    {
        if (weekResetCountdownText != null)
        {
            // Adicionar o componente WeekResetCountdown a este GameObject
            resetCountdown = gameObject.AddComponent<WeekResetCountdown>();

            // Inicializar com a referência ao texto
            resetCountdown.Initialize(weekResetCountdownText);
        }
    }

    protected virtual void InitializeRepository()
    {
        if (BioBlocksSettings.Instance.IsDebugMode())
        {
            Debug.Log("BioBlocks: Usando dados mock para desenvolvimento");
            rankingRepository = new MockRankingRepository();
            _ = InitializeRankingManager();
            return;
        }

        if (!FirestoreRepository.Instance.IsInitialized)
        {
            Debug.LogError("FirestoreRepository não está inicializado");
            return;
        }

        rankingRepository = new RankingRepository();
        _ = InitializeRankingManager();
    }

    protected virtual async Task InitializeRankingManager()
    {
        currentUserData = await rankingRepository.GetCurrentUserDataAsync();
        if (currentUserData != null)
        {
            UpdateUI();
            UserDataStore.OnUserDataChanged += OnUserDataChanged;
            await FetchRankings();
        }
        else
        {
            Debug.LogError("User data not loaded. Redirecting to Login.");
        }
    }

    protected virtual void OnDestroy()
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
    }

    protected virtual void OnUserDataChanged(UserData userData)
    {
        currentUserData = userData;
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        if (currentUserData == null) return;

        headerNameText.text = currentUserData.NickName;
        headerScoreText.text = $"{currentUserData.WeekScore} XP";
    }

    public virtual async Task FetchRankings()
    {
        try
        {
            rankings = await rankingRepository.GetRankingsAsync();

            Debug.Log($"Total de rankings obtidos: {rankings.Count}");

            if (rankings.Count > 0)
            {
                // Ordenar primariamente pelo WeekScore e usar TotalScore como critério de desempate
                rankings = rankings
                    .OrderByDescending(r => r.userWeekScore)     // Primeiro critério: WeekScore (maior para menor)
                    .ThenByDescending(r => r.userScore)         // Segundo critério: TotalScore para desempate
                    .ToList();

                Debug.Log("Rankings ordenados por WeekScore com desempate pelo TotalScore");

                // Log dos top 5 para verificar
                for (int i = 0; i < Math.Min(5, rankings.Count); i++)
                {
                    Debug.Log($"Top {i + 1}: {rankings[i].userName} - WeekScore: {rankings[i].userWeekScore}XP, TotalScore: {rankings[i].userScore}XP");
                }

                UpdateRankingTable();
            }
            else
            {
                Debug.LogWarning("Nenhum ranking foi adicionado à lista!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao buscar rankings: {e.Message}");
            rankings = new List<Ranking>();
        }
    }

    protected virtual void UpdateRankingTable()
    {
        if (rankingTableContent == null)
        {
            Debug.LogError("rankingTableContent é null!");
            return;
        }

        // Configurar o VerticalLayoutGroup para espaçamento adequado
        var verticalLayout = rankingTableContent.GetComponent<VerticalLayoutGroup>();
        if (verticalLayout == null)
            verticalLayout = rankingTableContent.gameObject.AddComponent<VerticalLayoutGroup>();

        verticalLayout.spacing = 5; // Ajuste este valor para controlar o espaçamento entre linhas
        verticalLayout.childAlignment = TextAnchor.UpperCenter;
        verticalLayout.childControlHeight = false;
        verticalLayout.childControlWidth = true;
        verticalLayout.childForceExpandHeight = false;
        verticalLayout.childForceExpandWidth = true;
        verticalLayout.padding = new RectOffset(10, 10, 10, 10); // Adiciona padding ao redor da lista

        Debug.Log($"Atualizando ranking table com {rankings.Count} rankings");
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
            CreateRankingRow(i + 1, ranking, applyCurrentUserStyle);
        }

        // Se o usuário atual não estiver no top 20, adicionar uma linha extra no final
        if (!top20Rankings.Any(r => r.userName == currentUserData.NickName))
        {
            int currentUserRank = rankings.FindIndex(r => r.userName == currentUserData.NickName) + 1;
            var currentUserRanking = rankings.Find(r => r.userName == currentUserData.NickName);

            if (currentUserRanking != null && currentUserRank > 20)
            {
                // Adicionar linha separadora (...)
                GameObject separatorGO = Instantiate(rankingRowPrefab, rankingTableContent);
                var separatorUI = separatorGO.GetComponent<RankingRowUI>();
                if (separatorUI != null)
                {
                    // Configurar linha de separação visual usando o método atualizado
                    separatorUI.SetupAsExtraRow(currentUserRank, currentUserRanking.userName,
                        currentUserRanking.userScore, currentUserRanking.userWeekScore,
                        currentUserRanking.profileImageUrl);
                }
            }
        }

        // Forçar atualização do layout
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rankingTableContent);

        // Opcional: Rolar para mostrar a linha do usuário atual se estiver fora da vista
        if (scrollRect != null)
        {
            StartCoroutine(ScrollToCurrentUser());
        }
    }

    protected virtual IEnumerator ScrollToCurrentUser()
    {
        yield return new WaitForEndOfFrame();

        // Se o usuário atual estiver nas últimas posições, rolar para baixo
        int currentUserRank = rankings.FindIndex(r => r.userName == currentUserData.NickName) + 1;
        if (currentUserRank > 15) // Ajuste este número conforme necessário
        {
            scrollRect.verticalNormalizedPosition = 0f; // Rola para o final
        }
    }

    protected virtual void CreateRankingRow(int rank, Ranking ranking, bool isCurrentUser)
    {
        GameObject rowGO = Instantiate(rankingRowPrefab, rankingTableContent);
        var rowUI = rowGO.GetComponent<RankingRowUI>();
        if (rowUI != null)
        {
            // Usar o método Setup que aceita tanto score total quanto semanal
            rowUI.Setup(rank, ranking.userName, ranking.userScore,
                        ranking.userWeekScore, ranking.profileImageUrl, isCurrentUser);
        }
        else
        {
            Debug.LogError("RankingRowUI component not found on prefab!");
        }
    }

    protected virtual void OnRankingRowClicked(Ranking ranking)
    {
        Debug.Log($"Clicked on ranking for user: {ranking.userName}");
    }

    public virtual void Navigate(string sceneName)
    {
        Debug.Log($"Navigating to {sceneName}");
        NavigationManager.Instance.NavigateTo(sceneName);
    }
}