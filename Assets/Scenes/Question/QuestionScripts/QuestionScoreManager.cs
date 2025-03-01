using UnityEngine;
using System.Threading.Tasks;
using QuestionSystem;
using System;

public class QuestionScoreManager : MonoBehaviour
{
    private UserData currentUserData;
    private AnsweredQuestionsManager answeredQuestionsManager;

    private void Start()
    {
        currentUserData = UserDataStore.CurrentUserData;
        answeredQuestionsManager = AnsweredQuestionsManager.Instance;

        if (currentUserData == null)
        {
            Debug.LogError("CurrentUserData é null no ScoreManager");
        }

        if (answeredQuestionsManager == null)
        {
            Debug.LogError("AnsweredQuestionsManager não encontrado");
        }
    }

    public async Task UpdateScore(int scoreChange, bool isCorrect, Question answeredQuestion)
    {
        try
        {
            if (AuthenticationRepository.Instance.Auth.CurrentUser == null)
            {
                Debug.LogError("Usuário não autenticado");
                return;
            }

            string userId = AuthenticationRepository.Instance.Auth.CurrentUser.UserId;
            UserData userData = await FirestoreRepository.Instance.GetUserData(userId);

            if (userData == null)
            {
                Debug.LogError("Dados do usuário não encontrados");
                return;
            }

            // Atualiza o score
            int newScore = userData.Score + scoreChange;

            // Se a resposta estiver correta, marca a questão como respondida usando o método centralizado
            if (isCorrect)
            {
                string databankName = answeredQuestion.questionDatabankName;
                int questionNumber = answeredQuestion.questionNumber;

                // Usa o novo método centralizado para marcar questões respondidas
                if (AnsweredQuestionsManager.Instance != null)
                {
                    // Este método já usa UpdateUserScore internamente
                    await AnsweredQuestionsManager.Instance.MarkQuestionAsAnswered(databankName, questionNumber);
                    Debug.Log($"Questão {questionNumber} do banco {databankName} marcada como respondida via AnsweredQuestionsManager");
                }
                else
                {
                    // Fallback se o AnsweredQuestionsManager não estiver disponível
                    Debug.LogWarning("AnsweredQuestionsManager não está disponível, usando UpdateUserScore diretamente");
                    await FirestoreRepository.Instance.UpdateUserScore(userId, newScore, questionNumber, databankName, true);
                }
            }
            else
            {
                // Se a resposta estiver errada, apenas atualiza o score
                await FirestoreRepository.Instance.UpdateUserScore(userId, newScore, 0, "", false);
            }

            // Atualiza o UserDataStore local
            UserDataStore.UpdateScore(newScore);

            Debug.Log($"Score atualizado: {newScore} (alteração: {scoreChange}), isCorrect: {isCorrect}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao atualizar score: {ex.Message}\n{ex.StackTrace}");
        }
    }

    private void OnEnable()
    {
        UserDataStore.OnUserDataChanged += OnUserDataChanged;
    }

    private void OnDisable()
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
    }

    private void OnUserDataChanged(UserData userData)
    {
        currentUserData = userData;
        Debug.Log($"ScoreManager: UserData atualizado - UserId: {userData.UserId}, Score: {userData.Score}");
    }
}