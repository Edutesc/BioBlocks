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

            // Calcular o novo score baseado no valor ATUAL do Firestore
            int newScore = userData.Score + scoreChange;
            Debug.Log($"Atualizando score: {userData.Score} + {scoreChange} = {newScore}");

            // Se a resposta estiver correta, atualizar o score E marcar a questão
            if (isCorrect)
            {
                string databankName = answeredQuestion.questionDatabankName;
                int questionNumber = answeredQuestion.questionNumber;

                Debug.Log($"Atualizando score para {newScore} e marcando questão {questionNumber} no banco {databankName}");
                
                // Atualizar diretamente usando o FirestoreRepository
                // Isso atualiza tanto o score quanto marca a questão como respondida
                try
                {
                    await FirestoreRepository.Instance.UpdateUserScore(
                        userId, 
                        newScore,  // Importante: Passamos o novo score
                        questionNumber, 
                        databankName, 
                        true
                    );
                    
                    Debug.Log($"Score atualizado para {newScore} e questão {questionNumber} marcada como respondida");
                    
                    // Forçar atualização dos dados de questões respondidas na UI
                    if (answeredQuestionsManager != null && answeredQuestionsManager.IsManagerInitialized)
                    {
                        await answeredQuestionsManager.ForceUpdate();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Falha ao atualizar score e marcar questão como respondida: {ex.Message}");
                }
            }
            else
            {
                // Se a resposta estiver errada, apenas atualizar o score
                Debug.Log($"Resposta incorreta. Atualizando apenas o score para {newScore}");
                try
                {
                    await FirestoreRepository.Instance.UpdateUserScore(
                        userId, 
                        newScore,  
                        0,         // Não marca nenhuma questão como respondida
                        "",        // Nenhum banco de dados
                        false      // Não é para marcar questão
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
                Debug.Log($"UserData atualizado com sucesso. Novo score: {updatedUserData.Score}");
            }
            else
            {
                // Fallback: atualizar apenas o score localmente
                UserDataStore.UpdateScore(newScore);
                Debug.Log($"Não foi possível obter UserData atualizado. Score local atualizado para {newScore}");
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
                UserDataStore.UpdateScore(clientSideScore);
                Debug.Log($"Atualização local do score para {clientSideScore} após erro");
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