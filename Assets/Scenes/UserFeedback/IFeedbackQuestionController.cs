using System.Collections.Generic;
using UnityEngine;

public interface IFeedbackQuestionController
{
    void SetupQuestion(FeedbackQuestion question);
    bool Validate();
    KeyValuePair<string, object> GetResult();
    void SetVisible(bool visible);
    void ClearAnswer();
}
