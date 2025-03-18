using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class BonusSceneManager : MonoBehaviour
{
    [System.Serializable]
    public class BonusUIElements
    {
        public string bonusFirestoreName;
        public TextMeshProUGUI bonusCountText;
        public TextMeshProUGUI isBonusActiveText;
        public GameObject bonusContainer;
    }

    [Header("Bonus UI Mappings")]
    [SerializeField] private List<BonusUIElements> bonusUIMappings = new List<BonusUIElements>();

    [Header("Bonus Especial")]
    [SerializeField] private TextMeshProUGUI bonusCountBE;
    [SerializeField] private TextMeshProUGUI isBonusActiveBE;

    [Header("Bonus das Listas")]
    [SerializeField] private TextMeshProUGUI bonusCountBL;
    [SerializeField] private TextMeshProUGUI isBonusActiveBL;

    [Header("Bonus Incansável")]
    [SerializeField] private TextMeshProUGUI bonusCountBI;
    [SerializeField] private TextMeshProUGUI isBonusActiveBI;

    [Header("Bonus Especial Pro")]
    [SerializeField] private TextMeshProUGUI bonusCountBEPro;
    [SerializeField] private TextMeshProUGUI isBonusActiveBEPro;

    [Header("Bonus das Listas Pro")]
    [SerializeField] private TextMeshProUGUI bonusCountBLPro;
    [SerializeField] private TextMeshProUGUI isBonusActiveBLPro;

    [Header("Bonus Incansável Pro")]
    [SerializeField] private TextMeshProUGUI bonusCountBIPro;
    [SerializeField] private TextMeshProUGUI isBonusActiveBIPro;

    private BonusFirestore bonusFirestore;
    private string userId;
    private bool isInitialized = false;

    private void Awake()
    {
        bonusFirestore = new BonusFirestore();

        if (bonusUIMappings.Count == 0)
        {
            SetupDefaultBonusMappings();
        }
    }

    private void SetupDefaultBonusMappings()
    {
        bonusUIMappings = new List<BonusUIElements>
        {
            new BonusUIElements
            {
                bonusFirestoreName = "correctAnswerBonus",
                bonusCountText = bonusCountBE,
                isBonusActiveText = isBonusActiveBE
            },
            // Outros mapeamentos podem ser adicionados aqui quando implementados
        };
    }

    private void OnEnable()
    {
        InitializeAndFetchBonus();
    }

    private async void InitializeAndFetchBonus()
    {
        if (UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            userId = UserDataStore.CurrentUserData.UserId;
            await FetchBonuses();

            StartListeningForBonusUpdates();

            isInitialized = true;
        }
        else
        {
            Debug.LogWarning("BonusSceneManager: Usuário não está logado");
        }
    }

    public async Task FetchBonuses()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusSceneManager: UserId não definido");
            return;
        }

        try
        {
            List<BonusType> userBonuses = await bonusFirestore.GetUserBonuses(userId);
            UpdateBonusUI(userBonuses);
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusSceneManager: Erro ao buscar bônus: {e.Message}");
        }
    }

    private void UpdateBonusUI(List<BonusType> bonuses)
    {
        foreach (var bonusUIMapping in bonusUIMappings)
        {
            BonusType matchingBonus = bonuses.FirstOrDefault(b =>
                b.BonusName == bonusUIMapping.bonusFirestoreName);

            if (matchingBonus != null)
            {
                if (bonusUIMapping.bonusCountText != null)
                {
                    bonusUIMapping.bonusCountText.text = matchingBonus.BonusCount.ToString();
                }

                if (bonusUIMapping.isBonusActiveText != null)
                {
                    bonusUIMapping.isBonusActiveText.text = matchingBonus.IsBonusActive ? "Ativo" : "Inativo";
                }

                if (bonusUIMapping.bonusContainer != null)
                {
                    // Opção: mudar cor, ativar efeitos, etc.
                    // Por exemplo: bonusUIMapping.bonusContainer.GetComponent<Image>().color = matchingBonus.IsBonusActive ? activeColor : inactiveColor;
                }
            }
            else
            {
                if (bonusUIMapping.bonusCountText != null)
                {
                    bonusUIMapping.bonusCountText.text = "0";
                }

                if (bonusUIMapping.isBonusActiveText != null)
                {
                    bonusUIMapping.isBonusActiveText.text = "Inativo";
                }
            }
        }
    }

    private void StartListeningForBonusUpdates()
    {
        bonusFirestore.ListenForBonusUpdates(userId, updatedBonuses =>
        {
            UpdateBonusUI(updatedBonuses);
        });

    }

    private void OnDisable()
    {
        if (isInitialized)
        {
            StopListeningForBonusUpdates();
        }
    }

    private void StopListeningForBonusUpdates()
    {
        // Cancelar escuta de mudanças em tempo real
        // Exemplo: bonusFirestore.StopListeningForBonusUpdates();
    }

    public async void RefreshBonusUI()
    {
        await FetchBonuses();
    }
}
