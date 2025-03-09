using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CircularProgressIndicator : MonoBehaviour
{
    [Header("Configurações de Visual")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text percentageText;
    [SerializeField] private float fillAnimationDuration = 0.5f;
    [SerializeField] private Color lowProgressColor = new Color(0.5f, 0.5f, 1f); // Azul claro
    [SerializeField] private Color midProgressColor = new Color(0.3f, 0.3f, 1f); // Azul médio
    [SerializeField] private Color highProgressColor = new Color(0.1f, 0.1f, 0.8f); // Azul escuro
    
    [Header("Detecção Automática (Opcional)")]
    [SerializeField] private string databaseNameSuffix = "PorcentageText";
    
    // Removida a referência ao CanvasGroup, não precisamos mais dela
    // private CanvasGroup canvasGroup;
    
    private float targetFillAmount = 0f;
    private float currentFillAmount = 0f;
    private float fillVelocity = 0f;
    private string databaseName;
    
    private void Awake()
    {
        // Removida a linha que buscava o CanvasGroup
        
        // Detectar automaticamente o nome do banco de dados a partir do nome do objeto
        if (percentageText == null)
        {
            percentageText = GetComponentInChildren<TMP_Text>();
        }
        
        if (percentageText != null)
        {
            string objName = percentageText.gameObject.name;
            if (objName.EndsWith(databaseNameSuffix))
            {
                databaseName = objName.Substring(0, objName.Length - databaseNameSuffix.Length);
                Debug.Log($"CircularProgressIndicator detectou banco de dados: {databaseName}");
            }
        }
        
        // Configuração inicial
        if (fillImage != null)
        {
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Radial360;
            fillImage.fillOrigin = (int)Image.Origin360.Top;
            fillImage.fillClockwise = true;
            fillImage.fillAmount = 0f;
        }
    }
    
    private void Start()
    {
        // Se o PathwayManager estiver ativo, registrar para o evento
        AnsweredQuestionsManager.OnAnsweredQuestionsUpdated += HandleAnsweredQuestionsUpdated;
        
        // Não precisamos mais definir opacidade inicial
    }
    
    private void OnDestroy()
    {
        AnsweredQuestionsManager.OnAnsweredQuestionsUpdated -= HandleAnsweredQuestionsUpdated;
    }
    
    private void Update()
    {
        // Animação suave do preenchimento
        if (currentFillAmount != targetFillAmount)
        {
            currentFillAmount = Mathf.SmoothDamp(
                currentFillAmount, 
                targetFillAmount, 
                ref fillVelocity, 
                fillAnimationDuration);
            
            if (Mathf.Abs(currentFillAmount - targetFillAmount) < 0.01f)
            {
                currentFillAmount = targetFillAmount;
            }
            
            // Atualizar visual
            UpdateVisuals(currentFillAmount);
        }
    }
    
    private void HandleAnsweredQuestionsUpdated(Dictionary<string, int> answeredCounts)
    {
        if (string.IsNullOrEmpty(databaseName) || !answeredCounts.ContainsKey(databaseName))
        {
            return;
        }
        
        int count = answeredCounts[databaseName];
        int totalQuestions = QuestionBankStatistics.GetTotalQuestions(databaseName);
        
        if (totalQuestions <= 0) totalQuestions = 50; // Valor padrão
        
        // Calcular a porcentagem
        int percentage = totalQuestions > 0 ? (count * 100) / totalQuestions : 0;
        percentage = Mathf.Min(percentage, 100); // Garantir máximo de 100%
        
        // Atualizar texto
        if (percentageText != null)
        {
            percentageText.text = $"{percentage}%";
        }
        
        // Definir o alvo para animação
        targetFillAmount = percentage / 100f;
        
        // Log para debug
        Debug.Log($"CircularProgress para {databaseName}: {percentage}% ({count}/{totalQuestions})");
    }
    
    public void SetProgress(int percentage)
    {
        // Método público para definir o progresso diretamente
        percentage = Mathf.Clamp(percentage, 0, 100);
        
        if (percentageText != null)
        {
            percentageText.text = $"{percentage}%";
        }
        
        targetFillAmount = percentage / 100f;
    }
    
    private void UpdateVisuals(float fillAmount)
    {
        // Atualizar preenchimento
        if (fillImage != null)
        {
            fillImage.fillAmount = fillAmount;
            
            // Atualizar cor baseado no progresso
            if (fillAmount < 0.3f)
            {
                fillImage.color = lowProgressColor;
            }
            else if (fillAmount < 0.7f)
            {
                fillImage.color = midProgressColor;
            }
            else
            {
                fillImage.color = highProgressColor;
            }
        }
        
        // Removidas todas as linhas relacionadas à opacidade
    }
}