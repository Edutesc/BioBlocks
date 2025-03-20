using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class BonusSceneManager : MonoBehaviour
{
    [Header("Bonus UI Mappings")]
    [SerializeField] private List<BonusUIElements> bonusUIMappings = new List<BonusUIElements>();

    [Header("Bonus Especial")]
    [SerializeField] private TextMeshProUGUI bonusCountBE;
    [SerializeField] private TextMeshProUGUI isBonusActiveBE;
    [SerializeField] private Button bonusButtonBE;

    [Header("Bonus das Listas")]
    [SerializeField] private TextMeshProUGUI bonusCountBL;
    [SerializeField] private TextMeshProUGUI isBonusActiveBL;
    [SerializeField] private Button bonusButtonBL;

    [Header("Bonus Incansável")]
    [SerializeField] private TextMeshProUGUI bonusCountBI;
    [SerializeField] private TextMeshProUGUI isBonusActiveBI;
    [SerializeField] private Button bonusButtonBI;

    [Header("Bonus Especial Pro")]
    [SerializeField] private TextMeshProUGUI bonusCountBEPro;
    [SerializeField] private TextMeshProUGUI isBonusActiveBEPro;
    [SerializeField] private Button bonusButtonBEPro;

    [Header("Bonus das Listas Pro")]
    [SerializeField] private TextMeshProUGUI bonusCountBLPro;
    [SerializeField] private TextMeshProUGUI isBonusActiveBLPro;
    [SerializeField] private Button bonusButtonBLPro;

    [Header("Bonus Incansável Pro")]
    [SerializeField] private TextMeshProUGUI bonusCountBIPro;
    [SerializeField] private TextMeshProUGUI isBonusActiveBIPro;
    [SerializeField] private Button bonusButtonBIPro;

    [Header("Visual Settings")]
    [SerializeField] private float inactiveAlpha = 0.6f;
    [SerializeField] private bool useGrayscaleWhenInactive = false;

    private SpecialBonusManager specialBonusManager;
    private string userId;
    private bool isInitialized = false;

    private void Awake()
    {
        specialBonusManager = new SpecialBonusManager();

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
                bonusFirestoreName = "specialBonus",
                bonusCountText = bonusCountBE,
                isBonusActiveText = isBonusActiveBE,
                bonusButton = bonusButtonBE,
                bonusContainer = bonusButtonBE?.gameObject
            },
            new BonusUIElements
            {
                bonusFirestoreName = "listCompletionBonus",
                bonusCountText = bonusCountBL,
                isBonusActiveText = isBonusActiveBL,
                bonusButton = bonusButtonBL,
                bonusContainer = bonusButtonBL?.gameObject
            },
            
            new BonusUIElements
            {
                bonusFirestoreName = "persistenceBonus",
                bonusCountText = bonusCountBI,
                isBonusActiveText = isBonusActiveBI,
                bonusButton = bonusButtonBI,
                bonusContainer = bonusButtonBI?.gameObject
            },
            new BonusUIElements
            {
                bonusFirestoreName = "correctAnswerBonusPro",
                bonusCountText = bonusCountBEPro,
                isBonusActiveText = isBonusActiveBEPro,
                bonusButton = bonusButtonBEPro,
                bonusContainer = bonusButtonBEPro?.gameObject
            },
            new BonusUIElements
            {
                bonusFirestoreName = "listCompletionBonusPro",
                bonusCountText = bonusCountBLPro,
                isBonusActiveText = isBonusActiveBLPro,
                bonusButton = bonusButtonBLPro,
                bonusContainer = bonusButtonBLPro?.gameObject
            },
            new BonusUIElements
            {
                bonusFirestoreName = "persistenceBonusPro",
                bonusCountText = bonusCountBIPro,
                isBonusActiveText = isBonusActiveBIPro,
                bonusButton = bonusButtonBIPro,
                bonusContainer = bonusButtonBIPro?.gameObject
            }
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
            List<BonusType> userBonuses = await specialBonusManager.GetUserBonuses(userId);
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

            bool isActive = false;
            int count = 0;

            if (matchingBonus != null)
            {
                count = matchingBonus.BonusCount;
                isActive = matchingBonus.IsBonusActive;

                if (bonusUIMapping.bonusCountText != null)
                {
                    bonusUIMapping.bonusCountText.text = count.ToString();
                }

                if (bonusUIMapping.isBonusActiveText != null)
                {
                    bonusUIMapping.isBonusActiveText.text = isActive ? "Ativo" : "Inativo";
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

            UpdateButtonState(bonusUIMapping, isActive);

            if (isActive)
            {
                SetupButtonAction(bonusUIMapping, matchingBonus);
            }
        }
    }

    private void UpdateButtonState(BonusUIElements bonusUI, bool isActive)
    {
        if (bonusUI.bonusButton != null)
        {
            bonusUI.bonusButton.interactable = isActive;

            Image buttonImage = bonusUI.bonusButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                if (isActive)
                {
                    if (bonusUI.useCustomColors)
                    {
                        buttonImage.color = new Color(
                            bonusUI.customColors.normalColor.r,
                            bonusUI.customColors.normalColor.g,
                            bonusUI.customColors.normalColor.b,
                            1.0f
                        );
                    }
                    else
                    {
                        buttonImage.color = buttonImage.color.WithAlpha(1.0f);
                    }
                }
                else
                {
                    if (useGrayscaleWhenInactive)
                    {
                        Color originalColor = buttonImage.color;
                        float grayValue = originalColor.r * 0.3f + originalColor.g * 0.59f + originalColor.b * 0.11f;
                        buttonImage.color = new Color(grayValue, grayValue, grayValue, originalColor.a * inactiveAlpha);
                    }
                    else
                    {
                        buttonImage.color = buttonImage.color.WithAlpha(inactiveAlpha);
                    }
                }
            }

            if (bonusUI.bonusCountText != null)
            {
                bonusUI.bonusCountText.color = bonusUI.bonusCountText.color.WithAlpha(isActive ? 1f : 0.8f);
            }

            if (bonusUI.isBonusActiveText != null)
            {
                bonusUI.isBonusActiveText.color = bonusUI.isBonusActiveText.color.WithAlpha(isActive ? 1f : 0.8f);
            }
        }
    }

    private void SetupButtonAction(BonusUIElements bonusUI, BonusType bonus)
    {
        if (bonusUI.bonusButton != null)
        {
            bonusUI.bonusButton.onClick.RemoveAllListeners();
            bonusUI.bonusButton.onClick.AddListener(() =>
            {
                UseBonusAction(bonus.BonusName);
            });
        }
    }

    public async void UseBonusAction(string bonusType)
    {
        switch (bonusType)
        {
            case "specialBonus":
                await ActivateSpecialBonus();
                break;

            case "listCompletionBonus":
                await ActivateListCompletionBonus();
                break;

            case "persistenceBonus":
                await ActivatePersistenceBonus();
                break;

            case "correctAnswerBonusPro":
                await ActivateCorrectAnswerBonusPro();
                break;

            default:
                Debug.LogWarning($"Tipo de bônus não implementado: {bonusType}");
                break;
        }

        await FetchBonuses();
    }

    private async Task ActivateSpecialBonus()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusSceneManager: UserId não definido");
            return;
        }
        
        try
        {
            await specialBonusManager.ActivateBonus(userId, "specialBonus", 900f);
            await FetchBonuses();
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusSceneManager: Erro ao ativar bônus especial: {e.Message}");
        }
    }

    private async Task ActivateListCompletionBonus()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusSceneManager: UserId não definido");
            return;
        }
        
        try
        {
            await specialBonusManager.ActivateBonus(userId, "listCompletionBonus", 300f);
            await FetchBonuses();
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusSceneManager: Erro ao ativar bônus das listas: {e.Message}");
        }
    }

    private async Task ActivatePersistenceBonus()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusSceneManager: UserId não definido");
            return;
        }
        
        try
        {
            await specialBonusManager.ActivateBonus(userId, "persistenceBonus", 900f);
            await FetchBonuses();
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusSceneManager: Erro ao ativar bônus incansável: {e.Message}");
        }
    }

    private async Task ActivateCorrectAnswerBonusPro()
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusSceneManager: UserId não definido");
            return;
        }
        
        try
        {
            await specialBonusManager.ActivateBonus(userId, "correctAnswerBonusPro", 1200f);
            await FetchBonuses();
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusSceneManager: Erro ao ativar bônus especial pro: {e.Message}");
        }
    }

    private void StartListeningForBonusUpdates()
    {
        // Implementar escuta de atualizações em tempo real do Firestore
        // Isto pode ser implementado de várias maneiras:
        
        // 1. Usando o listener padrão do Firestore:
        // DocumentReference docRef = FirebaseFirestore.DefaultInstance.Collection("UserBonus").Document(userId);
        // listenerRegistration = docRef.Listen(snapshot => {
        //     if (snapshot.Exists) {
        //         UpdateBonusesFromSnapshot(snapshot);
        //     }
        // });
        
        // 2. Ou usando um método de polling:
        StartCoroutine(BonusUpdatePollingCoroutine());
    }
    
    private IEnumerator BonusUpdatePollingCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            if (!isInitialized || string.IsNullOrEmpty(userId))
            {
                yield break;
            }
            
            _ = FetchBonuses();
        }
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
        StopAllCoroutines();
        
        // Se estiver usando listener do Firestore:
        // listenerRegistration?.Stop();
        // listenerRegistration = null;
    }

    public async void RefreshBonusUI()
    {
        await FetchBonuses();
    }
}