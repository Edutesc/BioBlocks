using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionViewInitializer : MonoBehaviour
{
    [SerializeField] private QuestionLoadManager loadManager;

    private async void Start()
    {
        // Desativa todos os outros elementos da UI inicialmente
        foreach (var canvasGroup in FindObjectsByType<CanvasGroup>(FindObjectsSortMode.None))
        {
            canvasGroup.alpha = 0;
        }

        if (await ShouldShowResetView())
        {
            NavigationManager.Instance.NavigateTo("ResetDatabaseView");
            return;
        }

        // Se chegou aqui, pode mostrar a UI normalmente
        foreach (var canvasGroup in FindObjectsByType<CanvasGroup>(FindObjectsSortMode.None))
        {
            canvasGroup.alpha = 1;
        }
    }

    private async Task<bool> ShouldShowResetView()
    {
        try
        {
            // Carrega as questões e já atualiza o databankName no processo
            var questions = await loadManager.LoadQuestionsForSet(QuestionSetManager.GetCurrentQuestionSet());

            if (questions == null || questions.Count == 0)
            {
                // Garante que temos o databankName antes de navegar
                if (!string.IsNullOrEmpty(loadManager.databankName))
                {
                    var sceneData = new Dictionary<string, object>
                {
                    { "databankName", loadManager.databankName }
                };
                    SceneDataManager.Instance.SetData(sceneData);
                    return true;
                }
                else
                {
                    Debug.LogError("databankName não foi definido no LoadManager");
                }
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro em ShouldShowResetView: {e.Message}");
            return false;
        }
    }

}
