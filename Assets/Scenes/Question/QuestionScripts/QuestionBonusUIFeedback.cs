using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionBonusUIFeedback : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI bonusMessageText;
    [SerializeField] private Image bonusPanel;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 3f;

    private void Start()
    {
        // Esconder no início
        gameObject.SetActive(false);
    }

    public void ShowBonusActivatedFeedback()
    {
        Debug.Log("ShowBonusActivatedFeedback iniciado");

        // Garantir que o objeto esteja ativo
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            Debug.Log("BonusUIFeedback ativado");
        }

        // Verificar e definir a mensagem
        if (bonusMessageText != null)
        {
            bonusMessageText.text = "BÔNUS ATIVADO!\nXP Dobrado por 10min";
            Debug.Log("Texto de bônus definido");
        }
        else
        {
            Debug.LogError("bonusMessageText é null!");
        }

        // Verificar o painel
        if (bonusPanel != null)
        {
            // Garantir que o painel esteja visível
            Color panelColor = bonusPanel.color;
            panelColor.a = 1f;
            bonusPanel.color = panelColor;
            Debug.Log("Painel de bônus configurado com alpha 1");
        }
        else
        {
            Debug.LogError("bonusPanel é null!");
        }

        // Iniciar a animação de fade
        StartCoroutine(FadeInAndOut());
        Debug.Log("Coroutine FadeInAndOut iniciada");
    }

    private IEnumerator FadeInAndOut()
    {
        // Fade in
        if (bonusPanel != null)
        {
            Color startColor = bonusPanel.color;
            startColor.a = 0f;
            bonusPanel.color = startColor;

            float timeElapsed = 0f;
            while (timeElapsed < fadeDuration)
            {
                float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
                Color newColor = bonusPanel.color;
                newColor.a = alpha;
                bonusPanel.color = newColor;

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            Color finalColor = bonusPanel.color;
            finalColor.a = 1f;
            bonusPanel.color = finalColor;
        }

        // Esperar pelo tempo de exibição
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        if (bonusPanel != null)
        {
            float timeElapsed = 0f;
            while (timeElapsed < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
                Color newColor = bonusPanel.color;
                newColor.a = alpha;
                bonusPanel.color = newColor;

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            Color finalColor = bonusPanel.color;
            finalColor.a = 0f;
            bonusPanel.color = finalColor;
        }

        // Continuar mostrando o timer, mas esconder o painel de feedback
        gameObject.SetActive(false);
    }
}