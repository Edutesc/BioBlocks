using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RetrySystem : MonoBehaviour
{
    [SerializeField] private GameObject retryPanel;
    [SerializeField] private TMP_Text errorMessageText;
    [SerializeField] private Button retryButton;

    private void Start()
    {
        retryButton.onClick.AddListener(HandleRetry);
        Hide();
    }

    public void Show(string errorMessage)
    {
        errorMessageText.text = errorMessage;
        retryPanel.SetActive(true);
    }

    public void Hide()
    {
        retryPanel.SetActive(false);
    }

    private void HandleRetry()
    {
        Hide();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
