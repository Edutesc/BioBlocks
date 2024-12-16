using UnityEngine;
using System.Collections.Generic;

public class CellArchQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions;

    private void Awake()
    {
        // Define as perguntas para "Scene1"
        questions = new List<Question>
        {
            new Question 
            {
                questionText = "Qual o maelhor jogador de futebol de todos os tempos?",
                answers = new string[] {"Obina", "Marcio Araújo", "Matheuzinho", "Zico"},
                correctIndex = 3
            },
            new Question 
            {
                questionText = "Qual o melhor clube de futebol do universo conhecido?",
                answers = new string[] {"Ibis", "XV de Piracicaba", "Flamengo", "Guanari da Paraíba"},
                correctIndex = 2
            }
        };
    }

    public List<Question> GetQuestions()
    {
        return questions;
    }
}
