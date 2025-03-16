using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private FeedbackManager feedbackManager;

    private FirebaseAuth auth;
    private Dictionary<string, LoginAttempt> loginAttempts = new Dictionary<string, LoginAttempt>();
    private const int MAX_ATTEMPTS = 5;
    private const int LOCKOUT_MINUTES = 15;
    private bool isProcessing = false;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        loginButton.onClick.AddListener(HandleLogin);
        registerButton.onClick.AddListener(HandleRegisterNavigation);
        InvokeRepeating(nameof(CleanupOldAttempts), 0f, 86400f);
    }

    public async void HandleLogin()
    {
        if (isProcessing)
        {
            return;
        }

        if (string.IsNullOrEmpty(emailInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            feedbackManager.ShowFeedback("Email e senha são obrigatórios.", true);
            return;
        }

        string email = emailInput.text.Trim().ToLower();

        if (!CheckRateLimit(email))
        {
            return;
        }

        isProcessing = true;
        SetButtonsInteractable(false);
        feedbackManager.ShowFeedback("", false);
        LoadingSpinnerComponent.Instance.ShowSpinner();

        try
        {
            if (AuthenticationRepository.Instance == null)
            {
                throw new Exception("NovoFirebaseManager não está inicializado");
            }

            var userData = await AuthenticationRepository.Instance.SignInWithEmailAsync(email, passwordInput.text);

            if (userData == null)
            {
                throw new Exception("Login falhou: dados do usuário nulos");
            }

            if (loginAttempts.ContainsKey(email))
            {
                loginAttempts[email].Reset();
            }

            UserDataStore.CurrentUserData = userData;
            await AnsweredQuestionsManager.Instance.ForceUpdate();
            LoadingSpinnerComponent.Instance.ShowSpinnerUntilSceneLoaded("PathwayScene");
            SceneManager.LoadScene("PathwayScene");
        }
        catch (FirebaseException e)
        {
            if (!loginAttempts.ContainsKey(email))
            {
                loginAttempts[email] = new LoginAttempt();
            }
            loginAttempts[email].IncrementAttempt();
            feedbackManager.ShowFeedback($"{GetFirebaseAuthErrorMessage(e)}", true);
            Debug.LogError($"{e.ErrorCode}, Message: {e.Message}");
            LoadingSpinnerComponent.Instance.HideSpinner();
            SetButtonsInteractable(true);
            isProcessing = false;
        }
        catch (Exception e)
        {
            feedbackManager.ShowFeedback($"{e.Message}", true);
            Debug.LogError($"{e.Message}");
            LoadingSpinnerComponent.Instance.HideSpinner();
            SetButtonsInteractable(true);
            isProcessing = false;
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        loginButton.interactable = interactable;
        registerButton.interactable = interactable;

        // Opcional: desabilitar também os campos de entrada
        emailInput.interactable = interactable;
        passwordInput.interactable = interactable;
    }

    private string GetFirebaseAuthErrorMessage(FirebaseException e)
    {
        if (e is FirebaseException authException)
        {
            var errorCode = (int)authException.ErrorCode;
            switch (errorCode)
            {
                case (int)AuthError.UserNotFound:
                    return "Usuário não encontrado.";
                case (int)AuthError.WrongPassword:
                    return "Email e/ou Senha incorretos.";
                case (int)AuthError.InvalidEmail:
                    return "Email inválido.";
                case (int)AuthError.NetworkRequestFailed:
                    return "Falha na conexão com a internet.";
                default:
                    return $"{authException.Message}";
            }
        }
        return $"Ocorreu um erro: {e.Message}";
    }

    public void HandleRegisterNavigation()
    {
        LoadingSpinnerComponent.Instance.ShowSpinnerUntilSceneLoaded("RegisterView");
        SceneManager.LoadScene("RegisterView");
    }

    private class LoginAttempt
    {
        public int Attempts { get; set; }
        public DateTime LastAttempt { get; set; }
        public DateTime? LockoutUntil { get; set; }

        public bool IsLockedOut => LockoutUntil.HasValue && DateTime.Now < LockoutUntil.Value;

        public void IncrementAttempt()
        {
            Attempts++;
            LastAttempt = DateTime.Now;

            if (Attempts >= MAX_ATTEMPTS)
            {
                LockoutUntil = DateTime.Now.AddMinutes(LOCKOUT_MINUTES);
            }
        }

        public void Reset()
        {
            Attempts = 0;
            LockoutUntil = null;
        }
    }

    private void CleanupOldAttempts()
    {
        var now = DateTime.Now;
        var keysToRemove = new List<string>();

        foreach (var kvp in loginAttempts)
        {
            if ((now - kvp.Value.LastAttempt).TotalHours > 24)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            loginAttempts.Remove(key);
        }
    }

    private bool CheckRateLimit(string email)
    {
        if (!loginAttempts.ContainsKey(email))
        {
            loginAttempts[email] = new LoginAttempt();
        }

        var attempt = loginAttempts[email];

        if (attempt.IsLockedOut)
        {
            var remainingMinutes = Math.Ceiling((attempt.LockoutUntil.Value - DateTime.Now).TotalMinutes);
            feedbackManager.ShowFeedback(
                $"Tente novamente em {remainingMinutes} minutos.",
                true
            );
            return false;
        }

        return true;
    }
}