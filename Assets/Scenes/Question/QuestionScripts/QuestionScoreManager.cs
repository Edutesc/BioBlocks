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

    public async Task UpdateScore(int scoreToAdd, bool isCorrect, Question question)
    {
        if (currentUserData == null)
        {
            Debug.LogError("Erro ao atualizar score: CurrentUserData é null");
            return;
        }

        try
        {
            int newScore = currentUserData.Score + scoreToAdd;

            // Atualiza no Firestore
            await FirestoreRepository.Instance.UpdateUserScore(
                currentUserData.UserId,
                newScore,
                question.questionNumber,
                question.questionDatabankName,
                isCorrect
            );

            // Atualiza o score no UserDataStore
            UserDataStore.UpdateScore(newScore);

            // Força atualização do AnsweredQuestionsManager
            if (answeredQuestionsManager != null)
            {
                await answeredQuestionsManager.ForceUpdate();
            }

            Debug.Log($"Score e questões respondidas atualizados com sucesso para usuário {currentUserData.NickName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao atualizar score: {e.Message}\nStackTrace: {e.StackTrace}");
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