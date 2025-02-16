using UnityEngine;

public class AnswerTextCanvasGroups : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private CanvasGroup answerTextCanvasGroup;
    [SerializeField] private CanvasGroup answerImageCanvasGroup;
    [SerializeField] private CanvasGroup questionsCompletedFeedback;
    [SerializeField] private CanvasGroup bottomBar;
    [SerializeField] private CanvasGroup questionTextContainer;
    [SerializeField] private CanvasGroup questionImageContainer;

    public void ValidateComponents()
    {
        if (loadingCanvasGroup == null)
            Debug.LogError("LoadingCanvasGroup não está atribuído");
        if (answerTextCanvasGroup == null)
            Debug.LogError("QuestionCanvasGroup não está atribuído");
        if (answerImageCanvasGroup == null)
            Debug.LogError("QuestionImageCanvasGroup não está atribuído");
        if (questionsCompletedFeedback == null)
            Debug.LogError("QuestionsCompletedFeedback não está atribuído");
        if (bottomBar == null)
            Debug.LogError("BottomBar não está atribuído");
        if (questionTextContainer == null)
            Debug.LogError("QuestionTextBackground não está atribuído");
        if (questionImageContainer == null)
            Debug.LogError("QuestionImageContainer não está atribuído");
    }

    // Propriedades públicas para acesso
    public CanvasGroup LoadingCanvasGroup => loadingCanvasGroup;
    public CanvasGroup AnswerTextCanvasGroup => answerTextCanvasGroup;
    public CanvasGroup AnswerImageCanvasGroup => answerImageCanvasGroup;
    public CanvasGroup QuestionsCompletedFeedback => questionsCompletedFeedback;
    public CanvasGroup BottomBar => bottomBar;
    public CanvasGroup QuestionTextContainer => questionTextContainer;
    public CanvasGroup QuestionImageContainer => questionImageContainer;

    public void InitializeCanvasGroups()
    {
        foreach (var canvasGroup in GetAllCanvasGroups())
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
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
            questionTextContainer
        };
    }
}