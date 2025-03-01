using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;

public class FirestoreRepository : MonoBehaviour
{
    private static FirestoreRepository _instance;
    private FirebaseFirestore db;
    private bool isInitialized;

    // Propriedade pública somente leitura para verificar o estado de inicialização
    public bool IsInitialized => isInitialized;

    public static FirestoreRepository Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<FirestoreRepository>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("FirestoreRepository");
                    _instance = go.AddComponent<FirestoreRepository>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public void Initialize()
    {
        if (isInitialized) return;

        try
        {
            db = FirebaseFirestore.DefaultInstance;
            isInitialized = true;
            Debug.Log("Firestore initialized successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Firestore initialization failed: {e.Message}");
            throw;
        }
    }

    public async Task<UserData> GetUserData(string userId)
    {
        try
        {
            DocumentSnapshot snapshot = await db.Collection("Users").Document(userId).GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Dictionary<string, object> userData = snapshot.ToDictionary();
                Debug.Log($"Dados brutos do Firestore: {string.Join(", ", userData.Select(kv => $"{kv.Key}: {kv.Value}"))}");

                // Tratamento correto do Timestamp
                Timestamp createdTime = Timestamp.FromDateTime(DateTime.UtcNow); // valor padrão
                if (userData["CreatedTime"] is Timestamp timestamp)
                {
                    createdTime = timestamp;
                }

                UserData user = new UserData
                {
                    UserId = userData["UserId"].ToString(),
                    NickName = userData["NickName"].ToString(),
                    Name = userData["Name"].ToString(),
                    Email = userData["Email"].ToString(),
                    Score = Convert.ToInt32(userData["Score"]),
                    Progress = Convert.ToInt32(userData["Progress"]),
                    ProfileImageUrl = userData["ProfileImageUrl"]?.ToString() ?? "",
                    CreatedTime = createdTime,
                    IsUserRegistered = Convert.ToBoolean(userData["IsUserRegistered"])
                };

                if (userData.ContainsKey("AnsweredQuestions"))
                {
                    user.AnsweredQuestions = new Dictionary<string, List<int>>();
                    var answeredQuestions = userData["AnsweredQuestions"] as Dictionary<string, object>;
                    if (answeredQuestions != null)
                    {
                        foreach (var kvp in answeredQuestions)
                        {
                            if (kvp.Value is IEnumerable<object> list)
                            {
                                user.AnsweredQuestions[kvp.Key] = list.Select(x => Convert.ToInt32(x)).ToList();
                            }
                        }
                    }
                }

                Debug.Log($"UserData carregado - NickName: {user.NickName}, Email: {user.Email}");
                return user;
            }
            else
            {
                Debug.LogError($"Documento do usuário {userId} não encontrado");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar dados do usuário: {ex.Message}");
            throw;
        }
    }

    private UserData ConvertFromFirestore(Dictionary<string, object> data)
    {
        UserData userData = new UserData();
        userData.UserId = (string)data["UserId"];
        userData.NickName = (string)data["NickName"];
        userData.Name = (string)data["Name"];
        userData.Email = (string)data["Email"];
        userData.ProfileImageUrl = (string)data["ProfileImageUrl"];
        userData.Score = Convert.ToInt32(data["Score"]);
        userData.Progress = Convert.ToInt32(data["Progress"]);
        userData.IsUserRegistered = Convert.ToBoolean(data["IsUserRegistered"]);

        if (data["CreatedTime"] is Timestamp timestamp)
        {
            userData.CreatedTime = timestamp;
        }

        userData.AnsweredQuestions = new Dictionary<string, List<int>>();
        var answeredQuestionsData = (Dictionary<string, object>)data["AnsweredQuestions"];
        foreach (var kvp in answeredQuestionsData)
        {
            string databankName = kvp.Key;
            var questionsList = (List<object>)kvp.Value;
            userData.AnsweredQuestions[databankName] = questionsList.Select(x => Convert.ToInt32(x)).ToList();
        }

        return userData;
    }

    public async Task CreateUserDocument(UserData userData)
    {
        try
        {
            if (!isInitialized) throw new System.Exception("Firestore não inicializado");

            if (string.IsNullOrEmpty(userData.UserId))
                throw new ArgumentException("UserId não pode ser vazio");

            var requiredFields = new Dictionary<string, object>
            {
                { "UserId", userData.UserId },
                { "NickName", userData.NickName },
                { "Name", userData.Name },
                { "Email", userData.Email },
                { "Score", userData.Score },
                { "Progress", userData.Progress }, // Garante que Progress está presente
                { "IsUserRegistered", userData.IsUserRegistered },
                { "CreatedTime", userData.CreatedTime }
            };

            DocumentReference docRef = db.Collection("Users").Document(userData.UserId);
            await docRef.SetAsync(requiredFields);
            Debug.Log($"Documento do usuário criado com sucesso: {userData.UserId}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao criar documento do usuário: {e.Message}");
            throw;
        }
    }

    public async Task UpdateUserScore(string userId, int newScore, int questionNumber, string databankName, bool isCorrect)
    {
        try
        {
            if (!isInitialized) throw new System.Exception("Firestore não inicializado");

            UserDataStore.UpdateScore(newScore);

            DocumentReference docRef = db.Collection("Users").Document(userId);

            await db.RunTransactionAsync(async transaction =>
            {
                DocumentSnapshot snapshot = await transaction.GetSnapshotAsync(docRef);
                Dictionary<string, object> updates = new Dictionary<string, object> { { "Score", newScore } };

                if (snapshot.Exists)
                {
                    Dictionary<string, List<int>> answeredQuestions = snapshot.GetValue<Dictionary<string, List<int>>>("AnsweredQuestions");

                    if (answeredQuestions == null)
                    {
                        answeredQuestions = new Dictionary<string, List<int>>();
                    }

                    if (isCorrect)
                    {
                        if (!answeredQuestions.ContainsKey(databankName))
                        {
                            answeredQuestions[databankName] = new List<int>();
                        }
                        answeredQuestions[databankName].Add(questionNumber);
                    }

                    updates["AnsweredQuestions"] = answeredQuestions;
                }
                else
                {
                    Debug.LogError("User document not found during score update!");
                    return;
                }

                transaction.Update(docRef, updates);
            });

            Debug.Log($"Score atualizado para {newScore} e questionNumber {questionNumber} adicionado em {databankName} se a resposta foi correta");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao atualizar score: {e.Message}");
            throw;
        }
    }

    public async Task UpdateUserData(UserData userData)
    {
        try
        {
            if (!isInitialized) throw new System.Exception("Firestore não inicializado");

            if (string.IsNullOrEmpty(userData.UserId))
                throw new ArgumentException("UserId não pode ser vazio");

            DocumentReference docRef = db.Collection("Users").Document(userData.UserId);

            // Usar o método ToDictionary para garantir consistência nos campos
            Dictionary<string, object> userDataDict = userData.ToDictionary();

            await docRef.UpdateAsync(userDataDict);
            Debug.Log($"Dados do usuário {userData.UserId} atualizados com sucesso");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao atualizar dados do usuário: {e.Message}");
            throw;
        }
    }

    public void ListenToUserData(string userId, Action<int> onScoreChanged = null, Action<Dictionary<string, List<int>>> onAnsweredQuestionsChanged = null)
    {
        db.Collection("Users").Document(userId)
        .Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();

                // Processa alterações no Score
                if (onScoreChanged != null && data.ContainsKey("Score"))
                {
                    int newScore = Convert.ToInt32(data["Score"]);
                    UserDataStore.UpdateScore(newScore);
                    onScoreChanged.Invoke(newScore);
                    Debug.Log($"Score atualizado do Firestore: {newScore}");
                }

                // Processa alterações em AnsweredQuestions
                if (onAnsweredQuestionsChanged != null && data.ContainsKey("AnsweredQuestions"))
                {
                    try
                    {
                        Dictionary<string, List<int>> answeredQuestions = new Dictionary<string, List<int>>();
                        var answeredQuestionsData = data["AnsweredQuestions"] as Dictionary<string, object>;

                        if (answeredQuestionsData != null)
                        {
                            foreach (var kvp in answeredQuestionsData)
                            {
                                string databankName = kvp.Key;
                                var questionsList = kvp.Value as IEnumerable<object>;

                                if (questionsList != null)
                                {
                                    answeredQuestions[databankName] = questionsList
                                        .Select(q => Convert.ToInt32(q))
                                        .ToList();

                                    // Atualiza o AnsweredQuestionsListStore
                                    AnsweredQuestionsListStore.UpdateAnsweredQuestionsCount(
                                        userId,
                                        databankName,
                                        answeredQuestions[databankName].Count
                                    );

                                    Debug.Log($"Questões respondidas atualizadas para {databankName}: {answeredQuestions[databankName].Count}");
                                }
                            }

                            // Notifica o callback
                            onAnsweredQuestionsChanged.Invoke(answeredQuestions);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Erro ao processar AnsweredQuestions do Firestore: {ex.Message}");
                    }
                }
            }
        });
    }

    public async Task UpdateUserProgress(string userId, int progress)
    {
        try
        {
            if (!isInitialized) throw new System.Exception("Firestore não inicializado");

            DocumentReference docRef = db.Collection("Users").Document(userId);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Progress", progress }
            };

            await docRef.UpdateAsync(updates);
            Debug.Log($"Progresso do usuário atualizado para {progress}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao atualizar progresso do usuário: {e.Message}");
            throw;
        }
    }

    public async Task ResetAnsweredQuestions(string userId, string databankName)
    {
        try
        {
            if (!isInitialized) throw new System.Exception("Firestore não inicializado");

            DocumentReference docRef = db.Collection("Users").Document(userId);

            await db.RunTransactionAsync(async transaction =>
            {
                DocumentSnapshot snapshot = await transaction.GetSnapshotAsync(docRef);

                if (snapshot.Exists)
                {
                    Dictionary<string, List<int>> answeredQuestions = snapshot.GetValue<Dictionary<string, List<int>>>("AnsweredQuestions");

                    if (answeredQuestions != null && answeredQuestions.ContainsKey(databankName))
                    {
                        answeredQuestions.Remove(databankName);
                        transaction.Update(docRef, "AnsweredQuestions", answeredQuestions);
                        Debug.Log($"AnsweredQuestions para {databankName} removidas para o usuário {userId}");
                    }
                    else
                    {
                        Debug.LogWarning($"AnsweredQuestions para {databankName} não encontrada para o usuário {userId}");
                    }
                }
                else
                {
                    Debug.LogError($"Usuário {userId} não encontrado!");
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao remover AnsweredQuestions: {e.Message}");
            throw;
        }
    }

    public async Task DeleteDocument(string collection, string documentId)
    {
        try
        {
            if (!isInitialized) throw new System.Exception("Firestore não inicializado");

            var user = AuthenticationRepository.Instance.Auth.CurrentUser;
            if (user == null) throw new System.Exception("Usuário não está autenticado");

            string token = await user.TokenAsync(true);
            Debug.Log($"Token atualizado antes da deleção: {!string.IsNullOrEmpty(token)}");

            DocumentReference docRef = db.Collection(collection).Document(documentId);

            int maxRetries = 3;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    await docRef.DeleteAsync();
                    Debug.Log($"Documento {documentId} deletado com sucesso da coleção {collection}");
                    return;
                }
                catch (System.Exception e) when (i < maxRetries - 1)
                {
                    Debug.LogWarning($"Tentativa {i + 1} falhou: {e.Message}. Tentando novamente...");
                    await Task.Delay(1000);
                    token = await user.TokenAsync(true);
                }
            }

            throw new System.Exception($"Falha ao deletar documento após {maxRetries} tentativas");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao deletar documento: {e.Message}");
            throw;
        }
    }

    public async Task<bool> AreNicknameTaken(string nickName)
    {
        QuerySnapshot snapshotNickname = await db.Collection("Users").WhereEqualTo("NickName", nickName).GetSnapshotAsync();
        return snapshotNickname.Documents.Count() > 0;
    }

    public async Task UpdateUserProfileImageUrl(string userId, string imageUrl)
    {
        try
        {
            DocumentReference userRef = db.Collection("Users").Document(userId);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "ProfileImageUrl", imageUrl }
            };

            await userRef.UpdateAsync(updates);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao atualizar URL da imagem de perfil: {ex.Message}");
            throw;
        }
    }

    public async Task<List<UserData>> GetAllUsersData()
    {
        try
        {
            if (!isInitialized)
            {
                throw new System.Exception("Firestore não inicializado");
            }

            QuerySnapshot querySnapshot = await db.Collection("Users").GetSnapshotAsync();
            List<UserData> users = new List<UserData>();

            foreach (DocumentSnapshot doc in querySnapshot.Documents)
            {
                Dictionary<string, object> userData = doc.ToDictionary();
                users.Add(ConvertFromFirestore(userData));
            }

            return users;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao buscar todos os usuários: {e.Message}");
            throw;
        }
    }

}


