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
            isImageAnswer = false,
            isImageQuestion = false
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
            isImageAnswer = false,
            isImageQuestion = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Identifique abaixo o aminoácido que pode formar pontes dissulfeto em proteínas",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/cisteina",
                "AnswerImages/AminoacidsDB/treonina",
                "AnswerImages/AminoacidsDB/alanina",
                "AnswerImages/AminoacidsDB/fenilalanina"
            },
            correctIndex = 0,
            questionNumber = 4,
            isImageAnswer = true,
            isImageQuestion = false
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
            isImageAnswer = false,
            isImageQuestion = false
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
            correctIndex = 0,
            questionNumber = 6,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_6"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Quais são os principais elementos de estrutura secundária?",
            answers = new string[] {
                "Alfa-hélices e fitas-betas",
                "Alfa-hélices e pontes dissulfeto",
                "Fitas-betas e ligações peptídicas",
                "Voltas e ligações iônicas"
            },
            correctIndex = 0,
            questionNumber = 7,
            isImageAnswer = false,
            isImageQuestion = false
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
            isImageAnswer = false,
            isImageQuestion = true,
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
            isImageQuestion = true,
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
                "Estrutura tridimensional de uma sequência de aminoácidos."
            },
            correctIndex = 3,
            questionNumber = 10,
            isImageAnswer = false,
            isImageQuestion = false
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
            isImageAnswer = false,
            isImageQuestion = false
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
            questionNumber = 12,
            isImageAnswer = true,
            isImageQuestion = false
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
            correctIndex = 3,
            questionNumber = 13,
            isImageAnswer = true,
            isImageQuestion = true,
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
            correctIndex = 2,
            questionNumber = 14,
            isImageAnswer = true,
            isImageQuestion = true,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_14"
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
            questionNumber = 15,
            isImageAnswer = true,
            isImageQuestion = false
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
            questionNumber = 16,
            isImageAnswer = true,
            isImageQuestion = false
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
            questionNumber = 17,
            isImageAnswer = true,
            isImageQuestion = false
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
            questionNumber = 18,
            isImageAnswer = false,
            isImageQuestion = true,
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
            questionNumber = 19,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath =  "AnswerImages/ProteinDB/proteinQuestion_20"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "Cromatografia de focalização isoelétrica",
                "Cromatografia de gel filtração",
                "Cromatrografia de troca iônica",
                "Cromatrografia de afinidade",
            },
            correctIndex = 2,
            questionNumber = 20,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath =  "AnswerImages/ProteinDB/methodsQuestions20"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "Cromatografia de focalização isoelétrica",
                "Cromatografia de gel filtração",
                "Cromatrografia de troca iônica",
                "Cromatrografia de afinidade",
            },
            correctIndex = 1,
            questionNumber = 21,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath =  "AnswerImages/ProteinDB/methodsQuestions21"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Imagine uma mistura de duas proteínas (15 kDa e 45 kDa). Usando uma coluna de exclusão que interage com proteínas de até 20 kDa, qual proteína sairá primeiro da coluna?",
            answers = new string[] {
                "Ambas saírão juntas",
                "12 kDa sairá primeiro",
                "45 kDa sairá primeiro",
                "A coluna não conseguirá separar as proteínas",
            },
            correctIndex = 2,
            questionNumber = 22,
            isImageAnswer = false,
            isImageQuestion = false
        },
                new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Imagine uma mistura de duas proteínas com pI = 6 e pI = 12. Usando uma coluna trocadora de cátions, qual proteína sairá primeiro da coluna com uma fase móvel com pH = 9?",
            answers = new string[] {
                "Ambas saírão juntas",
                "Proteína com pI = 6 sairá primeiro",
                "Proteína com pI = 9 sairá primeiro",
                "A coluna não conseguirá separar as proteínas",
            },
            correctIndex = 1,
            questionNumber = 23,
            isImageAnswer = false,
            isImageQuestion = false
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
            questionNumber = 24,
            isImageAnswer = false,
            isImageQuestion = false
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
            questionNumber = 25,
            isImageAnswer = false,
            isImageQuestion = false
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
            questionNumber = 26,
            isImageAnswer = false,
            isImageQuestion = false
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
            questionNumber = 27,
            isImageAnswer = false,
            isImageQuestion = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "A curva na cor vermelha refere-se a uma proteína",
                "A curva na cor azul refere-se a uma proteína",
                "A curva na cor vermelha refere-se ao aminoácido alanina",
                "A curva na cor azul refere-se ao aminoácido leucina"
            },
            correctIndex = 1,
            questionNumber = 28,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath = "AnswerImages/ProteinDB/methodsQuestions28"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "Transportar oxigênio",
                "Manter estrutura das hemácias",
                "Contração dos pulmões",
                "Armazenar oxigênio"
            },
            correctIndex = 3,
            questionNumber = 29,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath = "AnswerImages/ProteinDB/function_and_structure29"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "Transportar oxigênio",
                "Manter estrutura das hemácias",
                "Contração dos pulmões",
                "Armazenar oxigênio"
            },
            correctIndex = 0,
            questionNumber = 30,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath = "AnswerImages/ProteinDB/function_and_structure30"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "Hemoglobina",
                "Mioglobina",
                "Não há diferença",
                "Ambas as proteínas tem baixa afinidade por oxigênio"
            },
            correctIndex = 1,
            questionNumber = 31,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath = "AnswerImages/ProteinDB/function_and_structure31"
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            answers = new string[] {
                "A mioglobina tem uma estrutura quaternária que facilita a ligação com O<sub><size=150%>2</size></sub>",
                "A hemoblogina pode assumir duas conformações, R e T, que têm diferentes afinidades por O<sub><size=150%>2</size></sub> ",
                "As proteínas não apresentam diferenças de afinidade por O<sub><size=150%>2</size></sub>",
                "Porque a hoglobina liga-se a quatro grupos heme"
            },
            correctIndex = 1,
            questionNumber = 32,
            isImageAnswer = false,
            isImageQuestion = true,
            questionImagePath = "AnswerImages/ProteinDB/function_and_structure32"
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