using System;
using System.Collections.Generic;
using Firebase.Firestore;

public class FeedbackData
{
    [FirestoreProperty]
    public bool PreviousExperience { get; set; }

    [FirestoreProperty]
    public int FamiliarityLevel { get; set; }

    [FirestoreProperty]
    public int EaseOfUse { get; set; }

    [FirestoreProperty]
    public int InterfaceRating { get; set; }

    [FirestoreProperty]
    public string TechnicalIssues { get; set; }

    [FirestoreProperty]
    public int ContentReviewRating { get; set; }

    [FirestoreProperty]
    public int PerformanceImprovement { get; set; }

    [FirestoreProperty]
    public int MotivationLevel { get; set; }

    [FirestoreProperty]
    public string UsageFrequency { get; set; }

    [FirestoreProperty]
    public string UsagePurpose { get; set; }

    [FirestoreProperty]
    public string SessionDuration { get; set; }

    [FirestoreProperty]
    public string BestFeatures { get; set; }

    [FirestoreProperty]
    public string ImprovementSuggestions { get; set; }

    [FirestoreProperty]
    public bool WouldRecommend { get; set; }

    [FirestoreProperty]
    public string RecommendationReason { get; set; }

    [FirestoreProperty]
    public DateTime SubmissionDate { get; set; }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { "previousExperience", PreviousExperience },
            { "familiarityLevel", FamiliarityLevel },
            { "easeOfUse", EaseOfUse },
            { "interfaceRating", InterfaceRating },
            { "technicalIssues", TechnicalIssues },
            { "contentReviewRating", ContentReviewRating },
            { "performanceImprovement", PerformanceImprovement },
            { "motivationLevel", MotivationLevel },
            { "usageFrequency", UsageFrequency },
            { "usagePurpose", UsagePurpose },
            { "sessionDuration", SessionDuration },
            { "bestFeatures", BestFeatures },
            { "improvementSuggestions", ImprovementSuggestions },
            { "wouldRecommend", WouldRecommend },
            { "recommendationReason", RecommendationReason },
            { "submissionDate", DateTime.UtcNow }
        };
    }
}
