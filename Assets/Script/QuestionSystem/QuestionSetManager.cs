

using QuestionSystem;

public static class QuestionSetManager
{
    private static QuestionSet currentQuestionSet = QuestionSet.biochem;

    public static void SetCurrentQuestionSet(QuestionSet set)
    {
        currentQuestionSet = set;
    }

    public static QuestionSet GetCurrentQuestionSet()
    {
        return currentQuestionSet;
    }
}
