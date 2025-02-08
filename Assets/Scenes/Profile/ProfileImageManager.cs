using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections;

public class ProfileImageManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private RawImage imageContent;
    [SerializeField] private Button imageButton;
    [SerializeField] private Sprite standardProfileImage;

    private UserData currentUserData;
    private const int MaxImageSizeBytes = 1024 * 1024; // 1MB
    private bool isProcessing = false;

    private void Start()
    {
        currentUserData = UserDataStore.CurrentUserData;
        ConfigureImageMask();

        if (imageButton != null)
        {
            imageButton.onClick.AddListener(OnImageClick);
        }
        else
        {
            Debug.LogError("Image Button não atribuído!");
        }

        LoadProfileImage();
    }

    private void ConfigureImageMask()
    {
        var maskObject = imageContent.transform.parent.gameObject;
        if (maskObject == null)
        {
            Debug.LogError("MaskObject não encontrado na hierarquia!");
            return;
        }

        var mask = maskObject.GetComponent<Mask>();
        if (mask == null)
        {
            mask = maskObject.AddComponent<Mask>();
        }

        mask.showMaskGraphic = true;
        var maskImage = maskObject.GetComponent<Image>();
        if (maskImage == null)
        {
            maskImage = maskObject.AddComponent<Image>();
        }

        maskImage.sprite = CreateCircleSprite(true); // true para inverter as cores
        maskImage.type = Image.Type.Simple;
        maskImage.color = Color.white;

        var maskRect = maskObject.GetComponent<RectTransform>();
        if (maskRect != null)
        {
            maskRect.anchorMin = Vector2.zero;
            maskRect.anchorMax = Vector2.one;
            maskRect.sizeDelta = Vector2.zero;
            maskRect.anchoredPosition = Vector2.zero;
        }
    }

    private Sprite CreateCircleSprite(bool invertMask = false)
    {
        int resolution = 256;
        Texture2D texture = new Texture2D(resolution, resolution);
        float radius = resolution / 2f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                // Inverter a lógica: centro branco, bordas transparentes
                Color color = distance < radius ? Color.white : Color.clear;
                if (invertMask)
                {
                    color = distance < radius ? Color.white : Color.clear;
                }
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

    private void LoadProfileImage()
    {
        if (currentUserData != null && !string.IsNullOrEmpty(currentUserData.ProfileImageUrl))
        {
            StartCoroutine(LoadImageFromUrl(currentUserData.ProfileImageUrl));
        }
        else
        {
            LoadStandardProfileImage();
        }
    }

    private void LoadStandardProfileImage()
    {
        if (standardProfileImage != null)
        {
            imageContent.texture = standardProfileImage.texture;
            Debug.Log("Imagem padrão configurada");
        }
        else
        {
            Debug.LogError("Standard Profile Image não está atribuída!");
        }
    }

    private System.Collections.IEnumerator LoadImageFromUrl(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                SetProfileImage(texture);
            }
            else
            {
                Debug.LogError($"Erro ao carregar imagem: {www.error}");
                LoadStandardProfileImage();
            }
        }
    }

    private void OnImageClick()
    {
        if (isProcessing || NativeGallery.IsMediaPickerBusy())
            return;

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (string.IsNullOrEmpty(path))
                return;

            StartCoroutine(ProcessSelectedImage(path));
        }, "Selecione uma imagem", "image/*");

        if (permission == NativeGallery.Permission.Denied)
        {
            Debug.LogWarning("Permissão para acessar a galeria negada");
        }
    }

    private System.Collections.IEnumerator ProcessSelectedImage(string imagePath)
    {
        isProcessing = true;
        imageButton.interactable = false;

        // Verificar tamanho do arquivo
        FileInfo fileInfo = null;
        try
        {
            fileInfo = new FileInfo(imagePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao acessar arquivo: {e.Message}");
            isProcessing = false;
            imageButton.interactable = true;
            yield break;
        }

        if (fileInfo.Length > MaxImageSizeBytes)
        {
            Debug.LogError("Imagem muito grande. Máximo 1MB");
            isProcessing = false;
            imageButton.interactable = true;
            yield break;
        }

        // Carregar e redimensionar imagem
        byte[] imageBytes = null;
        Texture2D texture = null;
        try
        {
            imageBytes = File.ReadAllBytes(imagePath);
            texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            SetProfileImage(texture);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao carregar imagem: {e.Message}");
            isProcessing = false;
            imageButton.interactable = true;
            yield break;
        }

        // Deletar imagem antiga se existir
        if (!string.IsNullOrEmpty(currentUserData.ProfileImageUrl))
        {
            bool success = false;
            yield return StartCoroutine(DeleteOldProfileImage(currentUserData.ProfileImageUrl, (result) =>
            {
                success = result;
            }));

            if (!success)
            {
                Debug.LogWarning("Falha ao deletar imagem antiga, mas continuando com o upload da nova imagem");
            }
        }

        // Upload da nova imagem
        string userId = currentUserData.UserId;
        string fileName = $"profile_images/{userId}_{DateTime.UtcNow.Ticks}.jpg";

        bool uploadSuccess = false;
        string imageUrl = null;
        yield return StartCoroutine(UploadNewProfileImage(fileName, imageBytes, (success, url) =>
        {
            uploadSuccess = success;
            imageUrl = url;
        }));

        if (!uploadSuccess)
        {
            Debug.LogError("Falha no upload da nova imagem");
            isProcessing = false;
            imageButton.interactable = true;
            yield break;
        }

        bool updateSuccess = false;
        yield return StartCoroutine(UpdateProfileUrl(imageUrl, (success) =>
        {
            updateSuccess = success;
        }));

        if (!updateSuccess)
        {
            Debug.LogError("Falha ao atualizar URL do perfil");
        }

        isProcessing = false;
        imageButton.interactable = true;
    }

    private IEnumerator DeleteOldProfileImage(string imageUrl, Action<bool> callback)
    {
        var task = StorageRepository.Instance.DeleteProfileImageAsync(imageUrl);
        yield return task.AsCoroutine();
        HandleTaskCompletion(task, callback);
    }

    private IEnumerator UploadNewProfileImage(string fileName, byte[] imageBytes, Action<bool, string> callback)
    {
        var task = UploadImageAsync(fileName, imageBytes);
        yield return task.AsCoroutine();
        HandleUploadTaskCompletion(task, callback);
    }

    private IEnumerator UpdateProfileUrl(string imageUrl, Action<bool> callback)
    {
        var task = UpdateProfileImageUrlAsync(imageUrl);
        yield return task.AsCoroutine();
        HandleTaskCompletion(task, callback);
    }

    private void HandleTaskCompletion(Task task, Action<bool> callback)
    {
        if (task.IsFaulted)
        {
            Debug.LogError($"Erro na operação: {task.Exception?.Message}");
            callback(false);
        }
        else
        {
            callback(true);
        }
    }

    private void HandleUploadTaskCompletion(Task<string> task, Action<bool, string> callback)
    {
        if (task.IsFaulted)
        {
            Debug.LogError($"Erro no upload: {task.Exception?.Message}");
            callback(false, null);
        }
        else
        {
            callback(true, task.Result);
        }
    }

    private async Task<string> UploadImageAsync(string fileName, byte[] imageBytes)
    {
        try
        {
            return await StorageRepository.Instance.UploadImageAsync(fileName, imageBytes);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro no upload: {e.Message}");
            throw;
        }
    }

    private async Task UpdateProfileImageUrlAsync(string imageUrl)
    {
        try
        {
            await FirestoreRepository.Instance.UpdateUserProfileImageUrl(currentUserData.UserId, imageUrl);
            currentUserData.ProfileImageUrl = imageUrl;
            UserDataStore.CurrentUserData = currentUserData;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao atualizar URL: {e.Message}");
            throw;
        }
    }

    private void SetProfileImage(Texture2D texture)
    {
        if (imageContent != null)
        {
            if (imageContent.texture != null &&
                imageContent.texture != standardProfileImage?.texture &&
                imageContent.texture != texture)
            {
                Destroy(imageContent.texture);
            }
            imageContent.texture = texture;
        }
    }

    private void OnDestroy()
    {
        if (imageButton != null)
        {
            imageButton.onClick.RemoveListener(OnImageClick);
        }

        if (imageContent != null &&
            imageContent.texture != null &&
            imageContent.texture != standardProfileImage?.texture)
        {
            Destroy(imageContent.texture);
        }
    }
}