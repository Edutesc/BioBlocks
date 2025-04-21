using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AlertManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject alertPanel;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button okButton;

    private static AlertManager _instance;
    
    public static AlertManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AlertManager não encontrado na cena!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        if (alertPanel != null)
        {
            alertPanel.SetActive(false);
        }
        
        if (okButton != null)
        {
            okButton.onClick.AddListener(CloseAlert);
        }
    }

    public void ShowAlert(string message)
    {
        if (alertText != null)
        {
            alertText.text = message;
        }
        
        if (alertPanel != null)
        {
            alertPanel.SetActive(true);
        }
    }

    public void CloseAlert()
    {
        if (alertPanel != null)
        {
            alertPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (okButton != null)
        {
            okButton.onClick.RemoveListener(CloseAlert);
        }
    }
}
