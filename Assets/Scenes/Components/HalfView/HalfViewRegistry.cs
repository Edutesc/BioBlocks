using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sistema global de registro para componentes HalfViewComponent.
/// Permite o acesso e ativação de HalfViews em qualquer cena.
/// </summary>
public static class HalfViewRegistry
{
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
}
