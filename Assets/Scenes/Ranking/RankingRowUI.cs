using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingRowUI : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] public TMP_Text totalScoreText;
    [SerializeField] public TMP_Text weekScoreText;
    [SerializeField] private Image backgroundImage;

    [Header("Image Components")]
    [SerializeField] private RawImage profileImage;
    [SerializeField] private RankingImageManager imageManager;

    [Header("Layout Elements")]
    [SerializeField] private LayoutElement rankLayout;
    [SerializeField] private LayoutElement profileLayout;
    [SerializeField] private LayoutElement nickNameLayout;
    [SerializeField] private LayoutElement weekScoreLayout;
    [SerializeField] private LayoutElement totalScoreLayout;

    private RectTransform rectTransform;
    private bool isExtraRow = false;
    private bool isWeekRankingMode = false; // Flag para controlar o modo de exibição

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Auto-encontrar o RankingImageManager se não estiver atribuído
        if (imageManager == null)
        {
            imageManager = GetComponent<RankingImageManager>();
            if (imageManager == null)
            {
                Debug.LogError("RankingImageManager não encontrado! Adicionando...");
                imageManager = gameObject.AddComponent<RankingImageManager>();
            }
        }

        // Auto-encontrar o profileImage se não estiver atribuído
        if (profileImage == null)
        {
            profileImage = GetComponentInChildren<RawImage>();
            if (profileImage != null)
            {
                imageManager.SetImageContent(profileImage);
            }
            else
            {
                Debug.LogError("ProfileImage (RawImage) não encontrado na hierarquia!");
            }
        }

        ConfigureLayout();
    }

    private void ConfigureLayout()
    {
        // Configurar RectTransform para garantir altura adequada
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.pivot = new Vector2(0.5f, 0);
            rectTransform.sizeDelta = new Vector2(0, 150);
        }

        // Configurar HorizontalLayoutGroup
        var horizontalLayout = GetComponent<HorizontalLayoutGroup>();
        if (horizontalLayout == null)
            horizontalLayout = gameObject.AddComponent<HorizontalLayoutGroup>();

        horizontalLayout.padding = new RectOffset(15, 15, 5, 5);
        horizontalLayout.spacing = 12;
        horizontalLayout.childAlignment = TextAnchor.MiddleLeft;
        horizontalLayout.childControlWidth = false;
        horizontalLayout.childControlHeight = false;
        horizontalLayout.childForceExpandWidth = false;
        horizontalLayout.childForceExpandHeight = false;

        ConfigureLayoutElements();
    }

    protected virtual void ConfigureLayoutElements()
    {
        if (rankLayout != null)
        {
            rankLayout.preferredWidth = 40;
            rankLayout.minWidth = 40;
        }

        if (profileLayout != null)
        {
            profileLayout.preferredWidth = 50;
            profileLayout.preferredHeight = 50;
            profileLayout.minWidth = 50;
            profileLayout.minHeight = 50;
        }

        if (nickNameLayout != null)
        {
            nickNameLayout.preferredWidth = 300;
            nickNameLayout.minWidth = 200;
            nickNameLayout.flexibleWidth = 1; // Permite expandir
        }

        if (totalScoreLayout != null)
        {
            totalScoreLayout.preferredWidth = 200;
            totalScoreLayout.minWidth = 170;
        }

        if (weekScoreLayout != null)
        {
            weekScoreLayout.preferredWidth = 180;
            weekScoreLayout.minWidth = 150;
        }
    }

    public void Setup(int rank, string userName, int score, string profileImageUrl, bool isCurrentUser)
    {
        // Chama o novo método, passando 0 como weekScore
        Setup(rank, userName, score, 0, profileImageUrl, isCurrentUser);
    }

    public void Setup(int rank, string userName, int totalScore, int weekScore, string profileImageUrl, bool isCurrentUser)
    {
        if (imageManager == null)
        {
            Debug.LogError("RankingImageManager é null em Setup!");
            return;
        }

        rankText.text = isExtraRow ? "..." : $"{rank}.";
        nickNameText.text = userName;
        totalScoreText.text = $"Total de Pontos: {totalScore} XP"; // Formato como na imagem 1

        if (weekScoreText != null)
            weekScoreText.text = $"{weekScore} XP"; // Formato como na imagem 1

        SetupColors(rank, isCurrentUser);

        Debug.Log($"Tentando carregar imagem para {userName}: {profileImageUrl}");
        imageManager.LoadProfileImage(profileImageUrl);
    }

    public void SetupAsExtraRow(int actualRank, string userName, int score, string profileImageUrl)
    {
        isExtraRow = true;
        Setup(actualRank, userName, score, 0, profileImageUrl, true);
    }

    public void SetupAsExtraRow(int actualRank, string userName, int totalScore, int weekScore, string profileImageUrl)
    {
        isExtraRow = true;
        Setup(actualRank, userName, totalScore, weekScore, profileImageUrl, true);
    }

    private void SetupColors(int rank, bool isCurrentUser)
    {
        Color textColor;
        Color backgroundColor;

        if (rank <= 3)
        {
            switch (rank)
            {
                case 1: // Primeiro lugar - Verde
                    ColorUtility.TryParseHtmlString("#00824B", out textColor);
                    ColorUtility.TryParseHtmlString("#91FF7D", out backgroundColor);
                    break;
                case 2: // Segundo lugar - Azul
                    ColorUtility.TryParseHtmlString("#004699", out textColor);
                    ColorUtility.TryParseHtmlString("#7DF7FF", out backgroundColor);
                    break;
                case 3: // Terceiro lugar - Amarelo
                    ColorUtility.TryParseHtmlString("#EF9102", out textColor);
                    ColorUtility.TryParseHtmlString("#FFFCB5", out backgroundColor);
                    break;
                default:
                    ColorUtility.TryParseHtmlString("#000000", out textColor);
                    ColorUtility.TryParseHtmlString("#FFFFFF", out backgroundColor);
                    break;
            }
        }

        else
        {
            if (isCurrentUser)
            {
                ColorUtility.TryParseHtmlString("#000000", out textColor);
                ColorUtility.TryParseHtmlString("#E5E5E5", out backgroundColor);
            }
            else
            {
                ColorUtility.TryParseHtmlString("#000000", out textColor);
                ColorUtility.TryParseHtmlString("#FFFFFF", out backgroundColor);
            }
        }

        if (rankText) rankText.color = textColor;
        if (nickNameText) nickNameText.color = textColor;
        if (totalScoreText) totalScoreText.color = textColor;
        if (weekScoreText) weekScoreText.color = textColor;
        if (backgroundImage) backgroundImage.color = backgroundColor;
    }
}