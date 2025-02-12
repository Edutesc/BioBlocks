using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ResetTargetDatabaseScene : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private TextMeshProUGUI resetButtonText;

    public TextMeshProUGUI databankNameText;
    private string databankName;
    private UserData currentUserData;

    private void Start()
    {
        var sceneData = SceneDataManager.Instance.GetData();
        if (sceneData != null && sceneData.TryGetValue("databankName", out object value))
        {
            databankName = value as string;
            Debug.Log($"databankName recebido do SceneDataManager: {databankName}");

            if (!string.IsNullOrEmpty(databankName))
            {
                UpdateDatabankNameText();
                currentUserData = UserDataStore.CurrentUserData;
                SceneDataManager.Instance.ClearData(); // Movido para depois de usar o databankName
            }
            else
            {
                Debug.LogError("databankName está vazio mesmo após conversão");
                return;
            }
        }
        else
        {
            Debug.LogError("Nenhum databankName encontrado nos dados da cena");
            return;
        }
    }

    private void UpdateDatabankNameText()
    {
        // Verificar se o campo databankNameText foi atribuído no Inspector
        if (databankNameText == null)
        {
            Debug.LogError("databankNameText não está referenciado no Inspector");
            return;
        }

        // Verificar se databankName tem valor
        if (string.IsNullOrEmpty(databankName))
        {
            Debug.LogError("databankName está vazio ou nulo");
            return;
        }

        Debug.Log($"Tentando atualizar texto. databankName atual: {databankName}");

        Dictionary<string, string> databankNameMap = new Dictionary<string, string>()
    {
        {"AcidBaseBufferQuestionDatabase", "Ácidos, bases e tampões"},
        {"AminoacidQuestionDatabase", "Aminoácidos e peptídeos"},
        {"BiochemistryIntroductionQuestionDatabase", "Introdução à Bioquímica"},
        {"CarbohydratesQuestionDatabase", "Carboidratos"},
        {"EnzymeQuestionDatabase", "Enzimas"},
        {"LipidsQuestionDatabase", "Lipídeos"},
        {"MembranesQuestionDatabase", "Mambranas Biológicas"},
        {"NucleicAcidsQuestionDatabase", "Ácidos nucleicos"},
        {"ProteinQuestionDatabase", "Proteínas"},
        {"WaterQuestionDatabase", "Água"}
    };

        string displayName;
        bool found = databankNameMap.TryGetValue(databankName, out displayName);

        Debug.Log($"Nome encontrado no mapa: {found}");
        Debug.Log($"Display name: {displayName}");

        if (found)
        {
            databankNameText.text = $"Tópico: {displayName}";
        }
        else
        {
            databankNameText.text = $"Tópico: {databankName}";
        }

        // Verificar se o texto foi realmente atualizado
        Debug.Log($"Texto após atualização: {databankNameText.text}");
    }

    public async void ResetAnsweredQuestions()
    {
        try
        {
            if (resetButton != null) resetButton.interactable = false;
            if (resetButtonText != null) resetButtonText.text = "Resetando...";

            string userId = currentUserData.UserId;
            Debug.Log($"Resetando questões respondidas - UserId: {userId}, databankName: {databankName}");

            await FirestoreRepository.Instance.ResetAnsweredQuestions(userId, databankName);
            Debug.Log("Questões resetadas com sucesso");

            // Feedback visual de sucesso (opcional)
            if (resetButtonText != null) resetButtonText.text = "Sucesso!";

            AnsweredQuestionsListStore.UpdateAnsweredQuestionsCount(userId, databankName, 0);
            UpdateUIAfterReset(databankName);
            // Pequeno delay para mostrar o feedback de sucesso
            await Task.Delay(500);

            NavigateToPathway();
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao resetar questões: {e.Message}");

            // Reativa o botão em caso de erro
            if (resetButton != null)
            {
                resetButton.interactable = true;
                resetButtonText.text = "Tentar Novamente";
            }
        }
    }

    private void UpdateUIAfterReset(string databaseNameToReset)
    {
        string objectName = $"{databaseNameToReset}PorcentageText";
        GameObject textObject = GameObject.Find(objectName);

        if (textObject != null)
        {
            TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = "0%";
                Debug.Log($"{databankName}PorcentageText reset to 0%");
            }
        }
    }

    public void NavigateToPathway()
    {
        Debug.Log("Navegando para PathwayScene");
        NavigationManager.Instance.NavigateTo("PathwayScene");
    }

}
