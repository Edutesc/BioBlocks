using UnityEngine;
using System.Collections.Generic;

public class TermodinamicsQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions;

    private void Awake()
    {
        // Define as perguntas para "Scene1"
        questions = new List<Question>
        {
            new Question 
            {
                questionText = "Qual a capital do Veneto?",
                answers = new string[] {"Paris", "Veneza", "Roma", "Berlim"},
                correctIndex = 1
            },
            new Question 
            {
                questionText = "Qual a capital da Italia?",
                answers = new string[] {"Júpiter", "Saturno", "Terra", "Roma"},
                correctIndex = 3
            }
        };
    }

    public List<Question> GetQuestions()
    {
        return questions;
    }
}
