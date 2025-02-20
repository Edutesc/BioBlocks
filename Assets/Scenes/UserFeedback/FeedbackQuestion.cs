using UnityEngine;

public class FeedbackQuestion
{
    public string id;
    public string titleText;
    public string helperText;
    public FeedbackAnswerType feedbackAnswerType;
    public bool isRequired;
    public string category;
    
    // Opções para Text
    public int maxCharacters = 500;
    public string placeholderText;
    
    // Opções para Rating
    public string minRatingLabel = "1 = Nenhuma";
    public string maxRatingLabel = "5 = Muito alta";
    public int maxRating = 5;
    
    // Opções para Toggle
    public string toggleTrueLabel = "Sim";
    public string toggleFalseLabel = "Não";
}

public enum FeedbackAnswerType
{
    Text,
    Rating,
    Toggle
}

