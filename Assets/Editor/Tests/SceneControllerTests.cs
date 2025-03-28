// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.TestTools;
// using UnityEditor; 
// using UnityEditor.SceneManagement;
// using System.Collections;
// using System.Linq;

// public class SceneControllerTests
// {
//     private GameObject sceneControllerObject;
//     private SceneController sceneController;

//     [SetUp]
//     public void Setup()
//     {
//         // Criação de um objeto GameObject para o SceneController
//         sceneControllerObject = new GameObject();
//         sceneController = sceneControllerObject.AddComponent<SceneController>();
//     }

//     [TearDown]
//     public void Teardown()
//     {
//         // Limpeza após cada teste usando DestroyImmediate
//         Object.DestroyImmediate(sceneControllerObject);
//     }

//     [UnityTest]
//     public IEnumerator LoadAllScenes_ShouldLoadAllScenes()
//     {
//         // Recupere todas as cenas dos Build Settings
//         var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();

//         // Itera sobre cada cena e tenta carregá-las
//         foreach (var scenePath in scenes)
//         {
//             // Extrai o nome da cena do caminho
//             string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
//             // Act: Usa EditorSceneManager para abrir a cena
//             EditorSceneManager.OpenScene(scenePath);

//             // Aguardando a cena ser carregada (é mais uma prática garantir o carregamento)
//             yield return null;

//             // Assert
//             Assert.AreEqual(sceneName, SceneManager.GetActiveScene().name);
//         }
//     }

//     [UnityTest]
//     public IEnumerator LoadScene_ShouldLoadScene()
//     {
//         // Arrange
//         string sceneName = "Pathway_Level_1"; // Use o nome real da cena que você gostaria de testar
        
//         // Act: Usa EditorSceneManager para abrir a cena
//         EditorSceneManager.OpenScene($"Assets/Scenes/Level_1/{sceneName}.unity"); // Certifique-se de que o caminho esteja correto

//         // Aguardando a cena ser carregada (é mais uma prática garantir o carregamento)
//         yield return null;

//         // Assert
//         Assert.AreEqual(sceneName, SceneManager.GetActiveScene().name);
//     }
// }