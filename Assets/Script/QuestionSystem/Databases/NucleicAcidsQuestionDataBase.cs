using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class NucleicAcidsQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Quem primeiro isolou o ácido nucléico?",
            answers = new string[] { "Watson", "Crick", "Friedrich Miescher", "Chargaff" },
            correctIndex = 2,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a principal função do RNA na célula?",
            answers = new string[] { 
                "Armazenamento de informação genética", 
                "Síntese de proteínas", 
                "Catálise de reações", 
                "Transporte de íons" 
            },
            correctIndex = 1,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Quais são os três componentes de um nucleotídeo?",
            answers = new string[] { 
                "Açúcar, base, fosfato", 
                "Açúcar, base, aminoácido", 
                "Base, aminoácido, fosfato", 
                "Açúcar, lipídeo, base" 
            },
            correctIndex = 0,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual açúcar está presente no RNA?",
            answers = new string[] { "Desoxirribose", "Ribose", "Glicose", "Frutose" },
            correctIndex = 1,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual açúcar está presente no DNA?",
            answers = new string[] { "Desoxirribose", "Ribose", "Glicose", "Frutose" },
            correctIndex = 0,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que são nucleosídeos?",
            answers = new string[] {
                "Açúcar + base",
                "Açúcar + base + fosfato",
                "Base + fosfato",
                "Açúcar + aminoácido"
            },
            correctIndex = 0,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Identifique a estrutura do nucleosídeo",
            answers = new string[] {
                "AnswerImages/NucleicAcidDB/nucleotideo_di_fosfato",
                "AnswerImages/NucleicAcidDB/nucleotideo_mono_fosfato",
                "AnswerImages/NucleicAcidDB/nucleosideo",
                "AnswerImages/NucleicAcidDB/nucleotideo_tri_fosfato"
            },
            correctIndex = 2,
            questionNumber = 7,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Quais bases são encontradas no RNA, mas não no DNA?",
            answers = new string[] { "Adenina, guanina", "Citosina, timina", "Uracila", "Timina, uracila" },
            correctIndex = 2,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Quais bases são encontradas no DNA, mas não no RNA?",
            answers = new string[] { "Adenina, guanina", "Citosina, timina", "Uracila", "Timina" },
            correctIndex = 3,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a função principal dos grupamentos fosfato nos nucleotídeos?",
            answers = new string[] { "Dar caráter básico", "Dar caráter ácido", "Formar ligações peptídicas", "Armazenar energia" },
            correctIndex = 1,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Que tipo de ligação une os nucleotídeos em uma cadeia?",
            answers = new string[] { "Ligação peptídica", "Ligação glicosídica", "Ligação éster", "Ligação fosfodiéster" },
            correctIndex = 3,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a orientação das cadeias de DNA em uma dupla hélice?",
            answers = new string[] { "Paralela", "Antiparalela", "Perpendicular", "Aleatória" },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que são pares de bases de Watson-Crick?",
            answers = new string[] {
                "A-T e G-C",
                "A-G e T-C",
                "A-C e G-T",
                "Qualquer combinação de bases."
            },
            correctIndex = 0,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual tipo de ligação mantém os pares de bases unidos no DNA?",
            answers = new string[] { "Ligação iônica", "Ligação covalente", "Pontes de hidrogênio", "Ligação peptídica" },
            correctIndex = 2,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a função principal do DNA?",
            answers = new string[] { "Transporte de moléculas", "Síntese de proteínas", "Armazenamento de informação genética", "Catálise de reações" },
            correctIndex = 2,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a função principal do RNA?",
            answers = new string[] { "Transporte de moléculas", "Síntese de proteínas", "Armazenamento de informação genética", "Expressão da informação genética" },
            correctIndex = 3,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que é desnaturação do DNA?",
            answers = new string[] {
                "Quebra da dupla hélice.",
                "Separação das fitas.",
                "Mudança na seqüência de bases.",
                "Todas as alternativas acima."
            },
            correctIndex = 1,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que é renaturação do DNA?",
            answers = new string[] {
                "Formação de novas fitas.",
                "Reassociação das fitas.",
                "Replicação do DNA.",
                "Transcrição do DNA."
            },
            correctIndex = 1,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que causa a desnaturação do DNA?",
            answers = new string[] {
                "Altas temperaturas",
                "Extremos de pH",
                "Ação de enzimas",
                "Todas as alternativas acima"
            },
            correctIndex = 3,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Para que serve a medida de absorvância a 260nm?",
            answers = new string[] {
                "Medida da concentração de proteínas.",
                "Medida da concentração de ácidos nucléicos.",
                "Medida da temperatura de fusão do DNA.",
                "Medida da viscosidade de uma solução."
            },
            correctIndex = 1,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que é a regra de Chargaff?",
            answers = new string[] {
                "A = T e G = C",
                "A = G e T = C",
                "A = C e G = T",
                "Não há regra de Chargaff."
            },
            correctIndex = 0,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Em que tipo de molécula a regra de Chargaff se aplica?",
            answers = new string[] {
                "DNA",
                "RNA",
                "Proteínas",
                "Carboidratos"
            },
            correctIndex = 0,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que é o 'fluxo da informação genética'?",
            answers = new string[] {
                "O movimento de íons através da membrana.",
                "A replicação do DNA.",
                "O processo de conversão da informação genética em proteínas.",
                "O transporte de proteínas para o exterior da célula."
            },
            correctIndex = 2,
            questionNumber = 23,
            isImageAnswer = false
        },

        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual tipo de RNA transporta aminoácidos para os ribossomos?",
            answers = new string[] { "tRNA", "rRNA", "mRNA", "snRNA" },
            correctIndex = 0,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual tipo de RNA faz parte da estrutura dos ribossomos?",
            answers = new string[] { "tRNA", "rRNA", "mRNA", "snRNA" },
            correctIndex = 1,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a principal diferença química entre DNA e RNA?",
            answers = new string[] { "Açúcar", "Bases nitrogenadas", "Grupamento fosfato", "Sequência de bases" },
            correctIndex = 0,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a principal diferença na composição de bases entre DNA e RNA?",
            answers = new string[] { "Timina vs. Uracila", "Adenina vs. Guanina", "Citosina vs. Guanina", "Ribose vs. Desoxirribose" },
            correctIndex = 0,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que é um códon?",
            answers = new string[] {
                "Uma seqüência de três bases no tRNA.",
                "Uma seqüência de três bases no mRNA.",
                "Uma seqüência de três bases no rRNA.",
                "Uma seqüência de três bases no DNA."
            },
            correctIndex = 1,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a função do anticódon no tRNA?",
            answers = new string[] {
                "Ligar-se ao ribossomo.",
                "Ligar-se ao mRNA.",
                "Ligar-se a proteínas.",
                "Ligar-se ao DNA."
            },
            correctIndex = 1,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual a função principal dos rRNAs?",
            answers = new string[] {
                "Transporte de aminoácidos.",
                "Síntese de proteínas.",
                "Fazem parte da estrutura dos ribossomos.",
                "Catalisam reações."
            },
            correctIndex = 2,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Quais são os três componentes de um nucleotídeo?",
            answers = new string[] { "Açúcar, base, fosfato", "Açúcar, base, aminoácido", "Base, aminoácido, fosfato", "Açúcar, lipídeo, base" },
            correctIndex = 0,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "O que é um nucleosídeo?",
            answers = new string[] { "Açúcar + base + fosfato", "Açúcar + base", "Base + fosfato", "Açúcar + aminoácido" },
            correctIndex = 1,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual o açúcar presente nos ribonucleotídeos?",
            answers = new string[] { "Desoxirribose", "Ribose", "Glicose", "Frutose" },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "NucleicAcidsQuestionDatabase",
            questionText = "Qual o açúcar presente nos desoxirribonucleotídeos?",
            answers = new string[] { "Desoxirribose", "Ribose", "Glicose", "Frutose" },
            correctIndex = 0,
            questionNumber = 34,
            isImageAnswer = false
        }
    };

    public List<Question> GetQuestions()
    {
        return questions;
    }

    public QuestionSet GetQuestionSetType()
    {
        return QuestionSet.nucleicAcids;
    }

    public string GetDatabankName()
    {
        return "NucleicAcidsQuestionDatabase";
    }
}
