

using System.Collections.Generic;
using QuestionSystem;

namespace QuestionSystem
{
    public interface IQuestionDatabase
    {
        List<Question> GetQuestions();
        QuestionSet GetQuestionSetType();
        string GetDatabankName();
    }
}


