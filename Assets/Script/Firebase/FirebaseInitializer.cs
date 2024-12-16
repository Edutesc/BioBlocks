using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseInitializer : MonoBehaviour
{
    void Start()
    {
        // Inicializar o Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // O Firebase está inicializado com sucesso
                Debug.Log("Firebase inicializado com sucesso.");

                // Registrar um evento de analítica
                FirebaseAnalytics.LogEvent("game_started");
            }
            else
            {
                // Algo deu errado, exiba uma mensagem de erro
                Debug.LogError($"Falha na inicialização do Firebase: {dependencyStatus}");
            }
        });
    }
}
