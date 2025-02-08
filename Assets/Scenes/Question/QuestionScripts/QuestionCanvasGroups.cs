using UnityEngine;

public class QuestionCanvasGroups : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private CanvasGroup questionCanvasGroup;
    [SerializeField] private CanvasGroup questionImageCanvasGroup;
    [SerializeField] private CanvasGroup questionsCompletedFeedback;
    [SerializeField] private CanvasGroup bottomBar;
    [SerializeField] private CanvasGroup questionTextBackground;

    public void ValidateComponents()
    {
        if (loadingCanvasGroup == null)
            Debug.LogError("LoadingCanvasGroup não está atribuído");
        if (questionCanvasGroup == null)
            Debug.LogError("QuestionCanvasGroup não está atribuído");
        if (questionImageCanvasGroup == null)
            Debug.LogError("QuestionImageCanvasGroup não está atribuído");
        if (questionsCompletedFeedback == null)
            Debug.LogError("QuestionsCompletedFeedback não está atribuído");
        if (bottomBar == null)
            Debug.LogError("BottomBar não está atribuído");
        if (questionTextBackground == null)
            Debug.LogError("QuestionTextBackground não está atribuído");
    }

    // Propriedades públicas para acesso
    public CanvasGroup LoadingCanvasGroup => loadingCanvasGroup;
    public CanvasGroup QuestionCanvasGroup => questionCanvasGroup;
    public CanvasGroup QuestionImageCanvasGroup => questionImageCanvasGroup;
    public CanvasGroup QuestionsCompletedFeedback => questionsCompletedFeedback;
    public CanvasGroup BottomBar => bottomBar;
    public CanvasGroup QuestionTextBackground => questionTextBackground;

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
            questionCanvasGroup,
            questionImageCanvasGroup,
            questionsCompletedFeedback,
            bottomBar,
            questionTextBackground
        };
    }
}