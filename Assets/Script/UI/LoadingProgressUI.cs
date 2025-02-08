using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingProgressUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject loadingSpinner;

    public void UpdateStatus(string status)
    {
        statusText.text = status;
    }

    public void UpdateProgress(float progress)
    {
        progressBar.fillAmount = progress;
    }
}

