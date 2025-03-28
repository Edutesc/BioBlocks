// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
// using UnityEngine.TestTools;

// public class ScoreManagerTests
// {
//     private GameObject scoreManagerObject;
//     private ScoreManager scoreManager;
//     private Text scoreText;

//     [SetUp]
//     public void Setup()
//     {
//         // Criação de um objeto GameObject para o ScoreManager
//         scoreManagerObject = new GameObject("ScoreManager");
//         scoreManager = scoreManagerObject.AddComponent<ScoreManager>();

//         // Criar um objeto de UI Text para simular o scoreText
//         scoreText = new GameObject("ScoreText").AddComponent<Text>();
//         scoreManager.scoreText = scoreText; // Atribuir o scoreText ao ScoreManager

//         // Limpeza do estado do score (se necessário)
//        // scoreManager.ResetScore(); // Você poderá precisar implementar este método caso não exista
//     }
    

//     [TearDown]
//     public void Teardown()
//     {
//         // Limpeza após cada teste usando DestroyImmediate
//         Object.DestroyImmediate(scoreManagerObject);
//         Object.DestroyImmediate(scoreText.gameObject);

//          // Resetar a referência do singleton e o estado do scoreManager
//         ScoreManager.instance = null; 

//         // Se possível, re-instanciar o ScoreManager ou resetar sua pontuação
//         scoreManager = null; // Isso será reinicializado no próximo Setup
//     }

//     [UnityTest]
//     public IEnumerator AddScore_IncreasesScoreAndUpdatesText()
//     {
//         scoreManager.AddScore(10);

//         // Aguardando a atualização do UI
//         yield return null;

//         // Assert
//         Assert.AreEqual(10, scoreManager.GetScore());
//         Assert.AreEqual("Score: 10", scoreText.text);
//     }

//     [UnityTest]
//     public IEnumerator AddScore_AfterMultipleAdds_UpdatesCorrectly()
//     {
//         // Arrange
//         scoreManager.AddScore(5);
//         scoreManager.AddScore(15);

//         // Aguardando a atualização do UI
//         yield return null;

//         // Assert
//         Assert.AreEqual(20, scoreManager.GetScore());
//         Assert.AreEqual("Score: 20", scoreText.text);
//     }

//     [UnityTest]
//     public IEnumerator SingletonInstance_RemainsConsistent()
//     {
//         // Arrange
//     var scoreManager2GameObject = new GameObject("ScoreManager2");
//     var scoreManager2 = scoreManager2GameObject.AddComponent<ScoreManager>();

//     // Assert
//     Assert.AreEqual(ScoreManager.instance, scoreManager);
    
//     // Limpar a instância extra
//     Object.DestroyImmediate(scoreManager2GameObject);

//     // Return null to indicate completion
//     yield return null; // Pode ser apenas null se você não precisar da espera
//     }
// }




