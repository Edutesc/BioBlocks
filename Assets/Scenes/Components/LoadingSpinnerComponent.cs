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
        
        // Verificar/criar componentes necessários
        Initialize();
    }
    
    private void Initialize()
    {
        if (spinnerContainer != null && spinnerBackground != null && spinnerBorder != null)
        {
            Debug.Log("LoadingSpinnerComponent: Using existing UI components");
            return;
        }
        
        Debug.Log("LoadingSpinnerComponent: Creating UI components programmatically");
        
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10000;
        }
        
        // Adicionar CanvasScaler
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
        }
        
        // Adicionar GraphicRaycaster para bloquear interações
        GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            raycaster = gameObject.AddComponent<GraphicRaycaster>();
        }
        
        // Criar container do spinner
        spinnerContainer = new GameObject("SpinnerContainer");
        spinnerContainer.transform.SetParent(transform, false);
        
        // Adicionar background semi-transparente (opcional, pode ser removido)
        GameObject background = new GameObject("DarkBackground");
        background.transform.SetParent(spinnerContainer.transform, false);
        
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparente
        
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Criar o centro do spinner (fundo amarelo com letra B)
        GameObject centerObj = new GameObject("SpinnerBackground");
        centerObj.transform.SetParent(spinnerContainer.transform, false);
        spinnerBackground = centerObj.AddComponent<Image>();
        
        // Carregar sprite do fundo do spinner
        Sprite backgroundSprite = Resources.Load<Sprite>("UI/TEC_spinnerImage_300x300");
        if (backgroundSprite != null)
        {
            spinnerBackground.sprite = backgroundSprite;
        }
        else
        {
            // Se não encontrar, definir uma cor amarela básica
            spinnerBackground.color = new Color(1f, 0.84f, 0f); // Amarelo
        }
        
        RectTransform centerRect = centerObj.GetComponent<RectTransform>();
        centerRect.anchorMin = new Vector2(0.5f, 0.5f);
        centerRect.anchorMax = new Vector2(0.5f, 0.5f);
        centerRect.sizeDelta = new Vector2(150, 150);
        
        // Criar a borda giratória (vermelha)
        GameObject borderObj = new GameObject("SpinnerBorder");
        borderObj.transform.SetParent(spinnerContainer.transform, false);
        spinnerBorder = borderObj.AddComponent<Image>();
        
        // Carregar sprite da borda do spinner
        Sprite borderSprite = Resources.Load<Sprite>("UI/miniLogo_bioBlocks_border");
        if (borderSprite != null)
        {
            spinnerBorder.sprite = borderSprite;
        }
        else
        {
            // Se não encontrar, definir uma cor vermelha básica
            spinnerBorder.color = new Color(0.8f, 0.2f, 0.2f); // Vermelho
        }
        
        RectTransform borderRect = borderObj.GetComponent<RectTransform>();
        borderRect.anchorMin = new Vector2(0.5f, 0.5f);
        borderRect.anchorMax = new Vector2(0.5f, 0.5f);
        borderRect.sizeDelta = new Vector2(200, 200); // Ligeiramente maior que o background
        
        // Inicialmente escondido
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
        // Rotacionar apenas a borda, se estiver ativo
        if (spinnerBorder != null && spinnerContainer.activeSelf)
        {
            spinnerBorder.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        
        // Rotacionar o background apenas se configurado para tal
        if (rotateBackground && spinnerBackground != null && spinnerContainer.activeSelf)
        {
            spinnerBackground.transform.Rotate(0, 0, rotationSpeed * 0.2f * Time.deltaTime);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (waitForSceneLoad && scene.name == sceneToWaitFor)
        {
            // Esperar um frame para garantir que a cena está totalmente carregada
            StartCoroutine(HideSpinnerDelayed(0.2f));
        }
    }
    
    private IEnumerator HideSpinnerDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideSpinner();
    }
    
    /// <summary>
    /// Mostra o spinner e o mantém visível até que uma cena específica seja carregada
    /// </summary>
    public void ShowSpinnerUntilSceneLoaded(string sceneName)
    {
        waitForSceneLoad = true;
        sceneToWaitFor = sceneName;
        ShowSpinner();
        
        Debug.Log($"Spinner will be visible until scene '{sceneName}' is loaded");
    }
    
    /// <summary>
    /// Mostra o spinner
    /// </summary>
    public void ShowSpinner()
    {
        if (spinnerContainer != null)
        {
            spinnerContainer.SetActive(true);
            Debug.Log("Global spinner shown");
        }
    }
    
    /// <summary>
    /// Esconde o spinner
    /// </summary>
    public void HideSpinner()
    {
        if (spinnerContainer != null)
        {
            spinnerContainer.SetActive(false);
            waitForSceneLoad = false;
            sceneToWaitFor = string.Empty;
            Debug.Log("Global spinner hidden");
        }
    }
}