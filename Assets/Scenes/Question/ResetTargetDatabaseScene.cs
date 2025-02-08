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
            UpdateDatabankNameText();
        }
        else
        {
            Debug.LogError("Nenhum databankName encontrado nos dados da cena");
            NavigateToPathway();
            return;

        }

        SceneDataManager.Instance.ClearData();
        currentUserData = UserDataStore.CurrentUserData;
    }

    private void UpdateDatabankNameText()
    {
        if (databankNameText != null && !string.IsNullOrEmpty(databankName))
        {
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
            if (databankNameMap.TryGetValue(databankName, out displayName))
            {
                databankNameText.text = $"Tópico: {displayName}";
            }
            else
            {
                databankNameText.text = $"Tópico: {databankName}";
            }

            Debug.Log($"Updating databank name text to: {databankNameText.text}");
        }
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
