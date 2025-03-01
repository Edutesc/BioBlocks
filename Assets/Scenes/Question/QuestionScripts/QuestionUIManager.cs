using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QuestionSystem;

public class QuestionUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image questionImage;

    private Sprite preloadedQuestionImage;
    private bool isPreloading = false;

    private void Start()
    {
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (questionText == null) Debug.LogError("QuestionText não atribuído");
        if (questionImage == null) Debug.LogError("QuestionImage não atribuído");
    }

    public void ShowQuestion(Question question)
    {
        if (question.isImageQuestion)
        {
            ShowImageQuestion(question);
        }
        else
        {
            ShowTextQuestion(question);
        }
    }

    private void ShowImageQuestion(Question question)
    {
        // Exibe texto da questão (mesmo para questões com imagem)
        questionText.text = question.questionText;
        
        // Se a imagem foi pré-carregada, usa ela imediatamente
        if (preloadedQuestionImage != null && !string.IsNullOrEmpty(question.questionImagePath))
        {
            questionImage.sprite = preloadedQuestionImage;
            questionImage.gameObject.SetActive(true);
            preloadedQuestionImage = null; // Limpa a referência
        }
        // Caso contrário, carrega normalmente
        else if (!string.IsNullOrEmpty(question.questionImagePath))
        {
            StartCoroutine(LoadQuestionImage(question.questionImagePath));
        }
    }

    private void ShowTextQuestion(Question question)
    {
        questionText.text = question.questionText;
        // Garante que a imagem está oculta para questões de texto
        questionImage.gameObject.SetActive(false);
    }

    private IEnumerator LoadQuestionImage(string imagePath)
    {
        var request = Resources.LoadAsync<Sprite>(imagePath);
        yield return request;

        if (request.asset != null)
        {
            questionImage.sprite = request.asset as Sprite;
            questionImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"Failed to load question image at path: {imagePath}");
            questionImage.gameObject.SetActive(false);
        }
    }

    // Novo método para pré-carregar a imagem da próxima questão
    public async Task PreloadQuestionImage(Question questionToPreload)
    {
        if (!questionToPreload.isImageQuestion || string.IsNullOrEmpty(questionToPreload.questionImagePath))
        {
            preloadedQuestionImage = null;
            return;
        }

        if (isPreloading)
            return;

        isPreloading = true;
        
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(PreloadImageCoroutine(questionToPreload.questionImagePath, tcs));
        await tcs.Task;
        
        isPreloading = false;
    }

    private IEnumerator PreloadImageCoroutine(string imagePath, TaskCompletionSource<bool> tcs)
    {
        var request = Resources.LoadAsync<Sprite>(imagePath);
        yield return request;

        if (request.asset != null)
        {
            preloadedQuestionImage = request.asset as Sprite;
            tcs.SetResult(true);
        }
        else
        {
            Debug.LogWarning($"Failed to preload question image at path: {imagePath}");
            preloadedQuestionImage = null;
            tcs.SetResult(false);
        }
    }

    public void ClearPreloadedResources()
    {
        preloadedQuestionImage = null;
    }
}