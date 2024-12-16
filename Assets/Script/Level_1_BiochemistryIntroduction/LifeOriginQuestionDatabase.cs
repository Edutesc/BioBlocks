using UnityEngine;
using System.Collections.Generic;

public class LifeOriginQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions;

    private void Awake()
    {
        // Define as perguntas para "Scene1"
        questions = new List<Question>
        {
            new Question 
            {
                questionText = "Qual a capital da França?",
                answers = new string[] {"Paris", "Londres", "Roma", "Berlim"},
                correctIndex = 0
            },
            new Question 
            {
                questionText = "Qual o maior planeta do sistema solar?",
                answers = new string[] {"Júpiter", "Saturno", "Terra", "Marte"},
                correctIndex = 0
            }
        };
    }

    public List<Question> GetQuestions()
    {
        return questions;
    }
}
