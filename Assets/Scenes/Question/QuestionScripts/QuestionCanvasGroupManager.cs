using UnityEngine;
using UnityEngine.UI;

public class QuestionCanvasGroupManager : MonoBehaviour
{
    [Header("Question UI")]
    [SerializeField] private CanvasGroup questionTextContainer;
    [SerializeField] private CanvasGroup questionImageContainer;

    [Header("Answer UI")]
    [SerializeField] private CanvasGroup answerTextCanvasGroup;
    [SerializeField] private CanvasGroup answerImageCanvasGroup;

    [Header("Feedback UI")]
    [SerializeField] private CanvasGroup questionsCompletedFeedback;
    [SerializeField] private Image feedbackPanel;

    [Header("Other UI")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private CanvasGroup bottomBar;

    private void Awake()
    {
        if (!AreAllCanvasGroupsAssigned())
        {
            Debug.LogError("Alguns CanvasGroups não estão atribuídos corretamente!");
            return;
        }

        ValidateComponents();
        InitializeCanvasGroups();
    }

    public void ValidateComponents()
    {
        if (loadingCanvasGroup == null)
            Debug.LogError("LoadingCanvasGroup não está atribuído");
        if (answerTextCanvasGroup == null)
            Debug.LogError("AnswerTextCanvasGroup não está atribuído");
        if (answerImageCanvasGroup == null)
            Debug.LogError("AnswerImageCanvasGroup não está atribuído");
        if (questionsCompletedFeedback == null)
            Debug.LogError("QuestionsCompletedFeedback não está atribuído");
        if (bottomBar == null)
            Debug.LogError("BottomBar não está atribuído");
        if (questionTextContainer == null)
            Debug.LogError("QuestionTextContainer não está atribuído");
        if (questionImageContainer == null)
            Debug.LogError("QuestionImageContainer não está atribuído");
    }

    public void ShowLoading()
    {
        SetCanvasGroupState(loadingCanvasGroup, true);
        SetCanvasGroupState(questionTextContainer, false);
        SetCanvasGroupState(questionImageContainer, false);
        SetCanvasGroupState(answerTextCanvasGroup, false);
        SetCanvasGroupState(answerImageCanvasGroup, false);
        SetCanvasGroupState(bottomBar, false);
    }

    public void ShowQuestion(bool isImageQuestion, bool isImageAnswer)
    {
        Debug.Log($"ShowQuestion chamado - isImageQuestion: {isImageQuestion}, isImageAnswer: {isImageAnswer}");

        SetCanvasGroupState(loadingCanvasGroup, false);

        // Configurar visualização da questão
        SetCanvasGroupState(questionTextContainer, !isImageQuestion);
        SetCanvasGroupState(questionImageContainer, isImageQuestion);

        // Configurar visualização das respostas
        SetCanvasGroupState(answerTextCanvasGroup, !isImageAnswer);
        SetCanvasGroupState(answerImageCanvasGroup, isImageAnswer);

        SetCanvasGroupState(bottomBar, true);

        Debug.Log($"Estado dos CanvasGroups após configuração:");
        Debug.Log($"QuestionTextContainer: {(questionTextContainer?.alpha > 0 ? "Visível" : "Invisível")} (deve ser {(!isImageQuestion ? "Visível" : "Invisível")})");
        Debug.Log($"QuestionImageContainer: {(questionImageContainer?.alpha > 0 ? "Visível" : "Invisível")} (deve ser {(isImageQuestion ? "Visível" : "Invisível")})");
        Debug.Log($"AnswerTextCanvasGroup: {(answerTextCanvasGroup?.alpha > 0 ? "Visível" : "Invisível")} (deve ser {(!isImageAnswer ? "Visível" : "Invisível")})");
        Debug.Log($"AnswerImageCanvasGroup: {(answerImageCanvasGroup?.alpha > 0 ? "Visível" : "Invisível")} (deve ser {(isImageAnswer ? "Visível" : "Invisível")})");
    }

    private void LogRectTransformInfo(RectTransform rect, string name)
    {
        if (rect != null)
        {
            Debug.Log($"{name} RectTransform - Pos: {rect.position}, AnchoredPos: {rect.anchoredPosition}, " +
                      $"Pivot: {rect.pivot}, Size: {rect.sizeDelta}");
        }
    }


    private void LogCanvasGroupStates()
    {
        Debug.Log($"Estado dos CanvasGroups:");
        Debug.Log($"QuestionTextContainer: {(questionTextContainer?.alpha > 0 ? "Visível" : "Invisível")}");
        Debug.Log($"QuestionImageContainer: {(questionImageContainer?.alpha > 0 ? "Visível" : "Invisível")}");
        Debug.Log($"AnswerTextCanvasGroup: {(answerTextCanvasGroup?.alpha > 0 ? "Visível" : "Invisível")}");
        Debug.Log($"AnswerImageCanvasGroup: {(answerImageCanvasGroup?.alpha > 0 ? "Visível" : "Invisível")}");
        Debug.Log($"BottomBar: {(bottomBar?.alpha > 0 ? "Visível" : "Invisível")}");
    }

    public void ShowAnswerFeedback(bool isCorrect, Color correctColor, Color incorrectColor)
    {
        if (feedbackPanel != null)
        {
            feedbackPanel.gameObject.SetActive(true);
            feedbackPanel.color = isCorrect ? correctColor : incorrectColor;
        }
    }

    public void HideAnswerFeedback()
    {
        if (feedbackPanel != null)
        {
            feedbackPanel.gameObject.SetActive(false);
        }
    }

    public void ShowCompletionFeedback()
    {
        // Desativar grupos da questão e respostas
        SetCanvasGroupState(questionTextContainer, false);
        SetCanvasGroupState(questionImageContainer, false);
        SetCanvasGroupState(answerTextCanvasGroup, false);
        SetCanvasGroupState(answerImageCanvasGroup, false);

        // Ativar feedback de conclusão
        SetCanvasGroupState(questionsCompletedFeedback, true);
    }

    public void DisableAnswers()
    {
        SetCanvasGroupInteractable(answerTextCanvasGroup, false);
        SetCanvasGroupInteractable(answerImageCanvasGroup, false);
    }

    private void InitializeCanvasGroups()
    {
        foreach (var canvasGroup in GetAllCanvasGroups())
        {
            if (canvasGroup != null)
            {
                SetCanvasGroupState(canvasGroup, false);
            }
        }
    }

    private void SetCanvasGroupState(CanvasGroup canvasGroup, bool active)
    {
        if (canvasGroup == null) return;

        canvasGroup.gameObject.SetActive(true); // Garantir que o GameObject está ativo
        canvasGroup.alpha = active ? 1f : 0f;
        canvasGroup.interactable = active;
        canvasGroup.blocksRaycasts = active;

        Debug.Log($"SetCanvasGroupState para {canvasGroup.gameObject.name}: " +
                  $"GameObject ativo: {canvasGroup.gameObject.activeInHierarchy}, " +
                  $"Alpha: {canvasGroup.alpha}, " +
                  $"Interactable: {canvasGroup.interactable}");
    }


    private void SetCanvasGroupInteractable(CanvasGroup canvasGroup, bool interactable)
    {
        if (canvasGroup == null) return;

        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }

    private CanvasGroup[] GetAllCanvasGroups()
    {
        return new CanvasGroup[]
        {
            loadingCanvasGroup,
            answerTextCanvasGroup,
            answerImageCanvasGroup,
            questionsCompletedFeedback,
            bottomBar,
            questionTextContainer,
            questionImageContainer
        };
    }

    // Propriedades públicas mantidas para compatibilidade, se necessário
    public CanvasGroup LoadingCanvasGroup => loadingCanvasGroup;
    public CanvasGroup AnswerTextCanvasGroup => answerTextCanvasGroup;
    public CanvasGroup AnswerImageCanvasGroup => answerImageCanvasGroup;
    public CanvasGroup QuestionsCompletedFeedback => questionsCompletedFeedback;
    public CanvasGroup BottomBar => bottomBar;
    public CanvasGroup QuestionTextContainer => questionTextContainer;
    public CanvasGroup QuestionImageContainer => questionImageContainer;


    public bool AreAllCanvasGroupsAssigned()
    {
        bool allAssigned = true;

        if (answerImageCanvasGroup == null)
        {
            Debug.LogError("AnswerImageCanvasGroup não está atribuído no Inspector");
            allAssigned = false;
        }

        if (answerTextCanvasGroup == null)
        {
            Debug.LogError("AnswerTextCanvasGroup não está atribuído no Inspector");
            allAssigned = false;
        }

        if (questionImageContainer == null)
        {
            Debug.LogError("QuestionImageContainer não está atribuído no Inspector");
            allAssigned = false;
        }

        if (questionTextContainer == null)
        {
            Debug.LogError("QuestionTextContainer não está atribuído no Inspector");
            allAssigned = false;
        }

        return allAssigned;
    }

}