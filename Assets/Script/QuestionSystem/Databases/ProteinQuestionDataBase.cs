using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class ProteinQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
         new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "A = Ponte de Hidrogênio, B = Interação Eletrostática, C = Interação Hidrofóbica, D = Ponte Dissulfeto",
                "A = Ponte de Dissulfeto, B = Interação Eletrostática, C = Interação Hidrofóbica, D = Ponte de Hidrogênio",
                "A = Interação Hidrofóbica, B = Ponte de Hidrogênio, C = Ponte Dissulfeto, D = Interação Eletrostática",
                "A = Interação Eletrostática, B = Ponte de Hidrogênio, C = Interação Hidrofóbica, D = Ponte Dissulfeto",
            },
            correctIndex = 2,
            questionNumber = 1,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_1"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Que tipo de ligação une os aminoácidos na estrutura primária de uma proteína?",
            answers = new string[] {
                "Ligações de hidrogênio",
                "Ligações iônicas",
                "Ligações peptídicas",
                "Pontes dissulfeto"
            },
            correctIndex = 2,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A formação de uma ligação peptídica envolve:",
            answers = new string[] {
                "A união de dois aminoácidos com a perda de uma molécula de água.",
                "A união de dois aminoácidos com a adição de uma molécula de água.",
                "A união de três aminoácidos com a perda de uma molécula de água.",
                "A união de três aminoácidos com a adição de uma molécula de água."
            },
            correctIndex = 0,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Identifique abaixo o aminoácido que pode formar pontes dissulfeto em proteínas",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/cisteína",
                "AnswerImages/AminoacidsDB/treonina",
                "AnswerImages/AminoacidsDB/alanina",
                "AnswerImages/AminoacidsDB/fenilalanina"
            },
            correctIndex = 1,
            questionNumber = 4,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Que tipo de estrutura protéica é formada pela união de duas ou mais cadeias polipeptídicas?",
            answers = new string[] {
                "Primária",
                "Secundária",
                "Terciária",
                "Quaternária"
            },
            correctIndex = 3,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "Uma estrutura formada basicamente por alfa-hélices",
                "Uma estrutura formada basicamente por fitas-betas",
                "A imagem representa apenas a estrutura primária de uma proteína",
                "Não há estrutura definida",
            },
            correctIndex = 1,
            questionNumber = 1,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_6"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Quais são os principais elementos de estrutura secundária?",
            answers = new string[] {
                "Alfa-hélices e folhas beta",
                "Alfa-hélices e pontes dissulfeto",
                "Folhas beta e ligações peptídicas",
                "Voltas e ligações iônicas"
            },
            correctIndex = 0,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "Interações hidrofóbicas",
                "Pontes de hidrogênio",
                "Ligações iônicas",
                "Pontes dissulfeto"
            },
            correctIndex = 1,
            questionNumber = 8,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_8"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/acido_aspartico",
                "AnswerImages/AminoacidsDB/glutamina",
                "AnswerImages/AminoacidsDB/arginina",
                "AnswerImages/AminoacidsDB/prolina"
            },
            correctIndex = 3,
            questionNumber = 9,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_9"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A estrutura terciária de uma proteína se refere a:",
            answers = new string[] {
                "Seqüência de aminoácidos.",
                "Arranjo espacial de alfa-hélices e folhas beta.",
                "Interações entre diferentes cadeias polipeptídicas.",
                "Dobramento tridimensional da proteína."
            },
            correctIndex = 3,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Quais forças mantêm a estrutura terciária de uma proteína?",
            answers = new string[] {
                "Apenas ligações peptídicas.",
                "Pontes de hidrogênio, interações hidrofóbicas, interações iônicas e pontes dissulfeto.",
                "Apenas pontes dissulfeto.",
                "Apenas ligações de hidrogênio."
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Qual técnica utiliza cristais de proteína para determinar sua estrutura?",
            answers = new string[] {
                "Espectrometria de massas",
                "Cristalografia de raios-X",
                "Ressonância magnética nuclear (RMN)",
                "Microscopia eletrônica"
            },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/acido_aspartico",
                "AnswerImages/AminoacidsDB/treonina",
                "AnswerImages/AminoacidsDB/arginina",
                "AnswerImages/AminoacidsDB/isoleucina"
            },
            correctIndex = 1,
            questionNumber = 13,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_13"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/triptofano",
                "AnswerImages/AminoacidsDB/isoleucina",
                "AnswerImages/AminoacidsDB/arginina",
                "AnswerImages/AminoacidsDB/fenilalanina"
            },
            correctIndex = 1,
            questionNumber = 14,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_14"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
             questionText = "Identifique abaixo a melhor representação de estrutura secundaria",
            answers = new string[] {
                "AnswerImages/ProteinDB/estrutura_primaria",
                "AnswerImages/ProteinDB/estrutura_secundaria",
                "AnswerImages/ProteinDB/estrutura_terciaria",
                "AnswerImages/ProteinDB/estrutura_quaternaria"
            },
            correctIndex = 1,
            questionNumber = 15,
            isImageAnswer = true
        },
                new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
             questionText = "Identifique abaixo a melhor representação de estrutura quaternaria",
            answers = new string[] {
                "AnswerImages/ProteinDB/estrutura_primaria",
                "AnswerImages/ProteinDB/estrutura_secundaria",
                "AnswerImages/ProteinDB/estrutura_terciaria",
                "AnswerImages/ProteinDB/estrutura_quaternaria"
            },
            correctIndex = 3,
            questionNumber = 16,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
             questionText = "Identifique abaixo a melhor representação de estrutura primaria",
            answers = new string[] {
                "AnswerImages/ProteinDB/estrutura_primaria",
                "AnswerImages/ProteinDB/estrutura_secundaria",
                "AnswerImages/ProteinDB/estrutura_terciaria",
                "AnswerImages/ProteinDB/estrutura_quaternaria"
            },
            correctIndex = 0,
            questionNumber = 17,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
             questionText = "Identifique abaixo a melhor representação de estrutura terciária",
            answers = new string[] {
                "AnswerImages/ProteinDB/estrutura_primaria",
                "AnswerImages/ProteinDB/estrutura_secundaria",
                "AnswerImages/ProteinDB/estrutura_terciaria",
                "AnswerImages/ProteinDB/estrutura_quaternaria"
            },
            correctIndex = 2,
            questionNumber = 18,
            isImageAnswer = true
        },
         new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "alfa-hélices",
                "fitas-beta",
                "folhas-beta",
                "beta-hélices",
            },
            correctIndex = 0,
            questionNumber = 19,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_19"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "alfa-hélices",
                "fitas-beta",
                "fitas-alfa",
                "beta-hélices",
            },
            correctIndex = 1,
            questionNumber = 20,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_20"
        },
       
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O colágeno é uma proteína fibrosa encontrada em:",
            answers = new string[] {
                "Cabelos",
                "Ossos, cartilagens e tendões.",
                "Músculos",
                "Enzimas"
            },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A estrutura do colágeno é:",
            answers = new string[] {
                "Uma única hélice alfa.",
                "Uma dupla hélice alfa.",
                "Uma tripla hélice.",
                "Uma folha beta."
            },
            correctIndex = 2,
            questionNumber = 34,
            isImageAnswer = false
        },
         new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A técnica de eletroforese SDS-Page separa proteínas com base em:",
            answers = new string[] {
                "Seu tamanho.",
                "Sua carga líquida.",
                "Sua sequência de aminoácidos.",
                "Sua estrutura terciária."
            },
            correctIndex = 0,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Em eletroforese com focalização isoelétrica, um aminoácido com carga líquida positiva migrará para o pólo:",
            answers = new string[] {
                "Positivo",
                "Negativo",
                "Não migrará",
                "Depende do pH"
            },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Em eletroforese com focalização isoelétrica, um aminoácido com carga líquida negativa migrará para o pólo:",
            answers = new string[] {
                "Negativo",
                "Positivo",
                "Não migrará",
                "Depende do pH"
            },
            correctIndex = 1,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Em eletroforese com focalização isoelétrica, um aminoácido no seu ponto isoelétrico:",
            answers = new string[] {
                "Migrará para o pólo positivo.",
                "Migrará para o pólo negativo.",
                "Não migrará.",
                "Migrará para ambos os pólos."
            },
            correctIndex = 2,
            questionNumber = 18,
            isImageAnswer = false
        },
    };

    public List<Question> GetQuestions()
    {
        return questions;
    }

    public QuestionSet GetQuestionSetType()
    {
        return QuestionSet.proteins;
    }

    public string GetDatabankName()
    {
        return "ProteinQuestionDatabase";
    }
}