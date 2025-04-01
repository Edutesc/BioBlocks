using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Firebase.Firestore;

public class ListCompletionBonusManager
{
    private const string COLLECTION_NAME = "UserBonus";
    private FirebaseFirestore db;
    
    public ListCompletionBonusManager()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    
    public async Task ActivateListCompletionBonus(string userId, string databankName)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("ListCompletionBonusManager: UserId é nulo ou vazio");
            return;
        }
        
        try
        {
            // Obter o gerenciador de bônus especial existente
            SpecialBonusManager specialBonusManager = new SpecialBonusManager();
            List<BonusType> bonusList = await specialBonusManager.GetUserBonuses(userId);
            
            // Encontrar o bônus das listas
            string bonusName = "listCompletionBonus";
            BonusType listBonus = bonusList.FirstOrDefault(b => b.BonusName == bonusName);
            
            if (listBonus == null)
            {
                // Se não existir, criar um novo
                listBonus = new BonusType(bonusName, 1, true, 0, false);
                bonusList.Add(listBonus);
                Debug.Log($"ListCompletionBonusManager: Criando novo bônus de lista para {databankName}");
            }
            else
            {
                // Se já existir, incrementar o contador
                listBonus.BonusCount++;
                listBonus.IsBonusActive = true;
                Debug.Log($"ListCompletionBonusManager: Incrementando bônus de lista para {databankName}. Novo valor: {listBonus.BonusCount}");
            }
            
            // Salvar a lista atualizada
            await specialBonusManager.SaveBonusList(userId, bonusList);
            Debug.Log($"ListCompletionBonusManager: Bônus de lista ativado com sucesso para {databankName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"ListCompletionBonusManager: Erro ao ativar bônus de lista: {e.Message}");
        }
    }
    
    public async Task<bool> CheckIfDatabankEligibleForBonus(string userId, string databankName)
    {
        try
        {
            // Verifica se o usuário já ganhou bônus por este databank específico
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                
                // Verificar se já existe um registro de lista completa para este databank
                if (data.ContainsKey("CompletedDatabanks"))
                {
                    List<object> completedDatabanks = data["CompletedDatabanks"] as List<object>;
                    
                    if (completedDatabanks != null && completedDatabanks.Contains(databankName))
                    {
                        Debug.Log($"ListCompletionBonusManager: Databank {databankName} já foi marcado como completo");
                        return false;
                    }
                }
                
                // Se chegou aqui, o databank ainda não foi marcado
                return true;
            }
            
            // Se o documento não existe, ele é elegível
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"ListCompletionBonusManager: Erro ao verificar elegibilidade do databank: {e.Message}");
            return false;
        }
    }
    
    public async Task MarkDatabankAsCompleted(string userId, string databankName)
    {
        try
        {
            DocumentReference docRef = db.Collection(COLLECTION_NAME).Document(userId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            List<string> completedDatabanks = new List<string>();
            
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                
                if (data.ContainsKey("CompletedDatabanks"))
                {
                    List<object> existingList = data["CompletedDatabanks"] as List<object>;
                    
                    if (existingList != null)
                    {
                        completedDatabanks = existingList.Select(item => item.ToString()).ToList();
                    }
                }
            }
            
            // Adicionar o novo databank se ainda não estiver na lista
            if (!completedDatabanks.Contains(databankName))
            {
                completedDatabanks.Add(databankName);
                
                // Atualizar o documento
                Dictionary<string, object> updateData = new Dictionary<string, object>
                {
                    { "CompletedDatabanks", completedDatabanks }
                };
                
                await docRef.UpdateAsync(updateData);
                Debug.Log($"ListCompletionBonusManager: Databank {databankName} marcado como completo");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"ListCompletionBonusManager: Erro ao marcar databank como completo: {e.Message}");
        }
    }
}