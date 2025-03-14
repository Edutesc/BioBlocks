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

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.Log("Canvas não encontrado no parent, adicionando ao gameObject atual");
            canvas = gameObject.AddComponent<Canvas>();
        }

        canvas.overrideSorting = true;
        canvas.sortingOrder = 1000;

        GraphicRaycaster raycaster = GetComponentInParent<GraphicRaycaster>();
        if (raycaster == null)
        {
            Debug.Log("GraphicRaycaster não encontrado, adicionando um");
            raycaster = gameObject.AddComponent<GraphicRaycaster>();
        }

        Debug.Log($"ReAuthenticationUI inicializado com Canvas.sortingOrder={canvas.sortingOrder}");

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

        // Verificar e configurar o Canvas novamente quando o painel é mostrado
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000; // Garantir que esteja no topo
            Debug.Log($"Canvas sortingOrder definido para {canvas.sortingOrder}");
        }
        else
        {
            Debug.LogWarning("Canvas não encontrado ao mostrar ReAuthPanel!");
        }

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

        // Verificar se os botões estão interativos
        if (authenticateButton != null)
        {
            authenticateButton.interactable = true;
            Debug.Log("Botão authenticate definido como interativo");
        }

        if (cancelButton != null)
        {
            cancelButton.interactable = true;
            Debug.Log("Botão cancel definido como interativo");
        }

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
        Debug.Log("OnAuthenticateClick chamado");

        if (passwordInput == null)
        {
            Debug.LogError("passwordInput é null!");
            return;
        }

        if (string.IsNullOrEmpty(passwordInput.text))
        {
            if (errorText != null)
            {
                errorText.text = "Por favor, insira sua senha";
            }
            return;
        }

        try
        {
            if (authenticateButton != null) authenticateButton.interactable = false;
            if (authenticateButtonText != null) authenticateButtonText.text = "Autenticando...";
            if (errorText != null) errorText.text = "";

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
            if (errorText != null) errorText.text = "Senha incorreta. Por favor, tente novamente.";
            if (authenticateButton != null) authenticateButton.interactable = true;
            if (authenticateButtonText != null) authenticateButtonText.text = "Confirmar";
        }
    }

    public void OnCancelClick()
    {
        Debug.Log("OnCancelClick chamado");
        HideReAuthPanel();

        // Restaurar estado original
        GameObject deleteAccountDarkOverlay = GameObject.Find("DeleteAccountDarkOverlay");
        if (deleteAccountDarkOverlay != null)
        {
            Canvas overlayCanvas = deleteAccountDarkOverlay.GetComponent<Canvas>();
            if (overlayCanvas != null)
            {
                overlayCanvas.sortingOrder = 109; // Restaurar para o valor original
            }
        }
    }

    private void OnDestroy()
    {
        if (authenticateButton != null)
            authenticateButton.onClick.RemoveListener(OnAuthenticateClick);

        if (cancelButton != null)
            cancelButton.onClick.RemoveListener(OnCancelClick);
    }
}
