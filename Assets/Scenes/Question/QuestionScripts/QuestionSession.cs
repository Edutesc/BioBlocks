using System;
using System.Collections.Generic;
using System.Linq;
using QuestionSystem;

public class QuestionSession
{
    private List<Question> questions;
    private int currentQuestionIndex;
    private string databankName;

    public QuestionSession(List<Question> questions)
    {
        this.questions = questions ?? new List<Question>();
        this.currentQuestionIndex = 0;
        
        if (this.questions.Any())
        {
            this.databankName = this.questions[0].questionDatabankName;
        }
    }

    public int CurrentQuestionIndex => currentQuestionIndex;
    public List<Question> Questions => questions;
    public string DatabankName => databankName;
    public bool HasMoreQuestions => currentQuestionIndex < questions.Count;

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