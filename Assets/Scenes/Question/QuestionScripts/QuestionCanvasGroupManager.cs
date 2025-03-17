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
    
    [Header("Bonus UI")]
    [SerializeField] private CanvasGroup bonusFeedbackCanvasGroup;

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
        // O bonusFeedbackCanvasGroup é opcional, então não verificamos aqui
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
        if (bonusFeedbackCanvasGroup != null)
        {
            Debug.Log($"BonusFeedbackCanvasGroup: {(bonusFeedbackCanvasGroup?.alpha > 0 ? "Visível" : "Invisível")}");
        }
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
        Debug.Log("Iniciando exibição do feedback de conclusão...");
        if (questionTextContainer != null) questionTextContainer.gameObject.SetActive(false);
        if (questionImageContainer != null) questionImageContainer.gameObject.SetActive(false);
        if (answerTextCanvasGroup != null) answerTextCanvasGroup.gameObject.SetActive(false);
        if (answerImageCanvasGroup != null) answerImageCanvasGroup.gameObject.SetActive(false);

        if (feedbackPanel != null)
        {
            feedbackPanel.gameObject.SetActive(false);
        }

        foreach (var canvasGroup in GetAllCanvasGroups())
        {
            if (canvasGroup != null && canvasGroup != questionsCompletedFeedback && canvasGroup != bonusFeedbackCanvasGroup)
            {
                SetCanvasGroupState(canvasGroup, false);
                Debug.Log($"Desativado canvas group: {canvasGroup.gameObject.name}");
            }
        }

        SetCanvasGroupState(bottomBar, false);

        if (questionsCompletedFeedback != null)
        {
            questionsCompletedFeedback.gameObject.SetActive(true);
            questionsCompletedFeedback.alpha = 1f;
            questionsCompletedFeedback.interactable = true;
            questionsCompletedFeedback.blocksRaycasts = true;
            Debug.Log("Canvas Group de feedback de conclusão ativado");
        }
        else
        {
            Debug.LogError("Erro crítico: questionsCompletedFeedback é nulo!");
        }

        if (questionsCompletedFeedback != null)
        {
            Debug.Log($"Estado final do feedback de conclusão: " +
                     $"GameObject ativo: {questionsCompletedFeedback.gameObject.activeInHierarchy}, " +
                     $"Alpha: {questionsCompletedFeedback.alpha}, " +
                     $"Interactable: {questionsCompletedFeedback.interactable}");
        }
    }

    public void HideCompletionFeedback()
    {
        if (questionsCompletedFeedback != null)
        {
            questionsCompletedFeedback.gameObject.SetActive(false);
            SetCanvasGroupState(questionsCompletedFeedback, false);
        }
    }

    // Novo método para mostrar o feedback de bônus
    public void ShowBonusFeedback(bool show)
    {
        if (bonusFeedbackCanvasGroup != null)
        {
            // Não usamos SetCanvasGroupState porque queremos tratamento especial
            bonusFeedbackCanvasGroup.gameObject.SetActive(show);
            bonusFeedbackCanvasGroup.alpha = show ? 1f : 0f;
            bonusFeedbackCanvasGroup.interactable = show;
            bonusFeedbackCanvasGroup.blocksRaycasts = show;
            
            Debug.Log($"BonusFeedback definido como: {(show ? "visível" : "invisível")}");
        }
        else
        {
            Debug.LogWarning("bonusFeedbackCanvasGroup não está atribuído! Não é possível mostrar o feedback de bônus.");
        }
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
                // Não inicializar o bonusFeedbackCanvasGroup aqui
                if (canvasGroup != bonusFeedbackCanvasGroup)
                {
                    SetCanvasGroupState(canvasGroup, false);
                }
                else
                {
                    // Apenas garanta que o bonusFeedbackCanvasGroup esteja desativado no início
                    canvasGroup.gameObject.SetActive(false);
                    canvasGroup.alpha = 0f;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                }
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
            questionImageContainer,
            bonusFeedbackCanvasGroup  // Adicionado o canvas group do bônus
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
    public CanvasGroup BonusFeedbackCanvasGroup => bonusFeedbackCanvasGroup;

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

        // Não verificamos o bonusFeedbackCanvasGroup aqui porque é opcional

        return allAssigned;
    }
}