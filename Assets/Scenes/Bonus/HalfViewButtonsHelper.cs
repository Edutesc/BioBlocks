using UnityEngine;

public class HalfViewButtonsHelper : MonoBehaviour
{
    private BonusSceneManager bonusManager;
    private HalfViewComponent halfView;
    private string bonusType = "specialBonus"; // Valor padrão para manter compatibilidade

    public void Initialize(BonusSceneManager manager)
    {
        Initialize(manager, "specialBonus");
    }

    public void Initialize(BonusSceneManager manager, string type)
    {
        bonusManager = manager;
        string oldType = bonusType;
        bonusType = type;
        Debug.Log($"HalfViewButtonsHelper.Initialize: Mudando tipo de '{oldType}' para '{type}'");

        halfView = GetComponent<HalfViewComponent>();


        if (halfView != null)
        {
            if (halfView.PrimaryButton != null)
            {
                halfView.PrimaryButton.onClick.RemoveAllListeners();
                halfView.PrimaryButton.onClick.AddListener(OnPrimaryButtonClick);
            }

            if (halfView.SecondaryButton != null)
            {
                halfView.SecondaryButton.onClick.RemoveAllListeners();
                halfView.SecondaryButton.onClick.AddListener(OnSecondaryButtonClick);
            }

            if (halfView.CloseButton != null)
            {
                halfView.CloseButton.onClick.RemoveAllListeners();
                halfView.CloseButton.onClick.AddListener(OnCloseButtonClick);
            }
        }
        else
        {
            Debug.LogError("HalfViewComponent não encontrado no mesmo GameObject");
        }
    }

    public void OnPrimaryButtonClick()
    {
        Debug.Log($"Botão primário clicado via Helper para o tipo: {bonusType}");
        if (bonusManager != null)
        {
            switch (bonusType)
            {
                case "specialBonus":
                    bonusManager.CancelSpecialBonusFromButton();
                    break;
                case "listCompletionBonus":
                    bonusManager.CancelListCompletionBonusFromButton();
                    break;
                default:
                    Debug.LogWarning($"Tipo de bônus não implementado: {bonusType}");
                    break;
            }
        }
        else
        {
            Debug.LogError("BonusSceneManager não atribuído no HalfViewButtonsHelper");
        }
    }

    public void OnSecondaryButtonClick()
    {
        Debug.Log($"Botão secundário clicado via Helper para o tipo: {bonusType}");
        if (bonusManager != null)
        {
            switch (bonusType)
            {
                case "specialBonus":
                    bonusManager.ActivateSpecialBonusFromButton();
                    break;
                case "listCompletionBonus":
                    bonusManager.ActivateListCompletionBonusFromButton();
                    break;
                default:
                    Debug.LogWarning($"Tipo de bônus não implementado: {bonusType}");
                    break;
            }
        }
        else
        {
            Debug.LogError("BonusSceneManager não atribuído no HalfViewButtonsHelper");
        }
    }

    public void OnCloseButtonClick()
    {
        Debug.Log($"Botão de fechar clicado via Helper para o tipo: {bonusType}");
        if (bonusManager != null)
        {
            switch (bonusType)
            {
                case "specialBonus":
                    bonusManager.CancelSpecialBonusFromButton();
                    break;
                case "listCompletionBonus":
                    bonusManager.CancelListCompletionBonusFromButton();
                    break;
                default:
                    Debug.LogWarning($"Tipo de bônus não implementado: {bonusType}");
                    break;
            }
        }
        else
        {
            Debug.LogError("BonusSceneManager não atribuído no HalfViewButtonsHelper");
        }
    }

    public string GetBonusType()
    {
        return bonusType;
    }

}