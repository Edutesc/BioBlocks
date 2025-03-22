using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;


public class SpecialBonusHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestionBonusManager correctAnswerBonusManager;
    
    [Header("Special Bonus Configuration")]
    [SerializeField] private int specialBonusMultiplier = 3;
    [SerializeField] private float bonusDuration = 600f; // 10 minutos em segundos
    
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI specialBonusTimerText;
    [SerializeField] private GameObject specialBonusTimerContainer;
    
    private bool isSpecialBonusActive = false;
    private float specialBonusTimeRemaining = 0f;
    private Coroutine specialBonusTimerCoroutine = null;
    private SpecialBonusManager specialBonusManager;
    
    private void Awake()
    {
        specialBonusManager = new SpecialBonusManager();
    }
    
    private void Start()
    {
        if (correctAnswerBonusManager == null)
        {
            correctAnswerBonusManager = FindFirstObjectByType<QuestionBonusManager>();
        }
        
        if (specialBonusTimerContainer != null)
        {
            specialBonusTimerContainer.SetActive(false);
        }
        
        // Inicia a verificação do bonus ativo
        StartCoroutine(InitCheckForActiveBonus());
    }
    
    // Coroutine inicial que chama o método assíncrono
    private IEnumerator InitCheckForActiveBonus()
    {
        // Aguardar um curto período para garantir que outros sistemas foram inicializados
        yield return new WaitForSeconds(0.5f);
        
        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            Debug.LogWarning("SpecialBonusHandler: Usuário não está logado");
            yield break;
        }
        
        string userId = UserDataStore.CurrentUserData.UserId;
        
        // Iniciar a verificação assíncrona
        CheckForActiveSpecialBonus(userId);
    }
    
    // Método assíncrono separado para evitar usar await dentro da coroutine
    private async void CheckForActiveSpecialBonus(string userId)
    {
        try
        {
            List<BonusType> bonuses = await specialBonusManager.GetUserBonuses(userId);
            BonusType activeSpecialBonus = bonuses.FirstOrDefault(b => 
                b.BonusName == "active_specialBonus" && 
                b.IsBonusActive && 
                !b.IsExpired());
            
            if (activeSpecialBonus != null)
            {
                float remainingTime = activeSpecialBonus.GetRemainingSeconds();
                if (remainingTime > 0)
                {
                    ActivateSpecialBonusWithRemainingTime(remainingTime);
                    Debug.Log($"SpecialBonusHandler: Special Bonus ativo restaurado com {remainingTime} segundos restantes");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusHandler: Erro ao verificar bonus ativo: {e.Message}");
        }
    }
    
    private void ActivateSpecialBonusWithRemainingTime(float remainingSeconds)
    {
        isSpecialBonusActive = true;
        specialBonusTimeRemaining = remainingSeconds;
        
        if (specialBonusTimerContainer != null)
        {
            specialBonusTimerContainer.SetActive(true);
        }
        
        if (specialBonusTimerCoroutine != null)
        {
            StopCoroutine(specialBonusTimerCoroutine);
        }
        
        specialBonusTimerCoroutine = StartCoroutine(SpecialBonusTimerCoroutine());
        UpdateTimerDisplay();
    }
    
    private IEnumerator SpecialBonusTimerCoroutine()
    {
        float lastUpdateTime = specialBonusTimeRemaining;
        
        while (specialBonusTimeRemaining > 0)
        {
            UpdateTimerDisplay();
            
            yield return new WaitForSeconds(1f);
            specialBonusTimeRemaining -= 1f;
            
            // Atualizar no Firestore periodicamente
            if (lastUpdateTime - specialBonusTimeRemaining >= 30f || specialBonusTimeRemaining <= 10f)
            {
                lastUpdateTime = specialBonusTimeRemaining;
                UpdateBonusExpirationInFirestore();
            }
        }
        
        DeactivateSpecialBonus();
    }
    
    private async void UpdateBonusExpirationInFirestore()
    {
        if (!isSpecialBonusActive || UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            return;
        }
        
        string userId = UserDataStore.CurrentUserData.UserId;
        
        try
        {
            List<BonusType> bonuses = await specialBonusManager.GetUserBonuses(userId);
            BonusType activeSpecialBonus = bonuses.FirstOrDefault(b => b.BonusName == "active_specialBonus");
            
            if (activeSpecialBonus != null)
            {
                activeSpecialBonus.SetExpirationFromDuration(specialBonusTimeRemaining);
                await specialBonusManager.SaveBonusList(userId, bonuses);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusHandler: Erro ao atualizar timestamp: {e.Message}");
        }
    }
    
    private async void DeactivateSpecialBonus()
    {
        if (!isSpecialBonusActive)
        {
            return;
        }
        
        isSpecialBonusActive = false;
        
        if (specialBonusTimerContainer != null)
        {
            specialBonusTimerContainer.SetActive(false);
        }
        
        if (UserDataStore.CurrentUserData == null || string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            return;
        }
        
        string userId = UserDataStore.CurrentUserData.UserId;
        
        try
        {
            List<BonusType> bonuses = await specialBonusManager.GetUserBonuses(userId);
            BonusType activeSpecialBonus = bonuses.FirstOrDefault(b => b.BonusName == "active_specialBonus");
            
            if (activeSpecialBonus != null)
            {
                activeSpecialBonus.IsBonusActive = false;
                await specialBonusManager.SaveBonusList(userId, bonuses);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusHandler: Erro ao desativar bônus: {e.Message}");
        }
    }
    
    private void UpdateTimerDisplay()
    {
        if (specialBonusTimerText != null)
        {
            int minutes = Mathf.FloorToInt(specialBonusTimeRemaining / 60);
            int seconds = Mathf.FloorToInt(specialBonusTimeRemaining % 60);
            specialBonusTimerText.text = $"Bônus de XP triplicada ativo: {minutes:00}:{seconds:00}";
        }
    }
    
    public bool IsSpecialBonusActive()
    {
        return isSpecialBonusActive;
    }
    
    public int ApplyBonusToScore(int baseScore)
    {
        if (!isSpecialBonusActive || baseScore <= 0)
        {
            return baseScore;
        }
        
        return baseScore * specialBonusMultiplier;
    }
    
    public int GetCombinedScoreMultiplier(int correctAnswerMultiplier)
    {
        if (isSpecialBonusActive)
        {
            return correctAnswerMultiplier * specialBonusMultiplier;
        }
        
        return correctAnswerMultiplier;
    }
    
    private void OnDestroy()
    {
        if (specialBonusTimerCoroutine != null)
        {
            StopCoroutine(specialBonusTimerCoroutine);
        }
        
        if (isSpecialBonusActive && UserDataStore.CurrentUserData != null && !string.IsNullOrEmpty(UserDataStore.CurrentUserData.UserId))
        {
            // Chamar método não bloqueante
            SaveBonusStateOnExit(UserDataStore.CurrentUserData.UserId);
        }
    }
    
    // Mudado para método assíncrono normal ao invés de tentar usar Task não-await
    private async void SaveBonusStateOnExit(string userId)
    {
        try
        {
            List<BonusType> bonuses = await specialBonusManager.GetUserBonuses(userId);
            BonusType activeSpecialBonus = bonuses.FirstOrDefault(b => b.BonusName == "active_specialBonus");
            
            if (activeSpecialBonus != null)
            {
                activeSpecialBonus.SetExpirationFromDuration(specialBonusTimeRemaining);
                await specialBonusManager.SaveBonusList(userId, bonuses);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SpecialBonusHandler: Erro ao salvar estado do bônus: {e.Message}");
        }
    }
}