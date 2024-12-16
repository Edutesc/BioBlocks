

using Firebase;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseFirestore db;

    private void Start()
    {
        // Inicializa o Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.IsCompleted && task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance; // Inicializa o Firestore
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies.");
            }
        });
    }

    public async Task SavePlayer(Player player)
    {
        DocumentReference docRef = db.Collection("players").Document(player.id);
    
        try
        {
            // Convert Player object to Dictionary
            Dictionary<string, object> playerData = new Dictionary<string, object>
            {
                { "name", player.name },
                { "id", player.id },
                { "score", player.score }
            };

            await docRef.SetAsync(playerData);
            Debug.Log("Player saved to Firestore successfully!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving player: " + ex.Message);
        }
    }

    public async Task<Player> LoadPlayer(string playerId)
    {
        DocumentReference docRef = db.Collection("players").Document(playerId);

        try
        {
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Dictionary<string, object> playerData = snapshot.ToDictionary();
                string name = playerData["name"].ToString();
                string id = playerData["id"].ToString();
                int score = Convert.ToInt32(playerData["score"]);
                
                Player player = new Player(name, id, score);
                return player;
            }
            else
            {
                Debug.LogWarning("Player not found.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading player: " + ex.Message);
            return null;
        }
    }
}

