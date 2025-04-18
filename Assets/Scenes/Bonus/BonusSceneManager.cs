using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    private UserBonusManager userBonusManager;
    private string userId;
    private bool isInitialized = false;

    private void Awake()
    {
        userBonusManager = new UserBonusManager();

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
        HalfViewRegistry.OnAnyHalfViewHidden += OnAnyHalfViewHidden;
    }

    private void OnDisable()
    {
        if (isInitialized)
        {
            StopListeningForBonusUpdates();
        }

        HalfViewRegistry.OnAnyHalfViewHidden -= OnAnyHalfViewHidden;
    }

    private void OnAnyHalfViewHidden()
    {
        Debug.Log("HalfView escondido, forçando atualização da UI");
        ForceUpdateUIAndReactivateButton();
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
            List<BonusType> userBonuses = await userBonusManager.GetUserBonuses(userId);
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

                if (bonusUIMapping.bonusFirestoreName == "specialBonus" && count >= 5)
                {
                    isActive = true;

                    // Forçar a atualização do objeto BonusType
                    if (!matchingBonus.IsBonusActive)
                    {
                        matchingBonus.IsBonusActive = true;
                        Debug.Log("Forçando ativação do status do botão Special Bonus");
                    }
                }
                else if (bonusUIMapping.bonusFirestoreName == "listCompletionBonus" && count > 0)
                {
                    // Para o Bonus das Listas, ativar se tiver pelo menos 1
                    isActive = true;

                    // Garantir que esteja marcado como ativo no objeto
                    if (!matchingBonus.IsBonusActive)
                    {
                        matchingBonus.IsBonusActive = true;
                        Debug.Log($"Forçando ativação do status do botão List Completion Bonus (Count: {count})");
                    }
                }
                else
                {
                    isActive = matchingBonus.IsBonusActive;
                }

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

            if (bonusUIMapping.bonusFirestoreName == "specialBonus")
            {
                Debug.Log($"Special Bonus - Count: {count}, IsActive: {isActive}, Botão interagível: {bonusUIMapping.bonusButton?.interactable}");
            }

            if (bonusUIMapping.bonusFirestoreName == "listCompletionBonus")
            {
                Debug.Log($"List Completion Bonus - Count: {count}, IsActive: {isActive}, Botão interagível: {bonusUIMapping.bonusButton?.interactable}");
            }

            if (isActive)
            {
                SetupButtonAction(bonusUIMapping, matchingBonus);
            }
        }

        if (bonuses.Any(b => b.BonusName == "specialBonus" && b.BonusCount >= 5 && !b.IsBonusActive))
        {
            var specialBonus = bonuses.FirstOrDefault(b => b.BonusName == "specialBonus");
            if (specialBonus != null)
            {
                specialBonus.IsBonusActive = true;
                _ = userBonusManager.SaveBonusList(userId, bonuses);
            }
        }

        if (bonuses.Any(b => b.BonusName == "listCompletionBonus" && b.BonusCount > 0 && !b.IsBonusActive))
        {
            var listBonus = bonuses.FirstOrDefault(b => b.BonusName == "listCompletionBonus");
            if (listBonus != null)
            {
                listBonus.IsBonusActive = true;
                _ = userBonusManager.SaveBonusList(userId, bonuses);
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
                        Color customColor = bonusUI.customColors.normalColor;
                        customColor.a = 1.0f;
                        buttonImage.color = customColor;
                    }
                    else
                    {
                        Color color = buttonImage.color;
                        color.a = 1.0f;
                        buttonImage.color = color;
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
                        Color color = buttonImage.color;
                        color.a = inactiveAlpha;
                        buttonImage.color = color;
                    }
                }
            }

            if (bonusUI.bonusCountText != null)
            {
                Color textColor = bonusUI.bonusCountText.color;
                textColor.a = isActive ? 1f : 0.8f;
                bonusUI.bonusCountText.color = textColor;
            }

            if (bonusUI.isBonusActiveText != null)
            {
                Color statusColor = bonusUI.isBonusActiveText.color;
                statusColor.a = isActive ? 1f : 0.8f;
                bonusUI.isBonusActiveText.color = statusColor;
            }

            if (isActive && bonusUI.bonusFirestoreName == "specialBonus")
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(bonusUI.bonusButton.GetComponent<RectTransform>());
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

    public void UseBonusAction(string bonusType)
    {
        BonusUIElements bonusUI = bonusUIMappings.FirstOrDefault(b => b.bonusFirestoreName == bonusType);
        if (bonusUI != null && bonusUI.bonusButton != null)
        {
            bonusUI.bonusButton.interactable = false;
        }

        try
        {
            switch (bonusType)
            {
                case "specialBonus":
                    Debug.Log($"cliquei no bonus specialBonus {bonusType}");
                    ShowSpecialBonusConfirmation();
                    break;

                case "listCompletionBonus":
                    Debug.Log($"cliquei no bonus listCompletionBonus {bonusType}");
                    ShowListCompletionBonusConfirmation();
                    break;

                // ... outros casos ...

                default:
                    Debug.LogWarning($"Tipo de bônus não implementado: {bonusType}");
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao usar bônus: {e.Message}");

            if (bonusUI != null && bonusUI.bonusButton != null)
            {
                bonusUI.bonusButton.interactable = true;
            }
        }
    }

    private void ShowSpecialBonusConfirmation()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        HalfViewComponent halfView = HalfViewRegistry.GetHalfViewForScene(currentScene);

        if (halfView == null)
        {
            halfView = HalfViewRegistry.EnsureHalfViewInCurrentScene();
        }

        if (halfView != null)
        {
            halfView.HideMenu();
            StartCoroutine(ConfigureHalfViewAfterFrame(halfView));
        }
        else
        {
            Debug.LogError("Não foi possível criar o HalfViewComponent. Ativando Special Bonus diretamente.");
            _ = ActivateSpecialBonus();
        }
    }

    private void ShowListCompletionBonusConfirmation()
    {

        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log($"Aqui está ativa a cena: {currentScene} ");
        HalfViewComponent halfView = HalfViewRegistry.GetHalfViewForScene(currentScene);
        Debug.Log($"Aqui está ativa a halvView: {halfView} ");

        if (halfView == null)
        {
            halfView = HalfViewRegistry.EnsureHalfViewInCurrentScene();
        }

        if (halfView != null)
        {
            halfView.HideMenu();
            StartCoroutine(ConfigureListBonusHalfViewAfterFrame(halfView));
        }
        else
        {
            Debug.LogError("Não foi possível criar o HalfViewComponent. Ativando List Completion Bonus diretamente.");
            _ = ActivateListCompletionBonus();
        }
    }

    private IEnumerator ConfigureListBonusHalfViewAfterFrame(HalfViewComponent halfView)
    {
        yield return null;
        Debug.Log("ConfigureListBonusHalfViewAfterFrame: Iniciando configuração");

        HalfViewButtonsHelper existingHelper = halfView.gameObject.GetComponent<HalfViewButtonsHelper>();
        if (existingHelper != null)
        {
            Destroy(existingHelper); // Remover completamente o componente
            Debug.Log("HalfViewButtonsHelper existente removido");
        }

        // 2. Marque a halfView para impedir a reconfiguração automática
        var preventReconfigField = typeof(HalfViewComponent).GetField("preventButtonReconfiguration",
                                                                     System.Reflection.BindingFlags.NonPublic |
                                                                     System.Reflection.BindingFlags.Instance);
        if (preventReconfigField != null)
        {
            preventReconfigField.SetValue(halfView, true);
            Debug.Log("preventButtonReconfiguration definido como true");
        }

        // 3. Configurar a UI
        halfView.OnCancelled -= OnHalfViewCancelled;
        halfView.OnCancelled += OnHalfViewCancelled;
        halfView.SetTitle("Ativar Bonus das Listas");
        halfView.SetMessage("Você terá xp duplicada por 10 min.\nPoderá ser cumulativo se já existir um bonus em uso.\nDeseja ativar o bonus agora?");

        // 4. Configurar os botões usando SetPrimaryButton e SetSecondaryButton
        halfView.SetPrimaryButton("Cancelar", () =>
        {
            Debug.Log("Botão Cancelar clicado para List Completion Bonus");
            CancelListCompletionBonusFromButton();
        });

        halfView.SetSecondaryButton("Ativar Bonus", () =>
        {
            Debug.Log("Botão Ativar clicado para List Completion Bonus");
            ActivateListCompletionBonusFromButton();
        });

        // 5. Mostrar o menu
        halfView.ShowMenu();
    }

    public async void ActivateSpecialBonusFromButton()
    {
        Debug.Log("Iniciando ativação do Special Bonus via Helper");
        try
        {
            await ActivateSpecialBonus();
            await FetchBonuses();
            Debug.Log("Special Bonus ativado com sucesso");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao ativar Special Bonus: {e.Message}");
            ForceUpdateUIAndReactivateButton();
        }
    }

    public void CancelSpecialBonusFromButton()
    {
        Debug.Log("Cancelando ativação do Special Bonus via Helper");
        ForceUpdateUIAndReactivateButton();
    }

    private async Task ActivateListCompletionBonus()
    {
        Debug.Log("ActivateListCompletionBonus: Método iniciado");
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("BonusSceneManager: UserId não definido");
            return;
        }

        try
        {
            Debug.Log("Buscando lista de bônus do usuário");

            List<BonusType> bonusList = await userBonusManager.GetUserBonuses(userId);

            Debug.Log($"Total de bônus disponíveis: {bonusList.Count}");

            foreach (var bonus in bonusList)
            {
                Debug.Log($"Bônus encontrado: {bonus.BonusName}, Count: {bonus.BonusCount}, Ativo: {bonus.IsBonusActive}");
            }

            BonusType listBonus = bonusList.FirstOrDefault(b => b.BonusName == "listCompletionBonus");

            if (listBonus != null)
            {
                Debug.Log($"List Completion Bonus encontrado. Count: {listBonus.BonusCount}, Ativo: {listBonus.IsBonusActive}");

                if (listBonus.BonusCount > 0)
                {

                    // Decrementar o contador em 1, consumindo apenas um bônus por vez
                    listBonus.BonusCount--;

                    // Se ainda tiver bônus sobrando, mantemos como ativo
                    listBonus.IsBonusActive = listBonus.BonusCount > 0;

                    await userBonusManager.SaveBonusList(userId, bonusList);

                    QuestionSceneBonusManager questionSceneBonusManager = new QuestionSceneBonusManager();
                    // Bonus das Listas: dura 10 minutos e dá 2x de multiplicador
                    await questionSceneBonusManager.ActivateBonus(userId, "listCompletionBonus", 600f, 2);
                    await FetchBonuses();

                    Debug.Log($"List Completion Bonus ativado. Bônus restantes: {listBonus.BonusCount}");
                }
                else
                {
                    Debug.LogWarning($"List Completion Bonus tem contador zero: {listBonus.BonusCount}");
                }
            }
            else
            {
                Debug.LogError("List Completion Bonus não encontrado na lista de bônus");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusSceneManager: Erro ao ativar list completion bonus: {e.Message}");
        }
    }

    public async void ActivateListCompletionBonusFromButton()
    {
        Debug.Log("Iniciando ativação do List Completion Bonus via Helper");
        try
        {
            Debug.Log("Chamando método ActivateListCompletionBonus");
            await ActivateListCompletionBonus();
            await FetchBonuses();
            Debug.Log("List Completion Bonus ativado com sucesso");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao ativar List Completion Bonus: {e.Message}");
            ForceUpdateUIAndReactivateButton();
        }
    }

    public void CancelListCompletionBonusFromButton()
    {
        Debug.Log("Cancelando ativação do List Completion Bonus via Helper");
        ForceUpdateUIAndReactivateButton();
    }

    private IEnumerator ConfigureHalfViewAfterFrame(HalfViewComponent halfView)
    {
        yield return null;
        Debug.Log("ConfigureHalfViewAfterFrame: Iniciando configuração");

        // 1. Remover o HalfViewButtonsHelper existente (se houver)
        HalfViewButtonsHelper existingHelper = halfView.gameObject.GetComponent<HalfViewButtonsHelper>();
        if (existingHelper != null)
        {
            Destroy(existingHelper); // Remover completamente o componente
            Debug.Log("HalfViewButtonsHelper existente removido");
        }

        // 2. Marque a halfView para impedir a reconfiguração automática
        var preventReconfigField = typeof(HalfViewComponent).GetField("preventButtonReconfiguration",
                                                                    System.Reflection.BindingFlags.NonPublic |
                                                                    System.Reflection.BindingFlags.Instance);
        if (preventReconfigField != null)
        {
            preventReconfigField.SetValue(halfView, true);
            Debug.Log("preventButtonReconfiguration definido como true");
        }

        // 3. Configurar a UI
        halfView.OnCancelled -= OnHalfViewCancelled;
        halfView.OnCancelled += OnHalfViewCancelled;
        halfView.SetTitle("Ativar Special Bonus");
        halfView.SetMessage("Você terá xp triplicada por 10 min.\nPoderá ser cumulativo se já existir um bonus em uso.\nDeseja ativar o bonus agora?");

        // 4. Configurar os botões usando SetPrimaryButton e SetSecondaryButton
        halfView.SetPrimaryButton("Cancelar", () =>
        {
            Debug.Log("Botão Cancelar clicado para Special Bonus");
            CancelSpecialBonusFromButton();
        });

        halfView.SetSecondaryButton("Ativar Bonus", () =>
        {
            Debug.Log("Botão Ativar clicado para Special Bonus");
            ActivateSpecialBonusFromButton();
        });

        // 5. Mostrar o menu
        halfView.ShowMenu();
    }

    private void OnHalfViewCancelled()
    {
        ForceUpdateUIAndReactivateButton();
    }

    private void ForceUpdateUIAndReactivateButton()
    {
        StartCoroutine(FetchAndUpdateUI());
    }

    private IEnumerator FetchAndUpdateUI()
    {
        var fetchTask = userBonusManager.GetUserBonuses(userId);
        while (!fetchTask.IsCompleted)
        {
            yield return null;
        }

        if (fetchTask.IsFaulted || fetchTask.IsCanceled)
        {
            Debug.LogError($"Erro ao buscar dados do usuário: {fetchTask.Exception?.Message}");
            FallbackReactivateButton();
        }
        else
        {
            try
            {
                List<BonusType> bonuses = fetchTask.Result;
                UpdateBonusUI(bonuses);
                ReactivateButtonIfNeeded(bonuses);
            }
            catch (Exception e)
            {
                Debug.LogError($"Erro ao processar resultados: {e.Message}");
                FallbackReactivateButton();
            }
        }
    }

    private void FallbackReactivateButton()
    {
        BonusUIElements specialBonusUI = bonusUIMappings.FirstOrDefault(b => b.bonusFirestoreName == "specialBonus");
        if (specialBonusUI != null && specialBonusUI.bonusButton != null)
        {
            specialBonusUI.bonusButton.interactable = true;
            Debug.Log("Botão Special Bonus reativado por fallback");
        }
    }

    private void ReactivateButtonIfNeeded(List<BonusType> bonuses)
    {
        BonusType specialBonus = bonuses.FirstOrDefault(b => b.BonusName == "specialBonus");
        if (specialBonus != null && specialBonus.BonusCount >= 5)
        {
            BonusUIElements specialBonusUI = bonusUIMappings.FirstOrDefault(b => b.bonusFirestoreName == "specialBonus");
            if (specialBonusUI != null && specialBonusUI.bonusButton != null)
            {
                specialBonusUI.bonusButton.interactable = true;
                Debug.Log("Botão Special Bonus reativado após atualização de UI");
            }
        }
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
            List<BonusType> bonusList = await userBonusManager.GetUserBonuses(userId);
            BonusType specialBonus = bonusList.FirstOrDefault(b => b.BonusName == "specialBonus");

            if (specialBonus != null && specialBonus.BonusCount >= 5)
            {
                specialBonus.BonusCount = 0;
                specialBonus.IsBonusActive = false;
                await userBonusManager.SaveBonusList(userId, bonusList);
                QuestionSceneBonusManager questionSceneBonusManager = new QuestionSceneBonusManager();
                await questionSceneBonusManager.ActivateBonus(userId, "specialBonus", 600f, 3);
                await FetchBonuses();
            }
            else
            {
                Debug.LogWarning("Special Bonus não está disponível para ativação");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusSceneManager: Erro ao ativar special bonus: {e.Message}");
        }
    }

    private void StartListeningForBonusUpdates()
    {
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