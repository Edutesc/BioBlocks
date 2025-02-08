using UnityEngine;
using System.Collections.Generic;

public class SceneDataManager : MonoBehaviour
{
    private static SceneDataManager _instance;
    private Dictionary<string, object> _sceneData;

    public static SceneDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("SceneDataManager");
                _instance = go.AddComponent<SceneDataManager>();
                DontDestroyOnLoad(go);
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
        _sceneData = new Dictionary<string, object>();
    }

    public void SetData(Dictionary<string, object> data)
    {
        _sceneData = data;
        Debug.Log($"Data set in SceneDataManager: {string.Join(", ", data.Keys)}");
    }

    public Dictionary<string, object> GetData()
    {
        return _sceneData;
    }

    public T GetValue<T>(string key)
    {
        if (_sceneData.ContainsKey(key))
        {
            try
            {
                return (T)_sceneData[key];
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error getting value for key {key}: {e.Message}");
                return default(T);
            }
        }
        Debug.LogWarning($"Key {key} not found in scene data");
        return default(T);
    }

    public void ClearData()
    {
        _sceneData.Clear();
        Debug.Log("Scene data cleared");
    }
}
