using UnityEngine;
using System.Threading.Tasks;

public class QuestionTransitionManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private CanvasGroup questionCanvasGroup;
    
    public async Task TransitionToQuestion() { /* Lógica de transição */ }
    public async Task TransitionToLoading() { /* Lógica de transição */ }
}

