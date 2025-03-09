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
    [SerializeField] protected GameObject rankingRowPrefab; // Mudado para protected
    [SerializeField] protected RectTransform rankingTableContent; // Mudado para protected 
    [SerializeField] protected ScrollRect scrollRect; // Mudado para protected

    [Header("UI References")]
    [SerializeField] protected TMP_Text headerNameText; // Mudado para protected
    [SerializeField] protected TMP_Text headerScoreText; // Mudado para protected

    protected UserData currentUserData; // Mudado para protected
    protected List<Ranking> rankings; // Mudado para protected
    protected IRankingRepository rankingRepository; // Mudado para protected

    protected virtual void Start() // Mudado para protected virtual
    {
        if (rankingRowPrefab == null || rankingTableContent == null || scrollRect == null)
        {
            Debug.LogError("RankingManager: Referências obrigatórias não configuradas!");
            return;
        }

        InitializeRepository();
    }

    protected virtual void InitializeRepository() // Mudado para protected virtual
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

    protected virtual async Task InitializeRankingManager() // Mudado para protected virtual
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

    protected virtual void OnDestroy() // Mudado para protected virtual
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
    }

    protected virtual void OnUserDataChanged(UserData userData) // Mudado para protected virtual
    {
        currentUserData = userData;
        UpdateUI();
    }

    protected virtual void UpdateUI() // Mudado para protected virtual
    {
        if (currentUserData == null) return;

        headerNameText.text = currentUserData.NickName;
        headerScoreText.text = $"{currentUserData.Score} XP";
    }

    public virtual async Task FetchRankings() // Mudado para public virtual
    {
        try
        {
            rankings = await rankingRepository.GetRankingsAsync();

            Debug.Log($"Total de rankings obtidos: {rankings.Count}");

            if (rankings.Count > 0)
            {
                rankings = rankings.OrderByDescending(r => r.userScore).ToList();
                Debug.Log("Rankings ordenados por score");

                // Log dos top 5 para verificar
                for (int i = 0; i < Math.Min(5, rankings.Count); i++)
                {
                    Debug.Log($"Top {i + 1}: {rankings[i].userName} - {rankings[i].userScore}XP");
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

    protected virtual void UpdateRankingTable() // Mudado para protected virtual
    {
        if (rankingTableContent == null)
        {
            Debug.LogError("rankingTableContent é null!");
            return;
        }

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
                    // Configurar linha de separação visual
                    separatorUI.SetupAsExtraRow(currentUserRank, currentUserRanking.userName,
                        currentUserRanking.userScore, currentUserRanking.profileImageUrl);
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

    protected virtual IEnumerator ScrollToCurrentUser() // Mudado para protected virtual
    {
        yield return new WaitForEndOfFrame();

        // Se o usuário atual estiver nas últimas posições, rolar para baixo
        int currentUserRank = rankings.FindIndex(r => r.userName == currentUserData.NickName) + 1;
        if (currentUserRank > 15) // Ajuste este número conforme necessário
        {
            scrollRect.verticalNormalizedPosition = 0f; // Rola para o final
        }
    }

    protected virtual void CreateRankingRow(int rank, Ranking ranking, bool isCurrentUser) // Mudado para protected virtual
    {
        GameObject rowGO = Instantiate(rankingRowPrefab, rankingTableContent);
        var rowUI = rowGO.GetComponent<RankingRowUI>();
        if (rowUI != null)
        {
            rowUI.Setup(rank, ranking.userName, ranking.userScore,
                ranking.profileImageUrl, isCurrentUser);
        }
        else
        {
            Debug.LogError("RankingRowUI component not found on prefab!");
        }
    }

    protected virtual void OnRankingRowClicked(Ranking ranking) // Mudado para protected virtual
    {
        Debug.Log($"Clicked on ranking for user: {ranking.userName}");
    }

    public virtual void Navigate(string sceneName) // Mudado para public virtual
    {
        Debug.Log($"Navigating to {sceneName}");
        NavigationManager.Instance.NavigateTo(sceneName);
    }
}

// public static class RankingManagerDebugExtension
// {
//     public static void AddDebugLogs(this RankingManager manager)
//     {
//         Debug.Log("=== RANKING DEBUG START ===");

//         // Verificar referências serializadas
//         Debug.Log($"rankingRowPrefab null? {manager.rankingRowPrefab == null}");
//         Debug.Log($"rankingTableContent null? {manager.rankingTableContent == null}");
//         Debug.Log($"scrollRect null? {manager.scrollRect == null}");

//         // Verificar se o prefab tem todos os componentes necessários
//         var testRow = manager.rankingRowPrefab.GetComponent<RankingRowUI>();
//         Debug.Log($"RankingRowUI no prefab? {testRow != null}");

//         if (testRow != null)
//         {
//             // Verificar componentes do RankingRowUI
//             var imageManager = testRow.GetComponent<RankingImageManager>();
//             Debug.Log($"RankingImageManager encontrado? {imageManager != null}");

//             // Verificar configuração do Canvas
//             var canvas = GameObject.Find("Canvas");
//             if (canvas != null)
//             {
//                 var canvasScaler = canvas.GetComponent<CanvasScaler>();
//                 Debug.Log($"CanvasScaler configurado? {canvasScaler != null}");
//                 if (canvasScaler != null)
//                 {
//                     Debug.Log($"UI Scale Mode: {canvasScaler.uiScaleMode}");
//                     Debug.Log($"Reference Resolution: {canvasScaler.referenceResolution}");
//                 }
//             }
//         }

//         Debug.Log("=== RANKING DEBUG END ===");
//     }
// }