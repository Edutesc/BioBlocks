using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReAuthenticationUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private CanvasGroup reAuthCanvasGroup;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button authenticateButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI authenticateButtonText;

    [Header("References")]
    [SerializeField] private CanvasGroup deleteAccountCanvasGroup;

    private System.Action onReauthenticationSuccess;

    private void Awake()
    {
        HideReAuthPanel();
    }

    private void Start()
    {
        Debug.Log("ReAuthenticationUI inicializado");

        if (reAuthCanvasGroup == null)
        {
            Debug.LogError("reAuthCanvasGroup não está configurado!");
            return;
        }

        if (authenticateButton != null)
        {
            authenticateButton.onClick.AddListener(OnAuthenticateClick);
            Debug.Log("Botão de autenticação configurado");
        }
        else
        {
            Debug.LogError("Botão de autenticação não encontrado!");
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(OnCancelClick);
            Debug.Log("Botão de cancelamento configurado");
        }
        else
        {
            Debug.LogError("Botão de cancelamento não encontrado!");
        }
    }

    public void ShowReAuthPanel(string userEmail, System.Action onSuccess)
    {
        Debug.Log($"ShowReAuthPanel chamado para email: {userEmail}");
        onReauthenticationSuccess = onSuccess;

        // Configurar UI
        if (emailInput != null)
        {
            emailInput.text = userEmail;
            emailInput.interactable = false;
        }

        if (passwordInput != null)
        {
            passwordInput.text = "";
            passwordInput.interactable = true;
        }

        if (errorText != null)
        {
            errorText.text = "";
        }

        if (authenticateButtonText != null)
        {
            authenticateButtonText.text = "Confirmar";
        }

        // Mostrar painel
        reAuthCanvasGroup.alpha = 1;
        reAuthCanvasGroup.interactable = true;
        reAuthCanvasGroup.blocksRaycasts = true;

        // Focar no campo de senha
        if (passwordInput != null)
        {
            passwordInput.Select();
            passwordInput.ActivateInputField();
        }

        Debug.Log("Painel de reautenticação mostrado");
    }

    private void HideReAuthPanel()
    {
        if (reAuthCanvasGroup != null)
        {
            reAuthCanvasGroup.alpha = 0;
            reAuthCanvasGroup.interactable = false;
            reAuthCanvasGroup.blocksRaycasts = false;
        }
    }

    public async void OnAuthenticateClick()
    {
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            errorText.text = "Por favor, insira sua senha";
            return;
        }

        try
        {
            authenticateButton.interactable = false;
            authenticateButtonText.text = "Autenticando...";
            errorText.text = "";

            await AuthenticationRepository.Instance.ReauthenticateUser(emailInput.text, passwordInput.text);
            Debug.Log("Reautenticação bem-sucedida");

            if (onReauthenticationSuccess != null)
            {
                onReauthenticationSuccess.Invoke();
            }

            HideReAuthPanel();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro na reautenticação: {ex.Message}");
            errorText.text = "Senha incorreta. Por favor, tente novamente.";
            authenticateButton.interactable = true;
            authenticateButtonText.text = "Confirmar";
        }
    }

    public void OnCancelClick()
    {
        HideReAuthPanel();
        // Você pode adicionar aqui alguma lógica adicional para cancelamento
    }

    private void OnDestroy()
    {
        if (authenticateButton != null)
            authenticateButton.onClick.RemoveListener(OnAuthenticateClick);

        if (cancelButton != null)
            cancelButton.onClick.RemoveListener(OnCancelClick);
    }
}
