using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSpinnerComponent : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject spinnerContainer;
    [SerializeField] private Image spinnerBackground;
    [SerializeField] private Image spinnerBorder;

    [Header("Configuration")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool rotateBackground = false;

    private static LoadingSpinnerComponent _instance;
    private bool waitForSceneLoad = false;
    private string sceneToWaitFor = string.Empty;
    private bool isInitialized = false;

    public static LoadingSpinnerComponent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LoadingSpinnerComponent>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("GlobalLoadingSpinner");
                    _instance = go.AddComponent<LoadingSpinnerComponent>();
                    DontDestroyOnLoad(go);
                }
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
        DontDestroyOnLoad(gameObject);

        if (!isInitialized)
        {
            SafeInitialize();
        }
    }

    private void SafeInitialize()
    {
        try
        {
            if (spinnerContainer == null || spinnerBackground == null || spinnerBorder == null)
            {
                CreateSpinnerUI();
            }
            
            isInitialized = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error initializing LoadingSpinnerComponent: {e.Message}");
        }
    }

    private void CreateSpinnerUI()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10000;
        }
        else
        {
            canvas.sortingOrder = 10000;
        }

        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
        }

        GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            raycaster = gameObject.AddComponent<GraphicRaycaster>();
        }

        spinnerContainer = new GameObject("SpinnerContainer");
        spinnerContainer.transform.SetParent(transform, false);

        GameObject background = new GameObject("DarkBackground");
        background.transform.SetParent(spinnerContainer.transform, false);

        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.5f);
        bgImage.raycastTarget = true;
        
        CanvasGroup canvasGroup = background.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        GameObject centerObj = new GameObject("SpinnerBackground");
        centerObj.transform.SetParent(spinnerContainer.transform, false);
        spinnerBackground = centerObj.AddComponent<Image>();
        Sprite backgroundSprite = Resources.Load<Sprite>("UI/TEC_spinnerImage_300x300");

        if (backgroundSprite != null)
        {
            spinnerBackground.sprite = backgroundSprite;
        }
        else
        {
            spinnerBackground.color = new Color(1f, 0.84f, 0f);
        }

        RectTransform centerRect = centerObj.GetComponent<RectTransform>();
        centerRect.anchorMin = new Vector2(0.5f, 0.5f);
        centerRect.anchorMax = new Vector2(0.5f, 0.5f);
        centerRect.sizeDelta = new Vector2(150, 150);
        GameObject borderObj = new GameObject("SpinnerBorder");
        borderObj.transform.SetParent(spinnerContainer.transform, false);
        spinnerBorder = borderObj.AddComponent<Image>();
        Sprite borderSprite = Resources.Load<Sprite>("UI/miniLogo_bioBlocks_border");

        if (borderSprite != null)
        {
            spinnerBorder.sprite = borderSprite;
        }
        else
        {
            spinnerBorder.color = new Color(0.8f, 0.2f, 0.2f);
        }

        RectTransform borderRect = borderObj.GetComponent<RectTransform>();
        borderRect.anchorMin = new Vector2(0.5f, 0.5f);
        borderRect.anchorMax = new Vector2(0.5f, 0.5f);
        borderRect.sizeDelta = new Vector2(200, 200);
        spinnerContainer.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (spinnerContainer != null && spinnerContainer.activeSelf)
        {
            if (spinnerBorder != null)
            {
                spinnerBorder.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            }

            if (rotateBackground && spinnerBackground != null)
            {
                spinnerBackground.transform.Rotate(0, 0, rotationSpeed * 0.2f * Time.deltaTime);
            }
        }
    }

    public void VerifyAndFixSpinnerVisibility()
    {
        if (!isInitialized || spinnerContainer == null || spinnerBackground == null || spinnerBorder == null)
        {
            SafeInitialize();
        }

        if (spinnerContainer != null)
        {
            spinnerContainer.SetActive(true);

            if (spinnerBackground != null)
            {
                spinnerBackground.gameObject.SetActive(true);
            }

            if (spinnerBorder != null)
            {
                spinnerBorder.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Unable to show spinner - container is null after initialization");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (waitForSceneLoad && scene.name == sceneToWaitFor)
        {
            StartCoroutine(HideSpinnerDelayed(0.2f));
        }
    }

    private IEnumerator HideSpinnerDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideSpinner();
    }

    public void ShowSpinnerUntilSceneLoaded(string sceneName)
    {
        waitForSceneLoad = true;
        sceneToWaitFor = sceneName;
        ShowSpinner();
    }

    public void ShowSpinner()
    {
        VerifyAndFixSpinnerVisibility();
    }

    public void HideSpinner()
    {
        if (spinnerContainer != null)
        {
            spinnerContainer.SetActive(false);
            waitForSceneLoad = false;
            sceneToWaitFor = string.Empty;
        }
    }

    private void OnDrawGizmos()
    {
        if (spinnerContainer != null && spinnerContainer.activeSelf)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}