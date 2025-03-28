using System.Collections.Generic;
using UnityEngine;

public class UserFeedbackQuestionsDatabase : MonoBehaviour, IFeedbackDatabase
{
    private List<FeedbackQuestion> questions = new List<FeedbackQuestion>
    {
        // Perfil do Usuário
        new FeedbackQuestion
        {
            id = "previous_experience",
            titleText = "Você usa, ou já usou, outro aplicativo para estudar bioquímica antes deste?",
            feedbackAnswerType = FeedbackAnswerType.Text,
            isRequired = false,
            category = "Experiência com o Aplicativo",
            maxCharacters = 1000,
            placeholderText = "Se sim, qual aplicativo?"
        },
        new FeedbackQuestion
        {
            id = "familiarity_level",
            titleText = "Qual seu nível de familiaridade com bioquímica antes de usar o app?",
            feedbackAnswerType = FeedbackAnswerType.Rating,
            isRequired = false,
            category = "Perfil do Usuário",
            minRatingLabel = "1 = Nenhuma",
            maxRatingLabel = "5 = Muito alta",
            maxRating = 5
        },
        
        // Experiência com o Aplicativo
        new FeedbackQuestion
        {
            id = "ease_of_use",
            titleText = "O aplicativo foi fácil de usar?",
            feedbackAnswerType = FeedbackAnswerType.Rating,
            isRequired = false,
            category = "Experiência com o Aplicativo",
            minRatingLabel = "1 = Muito difícil",
            maxRatingLabel = "5 = Muito fácil",
            maxRating = 5
        },
        new FeedbackQuestion
        {
            id = "interface_rating",
            titleText = "Como você avalia a interface do aplicativo?",
            feedbackAnswerType = FeedbackAnswerType.Rating,
            isRequired = false,
            category = "Experiência com o Aplicativo",
            minRatingLabel = "1 = Péssima",
            maxRatingLabel = "5 = Excelente",
            maxRating = 5
        },
        new FeedbackQuestion
        {
            id = "technical_issues",
            titleText = "Você encontrou problemas técnicos ao usar o aplicativo?",
            helperText = "Se sim, por favor descreva-os brevemente",
            feedbackAnswerType = FeedbackAnswerType.Text,
            isRequired = false,
            category = "Experiência com o Aplicativo",
            maxCharacters = 1000,
            placeholderText = "Descreva aqui os problemas encontrados..."
        },
        
        // Impacto no Aprendizado
        new FeedbackQuestion
        {
            id = "content_review",
            titleText = "O aplicativo ajudou você a revisar melhor o conteúdo do curso?",
            feedbackAnswerType = FeedbackAnswerType.Rating,
            isRequired = false,
            category = "Impacto no Aprendizado",
            minRatingLabel = "1 = Nada",
            maxRatingLabel = "5 = Muito",
            maxRating = 5
        },
        new FeedbackQuestion
        {
            id = "performance_improvement",
            titleText = "O uso do aplicativo melhorou seu desempenho nas avaliações da disciplina?",
            feedbackAnswerType = FeedbackAnswerType.Rating,
            isRequired = false,
            category = "Impacto no Aprendizado",
            minRatingLabel = "1 = Não melhorou",
            maxRatingLabel = "5 = Melhorou muito",
            maxRating = 5
        },
        new FeedbackQuestion
        {
            id = "motivation_level",
            titleText = "Você se sentiu mais motivado a estudar bioquímica ao usar o aplicativo?",
            feedbackAnswerType = FeedbackAnswerType.Rating,
            isRequired = false,
            category = "Impacto no Aprendizado",
            minRatingLabel = "1 = Nada motivado",
            maxRatingLabel = "5 = Muito motivado",
            maxRating = 5
        },
        
        // Engajamento e Estratégias de Uso
        new FeedbackQuestion
        {
            id = "usage_frequency",
            titleText = "Com que frequência você usou o aplicativo?",
            feedbackAnswerType = FeedbackAnswerType.Rating,
            isRequired = false,
            category = "Engajamento e Estratégias de Uso",
            minRatingLabel = "1 vez por semana",
            maxRatingLabel = "5 vezes por semana",
            maxRating = 5
        },
        new FeedbackQuestion
        {
            id = "usage_purpose",
            titleText = "Você usou o aplicativo mais para revisar conteúdos já estudados ou para aprender novos conceitos?",
            feedbackAnswerType = FeedbackAnswerType.Text,
            isRequired = false,
            category = "Engajamento e Estratégias de Uso",
            maxCharacters = 300,
            placeholderText = "Por favor, comente..."
        },

        // Sugestões e Comentários
        new FeedbackQuestion
        {
            id = "best_features",
            titleText = "O que você mais gostou no aplicativo?",
            feedbackAnswerType = FeedbackAnswerType.Text,
            isRequired = false,
            category = "Sugestões e Comentários",
            maxCharacters = 500,
            placeholderText = "Por favor, comente..."
        },
        new FeedbackQuestion
        {
            id = "improvement_suggestions",
            titleText = "O que poderia ser melhorado no aplicativo?",
            feedbackAnswerType = FeedbackAnswerType.Text,
            isRequired = false,
            category = "Sugestões e Comentários",
            maxCharacters = 500,
            placeholderText = "Por favor, comente..."
        },
        new FeedbackQuestion
        {
            id = "would_recommend",
            titleText = "Você recomendaria este aplicativo para outros alunos de bioquímica?",
            feedbackAnswerType = FeedbackAnswerType.Toggle,
            isRequired = false,
            category = "Sugestões e Comentários",
            toggleTrueLabel = "Sim",
            toggleFalseLabel = "Não"
        },
        new FeedbackQuestion
        {
            id = "recommendation_reason",
            titleText = "Por favor, você poderia explicar o motivo da sua recomendação ou não recomendação.",
            feedbackAnswerType = FeedbackAnswerType.Text,
            isRequired = false,
            category = "Sugestões e Comentários",
            maxCharacters = 500,
            placeholderText = "Por favor, comente..."
        }
    };

    public List<FeedbackQuestion> GetQuestions()
    {
        return questions;
    }

    public string GetDatabaseName()
    {
        return "UserFeedbackQuestionsDatabase";
    }

    public string GetDatabaseDescription()
    {
        return "Feedback sobre usabilidade e impacto do aplicativo no aprendizado de bioquímica";
    }
}
