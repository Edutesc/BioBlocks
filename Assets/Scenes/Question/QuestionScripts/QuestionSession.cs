using System;
using System.Collections.Generic;
using System.Linq;
using QuestionSystem;

public class QuestionSession
{
    private List<Question> questions;
    private int currentQuestionIndex;
    private string databankName;

    // Construtor que aceita a lista de questões
    public QuestionSession(List<Question> questions)
    {
        this.questions = questions ?? new List<Question>();
        this.currentQuestionIndex = 0;
        
        // Se houver questões, pega o databankName da primeira
        if (this.questions.Any())
        {
            this.databankName = this.questions[0].questionDatabankName;
        }
    }

    // Propriedades públicas
    public int CurrentQuestionIndex => currentQuestionIndex;
    public List<Question> Questions => questions;
    public string DatabankName => databankName;
    public bool HasMoreQuestions => currentQuestionIndex < questions.Count;

    // Métodos
    public Question GetCurrentQuestion()
    {
        if (currentQuestionIndex < 0 || currentQuestionIndex >= questions.Count)
        {
            throw new InvalidOperationException("Não há questão atual disponível");
        }
        return questions[currentQuestionIndex];
    }

    public void NextQuestion()
    {
        if (HasMoreQuestions)
        {
            currentQuestionIndex++;
        }
    }

    public bool IsLastQuestion()
    {
        return currentQuestionIndex >= questions.Count - 1;
    }

    public int GetTotalQuestions()
    {
        return questions.Count;
    }

    public void Reset()
    {
        currentQuestionIndex = 0;
    }
}
