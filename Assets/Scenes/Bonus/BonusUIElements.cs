using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BonusUIElements
{
    public string bonusFirestoreName;
    public TextMeshProUGUI bonusCountText;
    public TextMeshProUGUI isBonusActiveText;
    public Button bonusButton;  // Alterado de Image para Button
    public GameObject bonusContainer;
    
    // Cores personalizadas (opcionais)
    public ColorBlock customColors;
    public bool useCustomColors = false;
}

