using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class QuestionTransitionManager : MonoBehaviour
{
    [Header("Question Container")]
    [SerializeField] private CanvasGroup questionObjects;

    [Header("Animation Settings")]
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private float slideDistance = 800f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Fade Settings")]
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private bool useSmoothFade = true;
    [SerializeField] private float fadeOverlap = 0.1f;

    private RectTransform questionObjectsRect;
    private Vector2 screenCenter;
    private bool isTransitioning;
    
    public event Action OnBeforeTransitionStart; 
    public event Action OnTransitionMidpoint;   

    private void Awake()
    {
        questionObjectsRect = questionObjects.GetComponent<RectTransform>();
        screenCenter = questionObjectsRect.anchoredPosition;
    }

    public async Task TransitionToNextQuestion()
    {
        if (isTransitioning) return;

        OnBeforeTransitionStart?.Invoke();
        
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(TransitionCoroutine(tcs));
        await tcs.Task;
    }

    private IEnumerator TransitionCoroutine(TaskCompletionSource<bool> tcs)
    {
        isTransitioning = true;

        Vector2 startPos = screenCenter;
        Vector2 exitPos = screenCenter + new Vector2(-slideDistance, 0);
        Vector2 enterPos = screenCenter + new Vector2(slideDistance, 0);

        float elapsedTime = 0;
        float exitDuration = transitionDuration * (1 + fadeOverlap);

        // Animar saída
        while (elapsedTime < exitDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / exitDuration;
            float curveValue = transitionCurve.Evaluate(normalizedTime);

            questionObjectsRect.anchoredPosition = Vector2.Lerp(startPos, exitPos, curveValue);
            
            if (useSmoothFade)
            {
                questionObjects.alpha = Mathf.Lerp(1f, minAlpha, curveValue);
            }

            yield return null;
        }

        OnTransitionMidpoint?.Invoke();
        
        questionObjectsRect.anchoredPosition = enterPos;
        
        elapsedTime = 0;
        float enterDuration = transitionDuration * (1 - fadeOverlap);

        while (elapsedTime < enterDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / enterDuration;
            float curveValue = transitionCurve.Evaluate(normalizedTime);

            questionObjectsRect.anchoredPosition = Vector2.Lerp(enterPos, screenCenter, curveValue);
            
            if (useSmoothFade)
            {
                questionObjects.alpha = Mathf.Lerp(minAlpha, 1f, curveValue);
            }

            yield return null;
        }

        questionObjectsRect.anchoredPosition = screenCenter;
        questionObjects.alpha = 1f;

        isTransitioning = false;
        tcs.SetResult(true);
    }
}