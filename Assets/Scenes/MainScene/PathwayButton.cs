
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using QuestionSystem;

public class PathwayButton : MonoBehaviour
{
    [SerializeField] private QuestionSet questionSetToLoad;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on PathwayButton");
        }
    }

    public void OnButtonClick()
    {
        Debug.Log($"Loading question set {questionSetToLoad}");
        QuestionSetManager.SetCurrentQuestionSet(questionSetToLoad);
        SceneManager.LoadScene("QuestionScene");
    }
}


