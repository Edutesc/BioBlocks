using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField nickNameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private GameObject logoLoading;
    [SerializeField] private FeedbackManager feedbackManager;

    [Header("Loading Spinner Configuration")]
    [SerializeField] private float spinnerRotationSpeed = 100f;    
    [SerializeField] private Image loadingSpinner;

    private FirebaseFirestore db;

    private void Start()
    {
        logoLoading.SetActive(false);
        db = FirebaseFirestore.DefaultInstance;
        nickNameInput.contentType = TMP_InputField.ContentType.Standard;
        nickNameInput.characterLimit = 15;
        nickNameInput.onValueChanged.AddListener(ValidateNickname);
        registerButton.onClick.AddListener(HandleRegistration);
    }

        private void Update()
    {
        // Rotacionar o spinner
        if (loadingSpinner != null && logoLoading.activeSelf)
        {
            loadingSpinner.transform.Rotate(0f, 0f, -spinnerRotationSpeed * Time.deltaTime);
        }
    }

    public async void HandleRegistration()
    {
        logoLoading.SetActive(true);

        try
        {
            if (AuthenticationRepository.Instance == null)
            {
                throw new Exception("NovoFirebaseManager não está inicializado");
            }

            if (string.IsNullOrEmpty(nickNameInput.text) || string.IsNullOrEmpty(nameInput.text) || string.IsNullOrEmpty(emailInput.text) || string.IsNullOrEmpty(passwordInput.text))
            {
                throw new Exception("Todos os campos são obrigatório.");
            }

            string nicknameToUse = nickNameInput.text;

            bool nicknameExists = await CheckNicknameExistsAsync(nicknameToUse);
            if (nicknameExists)
            {
                throw new Exception("Este nickname já está em uso. Por favor, escolha outro.");
            }

            await AuthenticationRepository.Instance.RegisterUserAsync(nameInput.text, nickNameInput.text, emailInput.text, passwordInput.text);
            SceneManager.LoadScene("PathwayScene");
        }
        catch (FirebaseException e)
        {
            string errorMessage = GetFirebaseAuthErrorMessage(e);
            feedbackManager.ShowFeedback(errorMessage, true);
            Debug.LogError($"{errorMessage}");
        }
        catch (Exception e)
        {
            string errorMessage = $"{e.Message}";
            feedbackManager.ShowFeedback(errorMessage, true);
            Debug.LogError(errorMessage);
        }
        finally
        {
            logoLoading.SetActive(false);
        }
    }

    private void ValidateNickname(string value)
    {
        if (value.Length < 3)
        {
            string errorMessage = "Nickname deve possuir mais de 3 caracteres.";
            feedbackManager.ShowFeedback(errorMessage, true);
        }
        else
        {
            feedbackManager.HideFeedback();
        }
    }

    private async Task<bool> CheckNicknameExistsAsync(string nickname)
    {
        try
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create();
            }
            Query query = db.Collection("Users").WhereEqualTo("nickName", nickname).Limit(1);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            return snapshot.Count > 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao verificar nickname: {e.Message}");
            throw;
        }
    }

    private string GetFirebaseAuthErrorMessage(FirebaseException e)
    {
        if (e is FirebaseException authException)
        {
            var errorCode = (int)authException.ErrorCode;
            switch (errorCode)
            {
                case (int)AuthError.EmailAlreadyInUse:
                    return "Email já registrado.";
                case (int)AuthError.WeakPassword:
                    return "Senha muito fraca.";
                default:
                    return $"{e.Message}";
            }
        }
        return $"Ocorreu um erro: {e.Message}";
    }

    public void SceneLoader()
    {
        SceneManager.LoadScene("LoginView");
    }

}