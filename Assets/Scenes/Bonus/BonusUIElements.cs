using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BonusUIElements
{
    public string bonusFirestoreName;
    public TextMeshProUGUI bonusCountText;
    public TextMeshProUGUI isBonusActiveText;
    public Button bonusButton;
    public GameObject bonusContainer;
    
    // Títulos e mensagens para HalfView
    public string bonusTitle;
    public string bonusMessage;
    
    // Cores personalizadas (opcionais)
    public ColorBlock customColors;
    public bool useCustomColors = false;
}

[System.Serializable]
public class BonusConfig
{
    public float duration = 600f;
    public int multiplier = 2;
    public int thresholdCount = 1;
}
