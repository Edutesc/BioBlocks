using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class NavigationManager : MonoBehaviour
{
    private static NavigationManager _instance;
    
    public static NavigationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("NavigationManager");
                _instance = go.AddComponent<NavigationManager>();
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
    }

    public void NavigateTo(string sceneName, Dictionary<string, object> sceneData = null)
    {
        Debug.Log($"Navigating to: {sceneName}");
        try
        {
            if (sceneData != null)
            {
                SceneDataManager.Instance.SetData(sceneData);
            }
            SceneManager.LoadScene(sceneName);
            Debug.Log($"Successfully loaded scene: {sceneName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading scene {sceneName}: {e.Message}");
        }
    }
}