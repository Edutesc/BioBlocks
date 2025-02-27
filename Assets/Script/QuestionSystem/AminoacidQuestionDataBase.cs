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
            questionText = "Qual aminoácido possui um anel aromático no radical R?",
            answers = new string[] {
                "Alanina",
                "Glicina",
                "Fenilalanina",
                "Serina"
            },
            correctIndex = 2,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A tirosina difere da fenilalanina pela presença de:",
            answers = new string[] {
                "Um grupo metil.",
                "Um grupo sulfidrila.",
                "Um grupo hidroxila.",
                "Um anel aromático."
            },
            correctIndex = 2,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com grupos hidroxila no radical R são:",
            answers = new string[] {
                "Apolares.",
                "Polares.",
                "Carregados positivamente.",
                "Carregados negativamente."
            },
            correctIndex = 1,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com grupos sulfidrila no radical R são:",
            answers = new string[] {
                "Apolares.",
                "Polares.",
                "Carregados positivamente.",
                "Carregados negativamente."
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Quais aminoácidos contêm enxofre em seus radicais R?",
            answers = new string[] {
                "Serina e Treonina",
                "Metionina e Cisteína",
                "Asparagina e Glutamina",
                "Lisina e Arginina"
            },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Quais aminoácidos são considerados ácidos?",
            answers = new string[] {
                "Lisina e Arginina",
                "Aspartato e Glutamato",
                "Serina e Treonina",
                "Fenilalanina e Tirosina"
            },
            correctIndex = 1,
            questionNumber = 13,
            isImageAnswer = false
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
            questionText = "Quais aminoácidos são considerados básicos?",
            answers = new string[] {
                "Aspartato e Glutamato",
                "Lisina, Arginina e Histidina",
                "Serina e Treonina",
                "Fenilalanina e Tirosina"
            },
            correctIndex = 1,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A lisina, arginina e histidina possuem:",
            answers = new string[] {
                "Grupos carboxila em seus radicais R.",
                "Grupos amino em seus radicais R.",
                "Grupos sulfidrila em seus radicais R.",
                "Anéis aromáticos em seus radicais R."
            },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Qual aminoácido forma um anel com seu grupamento R?",
            answers = new string[] {
                "Alanina",
                "Glicina",
                "Prolina",
                "Serina"
            },
            correctIndex = 2,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Aminoácidos com carga líquida nula em pH 7 são:",
            answers = new string[] {
                "Completamente protonados.",
                "Completamente desprotonados.",
                "Parcialmente protonados.",
                "Não possuem carga."
            },
            correctIndex = 2,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em pH ácido, a maioria dos aminoácidos terá carga líquida:",
            answers = new string[] {
                "Negativa",
                "Neutra",
                "Positiva",
                "Variável"
            },
            correctIndex = 2,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Em pH básico, a maioria dos aminoácidos terá carga líquida:",
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
            questionText = "A fenilcetonúria é causada por uma deficiência na enzima que metaboliza:",
            answers = new string[] {
                "Tirosina",
                "Fenilalanina",
                "Triptofano",
                "Glicina"
            },
            correctIndex = 1,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "O triptofano é um precursor de:",
            answers = new string[] {
                "Hormônios",
                "Neurotransmissores",
                "Vitaminas",
                "Todas as anteriores."
            },
            correctIndex = 3,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "Quais aminoácidos absorvem luz ultravioleta?",
            answers = new string[] {
                "Alanina e Valina",
                "Fenilalanina, Tirosina e Triptofano",
                "Serina e Treonina",
                "Asparagina e Glutamina"
            },
            correctIndex = 1,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A serina difere da alanina pela presença de:",
            answers = new string[] {
                "Um grupo metil.",
                "Um grupo sulfidrila.",
                "Um grupo hidroxila.",
                "Um anel aromático."
            },
            correctIndex = 2,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AminoacidQuestionDatabase",
            questionText = "A treonina difere da serina pela presença de:",
            answers = new string[] {
                "Um grupo hidroxila.",
                "Um grupo metil.",
                "Um grupo sulfidrila.",
                "Um anel aromático."
            },
            correctIndex = 1,
            questionNumber = 32,
            isImageAnswer = false
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