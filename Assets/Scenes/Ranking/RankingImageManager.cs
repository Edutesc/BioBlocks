using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RankingImageManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private RawImage imageContent;
    [SerializeField] private Sprite standardProfileImage;

    private void Awake()
    {
        if (imageContent == null)
        {
            Debug.LogWarning("ImageContent não atribuído no RankingImageManager");
            // Não vamos procurar automaticamente aqui, esperaremos o SetImageContent
        }
        else
        {
            ConfigureImageMask();
        }
    }

    public void SetImageContent(RawImage rawImage)
    {
        imageContent = rawImage;
        ConfigureImageMask();
        LoadStandardProfileImage(); // Carrega a imagem padrão imediatamente
    }

    private void ConfigureImageMask()
    {
        // Encontrar ou criar o MaskObject (deve ser o pai do RawImage)
        var maskObject = imageContent.transform.parent.gameObject;

        // Configurar a máscara
        var mask = maskObject.GetComponent<Mask>();
        if (mask == null)
        {
            mask = maskObject.AddComponent<Mask>();
        }
        mask.showMaskGraphic = true;

        // Configurar o Image da máscara
        var maskImage = maskObject.GetComponent<Image>();
        if (maskImage == null)
        {
            maskImage = maskObject.AddComponent<Image>();
        }

        // Criar e aplicar o sprite circular
        maskImage.sprite = CreateCircleSprite();
        maskImage.type = Image.Type.Simple;
        maskImage.color = Color.white;

        // Configurar o RectTransform do MaskObject para ter o tamanho correto
        var maskRect = maskObject.GetComponent<RectTransform>();
        if (maskRect != null)
        {
            maskRect.sizeDelta = new Vector2(100, 100); // Ajuste conforme necessário
        }

        // Configurar o RawImage para preencher o espaço da máscara
        var imageRect = imageContent.GetComponent<RectTransform>();
        if (imageRect != null)
        {
            imageRect.anchorMin = Vector2.zero;
            imageRect.anchorMax = Vector2.one;
            imageRect.sizeDelta = Vector2.zero;
            imageRect.anchoredPosition = Vector2.zero;
        }
    }

    private Sprite CreateCircleSprite()
    {
        int resolution = 128;
        Texture2D texture = new Texture2D(resolution, resolution);
        float radius = resolution / 2f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                Color color = distance < radius ? Color.white : Color.clear;
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return Sprite.Create(texture,
            new Rect(0, 0, resolution, resolution),
            Vector2.one * 0.5f,
            100f,
            0,
            SpriteMeshType.Tight);
    }

    public void LoadProfileImage(string imageUrl)
    {
        Debug.Log($"LoadProfileImage chamado com URL: {imageUrl}");

        if (string.IsNullOrEmpty(imageUrl))
        {
            Debug.Log("URL vazia, carregando imagem padrão...");
            LoadStandardProfileImage();
            return;
        }

        Debug.Log($"Tentando carregar imagem da URL: {imageUrl}");
        StartCoroutine(LoadImageFromUrl(imageUrl));
    }

    private IEnumerator LoadImageFromUrl(string url)
    {
        Debug.Log($"Iniciando download da imagem: {url}");
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                SetProfileImage(texture);
                Debug.Log("Imagem carregada com sucesso da URL");
            }
            else
            {
                Debug.LogError($"Erro ao carregar imagem da URL: {www.error}");
                Debug.Log("Carregando imagem padrão após falha...");
                LoadStandardProfileImage();
            }
        }
    }

    private void LoadStandardProfileImage()
    {
        Debug.Log("Tentando carregar imagem padrão...");
        if (standardProfileImage == null)
        {
            Debug.LogError("StandardProfileImage não está atribuída!");
            CreateAndSetPlaceholderTexture();
            return;
        }

        if (standardProfileImage.texture == null)
        {
            Debug.LogError("StandardProfileImage tem Sprite mas não tem textura!");
            CreateAndSetPlaceholderTexture();
            return;
        }

        if (imageContent == null)
        {
            Debug.LogError("ImageContent é null ao tentar carregar imagem padrão!");
            return;
        }

        imageContent.texture = standardProfileImage.texture;
        Debug.Log("Imagem padrão configurada com sucesso");
    }

    private void CreateAndSetPlaceholderTexture()
    {
        Debug.Log("Criando textura placeholder...");
        var texture = new Texture2D(128, 128);
        Color[] colors = new Color[128 * 128];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.gray;
        }
        texture.SetPixels(colors);
        texture.Apply();

        SetProfileImage(texture);
        Debug.Log("Textura placeholder criada e aplicada");
    }

    private void SetProfileImage(Texture texture)
    {
        if (imageContent != null)
        {
            imageContent.texture = texture;
            // Garante que a imagem seja visível
            imageContent.color = Color.white;
            Debug.Log($"SetProfileImage: Textura definida com sucesso: {texture}");

            // Verifica e loga as configurações do RawImage
            var rect = imageContent.GetComponent<RectTransform>();
            Debug.Log($"RawImage RectTransform - Width: {rect.rect.width}, Height: {rect.rect.height}");
            Debug.Log($"RawImage Color: {imageContent.color}");
            Debug.Log($"RawImage UV Rect: {imageContent.uvRect}");
        }
        else
        {
            Debug.LogError("SetProfileImage: ImageContent é null!");
        }
    }

    private void OnDestroy()
    {
        if (imageContent != null && imageContent.texture != null
            && imageContent.texture != standardProfileImage?.texture)
        {
            Destroy(imageContent.texture);
        }
    }
}
