using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeekResetCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    private Coroutine countdownUpdateCoroutine;

    public void Initialize(TMP_Text textComponent)
    {
        countdownText = textComponent;
        UpdateCountdown();
        countdownUpdateCoroutine = StartCoroutine(UpdateCountdownRoutine());
    }

    private IEnumerator UpdateCountdownRoutine()
    {
        while (true)
        {
            // Atualizar a cada minuto
            yield return new WaitForSeconds(60);
            UpdateCountdown();
        }
    }

    private void UpdateCountdown()
    {
        try
        {
            // Obter a data e hora atual em Brasília
            TimeZoneInfo brasiliaTimeZone;
            try
            {
                brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                Debug.LogWarning("Fuso horário de Brasília não encontrado, usando UTC-3");
                brasiliaTimeZone = TimeZoneInfo.CreateCustomTimeZone("Brasilia", new TimeSpan(-3, 0, 0), "Brasilia", "Brasilia");
            }

            DateTime nowInBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);
            
            // Calcular a data do próximo domingo (23:59)
            DateTime nextSundayReset = nowInBrasilia.Date;
            int daysUntilNextSunday = ((int)DayOfWeek.Sunday - (int)nowInBrasilia.DayOfWeek + 7) % 7;
            
            // Se hoje é domingo e ainda não passou das 23:59, o reset será hoje
            if (daysUntilNextSunday == 0)
            {
                // Se já passou das 23:59, então o reset será no próximo domingo
                if (nowInBrasilia.TimeOfDay >= new TimeSpan(23, 59, 0))
                {
                    daysUntilNextSunday = 7;
                }
            }
            
            // Se hoje não é domingo, precisamos calcular a data do próximo domingo
            nextSundayReset = nextSundayReset.AddDays(daysUntilNextSunday)
                            .Add(new TimeSpan(23, 59, 0));
            
            // Calcular a diferença de tempo
            TimeSpan timeUntilReset = nextSundayReset - nowInBrasilia;
            
            // Formatar a mensagem de countdown
            string countdownMessage;
            if (timeUntilReset.Days > 1)
            {
                countdownMessage = $"Reset em {timeUntilReset.Days} dias";
            }
            else if (timeUntilReset.Days == 1)
            {
                countdownMessage = "Reset amanhã";
            }
            else if (timeUntilReset.Hours > 1)
            {
                countdownMessage = $"Reset em {timeUntilReset.Hours} horas";
            }
            else if (timeUntilReset.Hours == 1)
            {
                countdownMessage = "Reset em 1 hora";
            }
            else if (timeUntilReset.Minutes > 1)
            {
                countdownMessage = $"Reset em {timeUntilReset.Minutes} minutos";
            }
            else
            {
                countdownMessage = "Reset a qualquer momento";
            }
            
            // Atualizar o texto no UI
            if (countdownText != null)
            {
                countdownText.text = countdownMessage;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao calcular countdown de reset: {e.Message}");
            if (countdownText != null)
            {
                countdownText.text = "Reset aos domingos";
            }
        }
    }

    private void OnDestroy()
    {
        if (countdownUpdateCoroutine != null)
        {
            StopCoroutine(countdownUpdateCoroutine);
        }
    }

    // Método público para forçar uma atualização imediata
    public void ForceUpdate()
    {
        UpdateCountdown();
    }
}
