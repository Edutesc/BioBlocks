using System.Collections.Generic;

public interface IFeedbackDatabase
{
    List<FeedbackQuestion> GetQuestions();
    string GetDatabaseName();
    string GetDatabaseDescription();
}

