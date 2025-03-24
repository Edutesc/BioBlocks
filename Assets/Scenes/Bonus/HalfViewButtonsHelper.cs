using UnityEngine;

public class HalfViewButtonsHelper : MonoBehaviour
{
    private BonusSceneManager bonusManager;
    private HalfViewComponent halfView;
    
    public void Initialize(BonusSceneManager manager)
    {
        bonusManager = manager;
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
        Debug.Log("Botão primário clicado via Helper");
        if (bonusManager != null)
        {
            bonusManager.CancelSpecialBonusFromButton();
        }
        else
        {
            Debug.LogError("BonusSceneManager não atribuído no HalfViewButtonsHelper");
        }
    }

    public void OnSecondaryButtonClick()
    {
        Debug.Log("Botão secundário clicado via Helper");
        if (bonusManager != null)
        {
            bonusManager.ActivateSpecialBonusFromButton();
        }
        else
        {
            Debug.LogError("BonusSceneManager não atribuído no HalfViewButtonsHelper");
        }
    }
    
    public void OnCloseButtonClick()
    {
        Debug.Log("Botão de fechar clicado via Helper");
        if (bonusManager != null)
        {
            bonusManager.CancelSpecialBonusFromButton();
        }
        else
        {
            Debug.LogError("BonusSceneManager não atribuído no HalfViewButtonsHelper");
        }
    }
}