using UnityEngine;

public class BioBlocksSettings : MonoBehaviour
{
    private static BioBlocksSettings _instance;

    [Header("Debug Settings")]
    [SerializeField] private bool isDebugMode = false;

    public static BioBlocksSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<BioBlocksSettings>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ProjectSettings");
                    _instance = go.AddComponent<BioBlocksSettings>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    #if DEBUG
        public const string VERSION = "3.0.0-dev";
        public const bool IS_DEBUG = true;
        public const string ENVIRONMENT = "Development";
    #else
        public const string VERSION = "3.0.0";
        public const bool IS_DEBUG = false;
        public const string ENVIRONMENT = "Production";
    #endif

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        #if RELEASE
            isDebugMode = false;
        #endif

        InitializeSettings();
    }

    private void InitializeSettings()
    {
        #if DEBUG
            Debug.Log($"[ProjectSettings] Initialized in {ENVIRONMENT} mode");
            Debug.Log($"[ProjectSettings] Version: {VERSION}");
            Debug.Log($"[ProjectSettings] Debug Mode: {isDebugMode}");
            Application.logMessageReceived += HandleLog;
        #else
            Debug.unityLogger.logEnabled = false;
        #endif
    }

    #if DEBUG
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            // Implementar sistema de analytics
        }
    }
    #endif

    public bool IsDebugMode()
    {
        #if RELEASE
            return false;
        #elif DEBUG
            return isDebugMode; 
        #else
            return false; 
        #endif
    }

    public void ToggleDebugMode()
    {
        #if DEBUG
            isDebugMode = !isDebugMode;
            Debug.Log($"[ProjectSettings] Debug Mode changed to: {isDebugMode}");
        #endif
    }

    public void SetDebugMode(bool debug)
    {
        #if DEBUG
            isDebugMode = debug;
            Debug.Log($"[ProjectSettings] Debug Mode set to: {isDebugMode}");
        #endif
    }
}