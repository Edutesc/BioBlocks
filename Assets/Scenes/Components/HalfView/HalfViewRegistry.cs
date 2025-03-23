using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Sistema global de registro para componentes HalfViewComponent.
/// Permite o acesso e ativação de HalfViews em qualquer cena.
/// </summary>
public static class HalfViewRegistry
{

    private const string HALF_VIEW_PREFAB_PATH = "Prefabs/HalfViewComponent";

    private static Dictionary<string, HalfViewComponent> sceneHalfViews = new Dictionary<string, HalfViewComponent>();
    
    /// <summary>
    /// Registra um componente HalfViewComponent para uma cena específica.
    /// </summary>
    public static void RegisterHalfView(string sceneName, HalfViewComponent component)
    {
        sceneHalfViews[sceneName] = component;
        Debug.Log($"[HalfViewRegistry] Componente registrado para cena '{sceneName}'");
    }
    
    /// <summary>
    /// Remove o registro de um componente HalfViewComponent para uma cena.
    /// </summary>
    public static void UnregisterHalfView(string sceneName)
    {
        if (sceneHalfViews.ContainsKey(sceneName))
        {
            sceneHalfViews.Remove(sceneName);
            Debug.Log($"[HalfViewRegistry] Componente removido para cena '{sceneName}'");
        }
    }
    
    /// <summary>
    /// Obtém o componente HalfViewComponent registrado para uma cena específica.
    /// </summary>
    public static HalfViewComponent GetHalfViewForScene(string sceneName)
    {
        if (sceneHalfViews.TryGetValue(sceneName, out HalfViewComponent component))
        {
            return component;
        }
        return null;
    }
    
    /// <summary>
    /// Mostra o HalfViewComponent registrado para a cena atual.
    /// </summary>
    public static void ShowHalfViewForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        ShowHalfViewForScene(currentScene);
    }
    
    /// <summary>
    /// Mostra o HalfViewComponent registrado para uma cena específica.
    /// </summary>
    public static void ShowHalfViewForScene(string sceneName)
    {
        HalfViewComponent halfView = GetHalfViewForScene(sceneName);
        if (halfView != null)
        {
            halfView.ShowMenu();
            Debug.Log($"[HalfViewRegistry] HalfView mostrado para cena '{sceneName}'");
        }
        else
        {
            Debug.LogWarning($"[HalfViewRegistry] Nenhum HalfView registrado para cena '{sceneName}'");
        }
    }
    
    /// <summary>
    /// Esconde o HalfViewComponent registrado para a cena atual.
    /// </summary>
    public static void HideHalfViewForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        HideHalfViewForScene(currentScene);
    }
    
    /// <summary>
    /// Esconde o HalfViewComponent registrado para uma cena específica.
    /// </summary>
    public static void HideHalfViewForScene(string sceneName)
    {
        HalfViewComponent halfView = GetHalfViewForScene(sceneName);
        if (halfView != null)
        {
            halfView.HideMenu();
            Debug.Log($"[HalfViewRegistry] HalfView escondido para cena '{sceneName}'");
        }
    }
    
    /// <summary>
    /// Limpa todos os registros.
    /// </summary>
    public static void ClearRegistry()
    {
        sceneHalfViews.Clear();
        Debug.Log("[HalfViewRegistry] Registro limpo");
    }

     public static HalfViewComponent EnsureHalfViewInCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Verificar se já existe um HalfView registrado para esta cena
        HalfViewComponent existingComponent = GetHalfViewForScene(currentScene);
        if (existingComponent != null)
        {
            return existingComponent;
        }
        
        // Carregar o prefab
        GameObject halfViewPrefab = Resources.Load<GameObject>(HALF_VIEW_PREFAB_PATH);
        if (halfViewPrefab == null)
        {
            Debug.LogError($"[HalfViewRegistry] Prefab não encontrado em: {HALF_VIEW_PREFAB_PATH}");
            return null;
        }
        
        // Instanciar o prefab
        GameObject halfViewInstance = GameObject.Instantiate(halfViewPrefab);
        halfViewInstance.name = "HalfView";
        
        // Garantir que ele seja filho do Canvas principal
        Canvas mainCanvas = GetMainCanvas();
        if (mainCanvas != null)
        {
            halfViewInstance.transform.SetParent(mainCanvas.transform, false);
        }
        else
        {
            Debug.LogWarning("[HalfViewRegistry] Canvas principal não encontrado. O HalfView foi instanciado como um GameObject raiz.");
        }
        
        // Obter o componente e retorná-lo
        HalfViewComponent halfViewComponent = halfViewInstance.GetComponent<HalfViewComponent>();
        
        // Como o componente se registra durante o Awake(), ele já estará disponível
        // no registro. Se não, algo deu errado.
        if (GetHalfViewForScene(currentScene) == null)
        {
            Debug.LogError("[HalfViewRegistry] Falha ao registrar o HalfView recém-criado.");
        }
        
        return halfViewComponent;
    }

     private static Canvas GetMainCanvas()
    {
        // Primeiro, tenta encontrar um canvas com o nome "Canvas" (padrão do Unity)
        Canvas mainCanvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();
        
        // Se não encontrar, tenta outros nomes comuns
        if (mainCanvas == null)
        {
            mainCanvas = GameObject.Find("MainCanvas")?.GetComponent<Canvas>();
        }
        
        // Se ainda não encontrar, procura qualquer canvas na cena
        if (mainCanvas == null)
        {
            Canvas[] allCanvases = GameObject.FindObjectsByType<Canvas>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            if (allCanvases.Length > 0)
            {
                // Usar o canvas com maior sorting order, que provavelmente é o principal
                int highestOrder = -1;
                foreach (Canvas canvas in allCanvases)
                {
                    if (canvas.sortingOrder > highestOrder && canvas.renderMode != RenderMode.WorldSpace)
                    {
                        highestOrder = canvas.sortingOrder;
                        mainCanvas = canvas;
                    }
                }
                
                // Se todos forem WorldSpace, pegar o primeiro
                if (mainCanvas == null && allCanvases.Length > 0)
                {
                    mainCanvas = allCanvases[0];
                }
            }
        }
        
        // Se ainda não encontrou nenhum canvas, cria um novo
        if (mainCanvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        return mainCanvas;
    }

}
