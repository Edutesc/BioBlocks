using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;

public class BonusFirestore
{
    private const string COLLECTION_NAME = "UserBonus";
    private const string CORRECT_ANSWER_BONUS = "correctAnswerBonus";
    private const int BONUS_ACTIVATION_THRESHOLD = 5;

    private FirebaseFirestore db;
    private ListenerRegistration bonusListener;

    public BonusFirestore()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public async Task IncrementCorrectAnswerBonus(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
        }

        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            List<BonusType> bonusList = new List<BonusType>();

            if (snapshot.Exists)
            {
                // Documento já existe, precisamos atualizar
                Dictionary<string, object> data = snapshot.ToDictionary();

                if (data.ContainsKey("BonusList"))
                {
                    List<object> bonusListData = data["BonusList"] as List<object>;

                    if (bonusListData != null)
                    {
                        foreach (object bonusObj in bonusListData)
                        {
                            Dictionary<string, object> bonusDict = bonusObj as Dictionary<string, object>;
                            if (bonusDict != null)
                            {
                                BonusType bonus = new BonusType(
                                    bonusDict.ContainsKey("BonusName") ? bonusDict["BonusName"].ToString() : "",
                                    bonusDict.ContainsKey("BonusCount") ? Convert.ToInt32(bonusDict["BonusCount"]) : 0,
                                    bonusDict.ContainsKey("IsBonusActive") ? Convert.ToBoolean(bonusDict["IsBonusActive"]) : false
                                );
                                bonusList.Add(bonus);
                            }
                        }
                    }
                }

                // Verifica se já existe um bônus do tipo correctAnswerBonus
                BonusType correctAnswerBonus = bonusList.FirstOrDefault(b => b.BonusName == CORRECT_ANSWER_BONUS);

                if (correctAnswerBonus != null)
                {
                    // Atualiza o bônus existente
                    if (!correctAnswerBonus.IsBonusActive)
                    {
                        correctAnswerBonus.BonusCount++;

                        // Verifica se atingiu o limite para ativação
                        if (correctAnswerBonus.BonusCount >= BONUS_ACTIVATION_THRESHOLD)
                        {
                            correctAnswerBonus.IsBonusActive = true;
                            Debug.Log($"BonusFirestore: Bônus {CORRECT_ANSWER_BONUS} ativado para o usuário {userId}");
                        }
                    }
                }
                else
                {
                    // Cria um novo bônus
                    bonusList.Add(new BonusType(CORRECT_ANSWER_BONUS, 1, false));
                }
            }
            else
            {
                // Documento não existe, vamos criar com o primeiro bônus
                bonusList.Add(new BonusType(CORRECT_ANSWER_BONUS, 1, false));
            }

            // Salva as alterações no Firestore
            await SaveBonusList(userId, bonusList);

            Debug.Log($"BonusFirestore: Bônus {CORRECT_ANSWER_BONUS} incrementado para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao incrementar bônus: {e.Message}");
        }
    }

    private async Task SaveBonusList(string userId, List<BonusType> bonusList)
    {
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "BonusList", bonusList.Select(b => b.ToDictionary()).ToList() }
            };

            await docRef.SetAsync(data);
            Debug.Log($"BonusFirestore: Lista de bônus salva para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao salvar lista de bônus: {e.Message}");
        }
    }

    public async Task<List<BonusType>> GetUserBonuses(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return new List<BonusType>();
        }

        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            List<BonusType> bonusList = new List<BonusType>();

            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();

                if (data.ContainsKey("BonusList"))
                {
                    List<object> bonusListData = data["BonusList"] as List<object>;

                    if (bonusListData != null)
                    {
                        foreach (object bonusObj in bonusListData)
                        {
                            Dictionary<string, object> bonusDict = bonusObj as Dictionary<string, object>;
                            if (bonusDict != null)
                            {
                                BonusType bonus = new BonusType(
                                    bonusDict.ContainsKey("BonusName") ? bonusDict["BonusName"].ToString() : "",
                                    bonusDict.ContainsKey("BonusCount") ? Convert.ToInt32(bonusDict["BonusCount"]) : 0,
                                    bonusDict.ContainsKey("IsBonusActive") ? Convert.ToBoolean(bonusDict["IsBonusActive"]) : false
                                );
                                bonusList.Add(bonus);
                            }
                        }
                    }
                }
            }

            return bonusList;
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao obter bônus do usuário: {e.Message}");
            return new List<BonusType>();
        }
    }

    public void ListenForBonusUpdates(string userId, Action<List<BonusType>> onUpdate)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("BonusFirestore: UserId é nulo ou vazio");
            return;
        }

        try
        {
            StopListeningForBonusUpdates();
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            bonusListener = docRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    List<BonusType> bonusList = new List<BonusType>();
                    Dictionary<string, object> data = snapshot.ToDictionary();

                    if (data.ContainsKey("BonusList"))
                    {
                        List<object> bonusListData = data["BonusList"] as List<object>;

                        if (bonusListData != null)
                        {
                            foreach (object bonusObj in bonusListData)
                            {
                                Dictionary<string, object> bonusDict = bonusObj as Dictionary<string, object>;
                                if (bonusDict != null)
                                {
                                    BonusType bonus = new BonusType(
                                        bonusDict.ContainsKey("BonusName") ? bonusDict["BonusName"].ToString() : "",
                                        bonusDict.ContainsKey("BonusCount") ? Convert.ToInt32(bonusDict["BonusCount"]) : 0,
                                        bonusDict.ContainsKey("IsBonusActive") ? Convert.ToBoolean(bonusDict["IsBonusActive"]) : false
                                    );
                                    bonusList.Add(bonus);
                                }
                            }
                        }
                    }

                    onUpdate?.Invoke(bonusList);
                }
                else
                {
                    onUpdate?.Invoke(new List<BonusType>());
                }
            });

            Debug.Log($"BonusFirestore: Ouvindo atualizações para o usuário {userId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"BonusFirestore: Erro ao configurar listener: {e.Message}");
        }
    }

    public void StopListeningForBonusUpdates()
    {
        bonusListener?.Stop();
        bonusListener = null;
        Debug.Log("BonusFirestore: Parou de ouvir atualizações");
    }
}