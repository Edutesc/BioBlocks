using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class AminoacidQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "O que define um aminoácido?",
            answers = new string[] {
                "Uma molécula orgânica com um grupo amino e um grupo carboxila.",
                "Uma molécula inorgânica com um grupo amino e um grupo carboxila.",
                "Uma molécula orgânica com apenas um grupo amino.",
                "Uma molécula inorgânica com apenas um grupo carboxila."
            },
            correctIndex = 0,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Qual o papel principal dos aminoácidos?",
            answers = new string[] {
                "Formar carboidratos.",
                "Formar lipídios.",
                "Formar proteínas.",
                "Formar ácidos nucléicos."
            },
            correctIndex = 2,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Identifique o aminoácido cuja cadeia lateral apresenta característica polar não carregada.",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/treonina",
                "AnswerImages/AminoacidsDB/glicina",
                "AnswerImages/AminoacidsDB/histidina",
                "AnswerImages/AminoacidsDB/alanina"
            },
            correctIndex = 0,
            questionNumber = 3,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "O que diferencia um aminoácido do outro?",
            answers = new string[] {
                "O grupo amino.",
                "O grupo carboxila.",
                "A sua cadeia lateral (R).",
                "O átomo de carbono alfa."
            },
            correctIndex = 2,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Identifique o alfa-aminoácido abaixo",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/alanina",
                "AnswerImages/AminoacidsDB/3-amino-2-butanona",
                "AnswerImages/AminoacidsDB/beta-alanina",
                "AnswerImages/AminoacidsDB/2-amino-propanoato-de-metila"
            },
            correctIndex = 0,
            questionNumber = 5,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com cadeias laterais alifáticas são:",
            answers = new string[] {
                "Polares.",
                "Apolares.",
                "Carregados positivamente.",
                "Carregados negativamente."
            },
            correctIndex = 1,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Identifique o aminoácido que absorve o comprimento de onda de 280 nm.",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/alanina",
                "AnswerImages/AminoacidsDB/treonina",
                "AnswerImages/AminoacidsDB/cisteina",
                "AnswerImages/AminoacidsDB/fenilalanina"
            },
            correctIndex = 3,
            questionNumber = 7,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/alanina",
                "AnswerImages/AminoacidsDB/treonina",
                "AnswerImages/AminoacidsDB/cisteina",
                "AnswerImages/AminoacidsDB/d-alanina"
            },
            correctIndex = 3,
            questionNumber = 8,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/AminoacidsDB/ImageQuestionContainer8"
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "pH = 2,3",
                "pH = 9,7",
                "pH = 6",
                "pH = 0"
            },
            correctIndex = 2,
            questionNumber = 9,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer9"
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/isoleucina",
                "AnswerImages/AminoacidsDB/isoleucina_carga0",
                "AnswerImages/AminoacidsDB/isoleucina_cargaPlus",
                "AnswerImages/AminoacidsDB/isoleucina_cargaMinus"
            },
            correctIndex = 2,
            questionNumber = 10,
            isImageAnswer = true,
            isImageQuestion = true,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer10"
        },
         new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/prolina",
                "AnswerImages/AminoacidsDB/prolina_carga0",
                "AnswerImages/AminoacidsDB/prolina_cargaPlus",
                "AnswerImages/AminoacidsDB/prolina_cargaMinus"
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = true,
            isImageQuestion = true,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer11"
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "pH = 5,5",
                "pH = 9,0",
                "pH = 10,7",
                "pH = 12,5"
            },
            correctIndex = 2,
            questionNumber = 12,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer12"
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "pH = 3,0",
                "pH = 5,5",
                "pH = 3,9",
                "pH = 9,8"
            },
            correctIndex = 0,
            questionNumber = 13,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer13"
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Os aminoácidos aspártico e glutâmico possuem:",
            answers = new string[] {
                "Um grupo carboxila no radical R.",
                "Um grupo amino no radical R.",
                "Um grupo sulfidrila no radical R.",
                "Um anel aromático no radical R."
            },
            correctIndex = 0,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Identifique abaixo o aminoácido cuja cadeia lateral é considerada básica",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/prolina",
                "AnswerImages/AminoacidsDB/isoleucina",
                "AnswerImages/AminoacidsDB/acido_aspartico",
                "AnswerImages/AminoacidsDB/arginina"
            },
            correctIndex = 3,
            questionNumber = 15,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Identifique abaixo o aminoácido cuja cadeia lateral é considerada ácida",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/fenilalanina",
                "AnswerImages/AminoacidsDB/alanina",
                "AnswerImages/AminoacidsDB/acido_aspartico",
                "AnswerImages/AminoacidsDB/arginina"
            },
            correctIndex = 2,
            questionNumber = 16,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
             questionText = "Identifique abaixo o aminoácido cuja cadeixa lateral apresenta um grupo funcional álcool.",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/tirosina",
                "AnswerImages/AminoacidsDB/prolina",
                "AnswerImages/AminoacidsDB/treonina",
                "AnswerImages/AminoacidsDB/leucina"
            },
            correctIndex = 2,
            questionNumber = 17,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em pH ácido, o estado de protonação da maioria dos aminoácidos presentes na solução terá carga líquida:",
            answers = new string[] {
                "Negativa",
                "Neutra",
                "Positiva",
                "Variável"
            },
            correctIndex = 2,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
             questionText = "Identifique abaixo o aminoácido que absorve luz de comprimento de onda 280 nm.",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/triptofano",
                "AnswerImages/AminoacidsDB/glutamina",
                "AnswerImages/AminoacidsDB/glicina",
                "AnswerImages/AminoacidsDB/alanina"
            },
            correctIndex = 0,
            questionNumber = 19,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em pH básico, o estado de protonação da maioria dos aminoácidos presentes na solução terá carga líquida:",
            answers = new string[] {
                "Positiva",
                "Neutra",
                "Negativa",
                "Variável"
            },
            correctIndex = 2,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "O que é um carbono quiral?",
            answers = new string[] {
                "Um carbono ligado a quatro átomos diferentes.",
                "Um carbono ligado a dois átomos iguais.",
                "Um carbono com dupla ligação.",
                "Um carbono com tripla ligação."
            },
            correctIndex = 0,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Uma molécula com um carbono quiral é:",
            answers = new string[] {
                "Apolar",
                "Assimétrica",
                "Linear",
                "Simétrica"
            },
            correctIndex = 1,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Isômeros que são imagens especulares um do outro, e NÃO são sobreponíveis:",
            answers = new string[] {
                "Enantiômeros",
                "Diasteroisômeros",
                "Isômeros constitucionais",
                "Isômeros conformacionais"
            },
            correctIndex = 0,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A designação D e L para aminoácidos se refere a:",
            answers = new string[] {
                "Sua composição química.",
                "Sua estrutura tridimensional.",
                "Sua solubilidade em água.",
                "Seu ponto isoelétrico."
            },
            correctIndex = 1,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Quais aminoácidos são encontrados principalmente nas proteínas?",
            answers = new string[] {
                "D-aminoácidos",
                "L-aminoácidos",
                "Ambos em quantidades iguais",
                "Depende do organismo"
            },
            correctIndex = 1,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos essenciais são aqueles que:",
            answers = new string[] {
                "Nosso corpo produz.",
                "Devem ser obtidos pela dieta.",
                "São encontrados em plantas.",
                "São encontrados em animais."
            },
            correctIndex = 1,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos não-essenciais são aqueles que:",
            answers = new string[] {
                "Devem ser obtidos pela dieta.",
                "Nosso corpo produz.",
                "São encontrados apenas em animais.",
                "São encontrados apenas em plantas."
            },
            correctIndex = 1,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "Uma amida",
                "H<sup><size=150%> +</size></sup>",
                "Água",
                "OH<sup><size=150%> -</size></sup>"
            },
            correctIndex = 2,
            questionNumber = 28,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer28"
        },
       new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Qual é o nome da ligação que ocorre entre os aminoácidos para forma proteínas",
            answers = new string[] {
                "Ponte de hidrogênio",
                "Ligação proteica",
                "Ligação peptídica",
                "Ligação eletrostática"
            },
            correctIndex = 2,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Qual o nome do grupo funcional é criado pela condensação de dois aminoácidos para formar um peptídeo?",
            answers = new string[] {
                "Grupo funcional álcool",
                "Grupo funcional amina",
                "Grupo funcional ácido carboxílico",
                "Grupo funcional amida"
            },
            correctIndex = 3,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "2 aminoácidos",
                "3 aminoácidos",
                "4 aminoácidos",
                "5 aminoácidos"
            },
            correctIndex = 2,
            questionNumber = 31,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer31"
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            answers = new string[] {
                "Aminoácido",
                "Dipeptídeo",
                "Tripeptídeo",
                "Tetrapeptídeo"
            },
            correctIndex = 3,
            questionNumber = 32,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/AminoacidsDB/imageQuestionContainer32"
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com carga líquida positiva em pH 7 são:",
            answers = new string[] {
                "Ácidos",
                "Básicos",
                "Apolares",
                "Neutros"
            },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com carga líquida negativa em pH 7 são:",
            answers = new string[] {
                "Básicos",
                "Ácidos",
                "Apolares",
                "Neutros"
            },
            correctIndex = 1,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "O ponto isoelétrico (pI) de um aminoácido é:",
            answers = new string[] {
                "O pH em que ele é completamente protonado.",
                "O pH em que ele é completamente desprotonado.",
                "O pH em que sua carga líquida é zero.",
                "O pH em que sua solubilidade é máxima."
            },
            correctIndex = 2,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Para um aminoácido com dois pKs, o pI é calculado como:",
            answers = new string[] {
                "A média dos dois pKs.",
                "A diferença entre os dois pKs.",
                "O maior dos dois pKs.",
                "O menor dos dois pKs."
            },
            correctIndex = 0,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em uma titulação de um aminoácido, os platôs na curva representam:",
            answers = new string[] {
                "Mudanças rápidas de pH.",
                "Mudanças lentas de pH.",
                "Dissociação de grupamentos ionizáveis.",
                "Adição de ácido ou base."
            },
            correctIndex = 2,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "O pK de um grupamento ionizável em um aminoácido representa:",
            answers = new string[] {
                "O pH em que o grupamento está completamente protonado.",
                "O pH em que o grupamento está completamente desprotonado.",
                "O pH em que metade do grupamento está protonado e metade desprotonado.",
                "O pH em que o aminoácido tem carga líquida zero."
            },
            correctIndex = 2,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em uma curva de titulação de um aminoácido, o pI é:",
            answers = new string[] {
                "O pH em que ocorre a primeira dissociação.",
                "O pH em que ocorre a última dissociação.",
                "O pH em que a carga líquida do aminoácido é zero.",
                "O pH em que a concentração de H+ é máxima."
            },
            correctIndex = 2,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos apolares são geralmente:",
            answers = new string[] {
                "Solúveis em água.",
                "Insolúveis em água.",
                "Anfipáticos.",
                "Carregados."
            },
            correctIndex = 1,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos polares são geralmente:",
            answers = new string[] {
                "Insolúveis em água.",
                "Solúveis em água.",
                "Anfipáticos.",
                "Neutros."
            },
            correctIndex = 1,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com grupos R carregados positivamente são:",
            answers = new string[] {
                "Ácidos",
                "Básicos",
                "Neutros",
                "Apolares"
            },
            correctIndex = 1,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com grupos R carregados negativamente são:",
            answers = new string[] {
                "Básicos",
                "Ácidos",
                "Neutros",
                "Apolares"
            },
            correctIndex = 1,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A técnica de eletroforese SDS-Page separa proteínas com base em:",
            answers = new string[] {
                "Seu tamanho.",
                "Sua carga líquida.",
                "Sua sequência de aminoácidos.",
                "Sua estrutura terciária."
            },
            correctIndex = 0,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em eletroforese com focalização isoelétrica, um aminoácido com carga líquida positiva migrará para o pólo:",
            answers = new string[] {
                "Positivo",
                "Negativo",
                "Não migrará",
                "Depende do pH"
            },
            correctIndex = 1,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em eletroforese com focalização isoelétrica, um aminoácido com carga líquida negativa migrará para o pólo:",
            answers = new string[] {
                "Negativo",
                "Positivo",
                "Não migrará",
                "Depende do pH"
            },
            correctIndex = 1,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em eletroforese com focalização isoelétrica, um aminoácido no seu ponto isoelétrico:",
            answers = new string[] {
                "Migrará para o pólo positivo.",
                "Migrará para o pólo negativo.",
                "Não migrará.",
                "Migrará para ambos os pólos."
            },
            correctIndex = 2,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A 4-hidroxiprolina e a 5-hidroxilisina são encontradas em:",
            answers = new string[] {
                "Todas as proteínas",
                "Proteínas de membrana",
                "Colágeno",
                "Enzimas"
            },
            correctIndex = 2,
            questionNumber = 48,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A ornitina e a citrulina são:",
            answers = new string[] {
                "Aminoácidos comuns em proteínas",
                "Aminoácidos envolvidos em vias metabólicas",
                "Aminoácidos essenciais",
                "Aminoácidos não-essenciais"
            },
            correctIndex = 1,
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A fenilcetonúria é um distúrbio metabólico causado pela deficiência da enzima que metaboliza:",
            answers = new string[] {
                "Tirosina",
                "Triptofano",
                "Fenilalanina",
                "Glicina"
            },
            correctIndex = 2,
            questionNumber = 50,
            isImageAnswer = false
        }
    };

    public List<Question> GetQuestions()
    {
        return questions;
    }

    public QuestionSet GetQuestionSetType()
    {
        return QuestionSet.aminoacids;
    }

    public string GetDatabankName()
    {
        return "AminoacidQuestionDatabase";
    }
}