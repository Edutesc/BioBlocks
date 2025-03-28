using UnityEngine;
using TMPro;
using System.Collections;

public class QuestionTimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timePanel; // Referência ao TimePanel
    [SerializeField] private float initialTime = 30f;
    private float currentTime;
    private bool isRunning;
    
    public event System.Action OnTimerComplete;

    private void Start()
    {
        if (timerText == null)
        {
            timerText = GameObject.Find("TimerText")?.GetComponent<TextMeshProUGUI>();
            if (timerText == null)
            {
                Debug.LogError("TimerText não encontrado!");
            }
        }

        if (timePanel == null)
        {
            timePanel = GameObject.Find("TimePanel");
            if (timePanel == null)
            {
                Debug.LogError("TimePanel não encontrado!");
            }
        }
    }

    public void StartTimer()
    {
        // Ativa o painel antes de iniciar o timer
        if (timePanel != null)
        {
            timePanel.SetActive(true);
            Debug.Log("TimePanel ativado");
        }
        else
        {
            Debug.LogError("TimePanel é null ao tentar iniciar o timer");
            return;
        }

        currentTime = initialTime;
        isRunning = true;
        UpdateTimerDisplay();
        StartCoroutine(TimerCoroutine());
        Debug.Log("Timer iniciado com sucesso");
    }

    public void StopTimer()
    {
        isRunning = false;
        StopAllCoroutines();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = $"{Mathf.Ceil(currentTime)}";
            Debug.Log($"Timer atualizado: {timerText.text}");
        }
        else
        {
            Debug.LogError("TimerText está null!");
        }
    }

    private IEnumerator TimerCoroutine()
    {
        Debug.Log("TimerCoroutine iniciada");
        while (isRunning && currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            UpdateTimerDisplay();
        }

        if (currentTime <= 0)
        {
            OnTimerComplete?.Invoke();
        }
    }
}