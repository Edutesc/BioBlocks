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
        
        // Obter os dados mais recentes do usuário
        UserData userData = await FirestoreRepository.Instance.GetUserData(userId);

        if (userData == null)
        {
            Debug.LogError("Dados do usuário não encontrados");
            return;
        }

        // Se a resposta estiver correta, atualizar o score E marcar a questão
        if (isCorrect)
        {
            string databankName = answeredQuestion.questionDatabankName;
            int questionNumber = answeredQuestion.questionNumber;

            Debug.Log($"Atualizando score em {scoreChange} pontos e marcando questão {questionNumber} no banco {databankName}");
            
            // Usar o novo método UpdateUserScores que atualiza tanto o Score quanto o WeekScore
            try
            {
                await FirestoreRepository.Instance.UpdateUserScores(
                    userId, 
                    scoreChange,   // Passamos o incremento, não o novo valor total
                    questionNumber, 
                    databankName, 
                    true
                );
                
                Debug.Log($"Score e WeekScore incrementados em {scoreChange} e questão {questionNumber} marcada como respondida");
                
                // Forçar atualização dos dados de questões respondidas na UI
                if (answeredQuestionsManager != null && answeredQuestionsManager.IsManagerInitialized)
                {
                    await answeredQuestionsManager.ForceUpdate();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Falha ao atualizar scores e marcar questão como respondida: {ex.Message}");
            }
        }
        else
        {
            // Se a resposta estiver errada, apenas atualizar o score
            Debug.Log($"Resposta incorreta. Atualizando o score em {scoreChange} pontos");
            try
            {
                await FirestoreRepository.Instance.UpdateUserScores(
                    userId, 
                    scoreChange, // Incremento no score, não o novo valor total
                    0,           // Não marca nenhuma questão como respondida
                    "",          // Nenhum banco de dados
                    false        // Não é para marcar questão
                );
            }
            catch (Exception ex)
            {
                Debug.LogError($"Falha ao atualizar o score no Firestore: {ex.Message}");
            }
        }

        // Após a atualização no Firestore, buscar os dados atualizados
        // para garantir que temos o estado mais recente
        UserData updatedUserData = await FirestoreRepository.Instance.GetUserData(userId);
        
        if (updatedUserData != null)
        {
            // Atualizar o UserDataStore local com os dados completos e atualizados
            UserDataStore.CurrentUserData = updatedUserData;
            Debug.Log($"UserData atualizado com sucesso. Novo score: {updatedUserData.Score}, WeekScore: {updatedUserData.WeekScore}");
        }
    }
    catch (Exception ex)
    {
        Debug.LogError($"Erro ao atualizar score: {ex.Message}\n{ex.StackTrace}");
        
        // Mesmo em caso de erro, tentar atualizar o UserDataStore local
        // para manter uma experiência mais consistente para o usuário
        if (currentUserData != null && scoreChange != 0)
        {
            int clientSideScore = currentUserData.Score + scoreChange;
            int clientSideWeekScore = currentUserData.WeekScore + scoreChange;
            
            currentUserData.Score = clientSideScore;
            currentUserData.WeekScore = clientSideWeekScore;
            UserDataStore.CurrentUserData = currentUserData;
            
            Debug.Log($"Atualização local dos scores após erro - Score: {clientSideScore}, WeekScore: {clientSideWeekScore}");
        }
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