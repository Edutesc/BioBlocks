using UnityEngine;
using TMPro;

public class TopBarUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        ValidateComponents();
        UpdateUserInfo();
        UserDataStore.OnUserDataChanged += OnUserDataChanged;
    }

    private void OnDestroy()
    {
        UserDataStore.OnUserDataChanged -= OnUserDataChanged;
    }

    private void ValidateComponents()
    {
        if (nickNameText == null) Debug.LogError("NickNameText não atribuído");
        if (scoreText == null) Debug.LogError("ScoreText não atribuído");
    }

    private void UpdateUserInfo()
    {
        if (UserDataStore.CurrentUserData != null)
        {
            UpdateScore(UserDataStore.CurrentUserData.Score);
            UpdateName(UserDataStore.CurrentUserData.NickName);
        }
        else
        {
            Debug.LogError("CurrentUserData é null ao tentar atualizar UI");
        }
    }

    private void OnUserDataChanged(UserData userData)
    {
        if (userData != null)
        {
            UpdateScore(userData.WeekScore);
            UpdateName(userData.NickName);
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{score} XP";
        }
    }

    public void UpdateName(string name)
    {
        if (nickNameText != null)
        {
            nickNameText.text = name;
        }
    }
}
