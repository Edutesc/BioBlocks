using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class EnzymeQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
       new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O que são enzimas?",
            answers = new string[] {
                "Catalisadores químicos inorgânicos.",
                "Catalisadores biológicos, principalmente proteínas.",
                "Substratos que participam de reações químicas.",
                "Produtos de reações químicas."
            },
            correctIndex = 1,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Qual a principal função de uma enzima?",
            answers = new string[] {
                "Sintetizar proteínas.",
                "Aumentar a velocidade de uma reação.",
                "Regular a temperatura corporal.",
                "Transportar oxigênio."
            },
            correctIndex = 1,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Como as enzimas aumentam a velocidade das reações?",
            answers = new string[] {
                "Aumentando a energia de ativação.",
                "Diminuindo a energia de ativação.",
                "Alterando o equilíbrio da reação.",
                "Aumentando a concentração de substrato."
            },
            correctIndex = 1,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O que é o estado de transição em uma reação?",
            answers = new string[] {
                "O estado inicial da reação.",
                "O estado final da reação.",
                "Um estado intermediário de alta energia.",
                "Um catalisador."
            },
            correctIndex = 2,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O que é energia de ativação?",
            answers = new string[] {
                "A energia necessária para iniciar uma reação.",
                "A energia liberada durante uma reação.",
                "A diferença de energia entre o substrato e o estado de transição.",
                "A energia do produto."
            },
            correctIndex = 2,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "As enzimas atuam em condições:",
            answers = new string[] {
                "Extremas de temperatura e pH.",
                "Compatíveis com a vida.",
                "Exclusivamente in vitro.",
                "Independentes do meio."
            },
            correctIndex = 1,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O sítio ativo de uma enzima é:",
            answers = new string[] {
                "A região onde a enzima se liga ao produto.",
                "A região onde a enzima se liga ao substrato.",
                "A região responsável pela regulação da enzima.",
                "A região onde a enzima se liga a cofatores."
            },
            correctIndex = 1,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O modelo chave-fechadura descreve a interação enzima-substrato como:",
            answers = new string[] {
                "Um ajuste induzido.",
                "Uma ligação covalente.",
                "Um encaixe complementar.",
                "Uma interação hidrofóbica."
            },
            correctIndex = 2,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O modelo de ajuste induzido descreve a interação enzima-substrato como:",
            answers = new string[] {
                "Uma ligação covalente.",
                "Um ajuste estrutural na enzima.",
                "Um encaixe complementar.",
                "Uma interação iônica."
            },
            correctIndex = 1,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A especificidade de uma enzima se deve principalmente a:",
            answers = new string[] {
                "Seu tamanho.",
                "Sua forma tridimensional.",
                "A interação entre o sítio ativo e o substrato.",
                "Sua localização na célula."
            },
            correctIndex = 2,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A hipótese da chave-fechadura foi proposta por:",
            answers = new string[] {
                "Linus Pauling",
                "Emil Fischer",
                "James Sumner",
                "Daniel Koshland"
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A hipótese do ajuste induzido foi proposta por:",
            answers = new string[] {
                "James Sumner",
                "Emil Fischer",
                "Daniel Koshland",
                "Linus Pauling"
            },
            correctIndex = 2,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A atividade de uma enzima pode ser afetada por:",
            answers = new string[] {
                "Temperatura e pH.",
                "Concentração de substrato.",
                "Presença de inibidores.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O pH ótimo de uma enzima é:",
            answers = new string[] {
                "O pH em que a enzima tem atividade máxima.",
                "O pH em que a enzima é inativada.",
                "O pH em que a enzima é desnaturada.",
                "O pH do meio celular."
            },
            correctIndex = 0,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A temperatura ótima de uma enzima é:",
            answers = new string[] {
                "A temperatura em que a enzima é desnaturada.",
                "A temperatura em que a enzima tem atividade máxima.",
                "A temperatura ambiente.",
                "A temperatura do organismo."
            },
            correctIndex = 1,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O que acontece com a atividade de uma enzima quando a temperatura aumenta muito além da sua temperatura ótima?",
            answers = new string[] {
                "Aumenta.",
                "Diminui.",
                "Permanece constante.",
                "Varia de forma imprevisível."
            },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Desnaturação de uma enzima significa:",
            answers = new string[] {
                "Ativação da enzima.",
                "Perda da atividade enzimática devido à alteração da sua estrutura.",
                "Aumento da velocidade da reação.",
                "Formação de um complexo enzima-substrato."
            },
            correctIndex = 1,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Quais fatores podem causar a desnaturação de uma enzima?",
            answers = new string[] {
                "Altas temperaturas.",
                "Variações de pH.",
                "Solventes orgânicos.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Inibidores enzimáticos são moléculas que:",
            answers = new string[] {
                "Aumentam a atividade da enzima.",
                "Diminuem ou impedem a atividade da enzima.",
                "Alteram o equilíbrio da reação.",
                "São substratos da enzima."
            },
            correctIndex = 1,
            questionNumber = 19,
            isImageAnswer = false
        },
         new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Inibidores irreversíveis se ligam à enzima:",
            answers = new string[] {
                "Reversivelmente.",
                "Irreversivelmente, modificando permanentemente sua estrutura.",
                "Em um sítio alostérico.",
                "Somente em pH ácido."
            },
            correctIndex = 1,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Inibidores reversíveis competitivos competem com o:",
            answers = new string[] {
                "Produto",
                "Inibidor irreversível",
                "Substrato",
                "Cofator"
            },
            correctIndex = 2,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Inibidores reversíveis não-competitivos se ligam à enzima em um sítio:",
            answers = new string[] {
                "Igual ao do substrato.",
                "Diferente do sítio ativo.",
                "Somente em pH básico.",
                "Somente em altas temperaturas."
            },
            correctIndex = 1,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A constante de Michaelis (KM) indica:",
            answers = new string[] {
                "A velocidade máxima da reação.",
                "A concentração de enzima.",
                "A afinidade da enzima pelo substrato.",
                "A energia de ativação."
            },
            correctIndex = 2,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Um KM baixo indica:",
            answers = new string[] {
                "Baixa afinidade da enzima pelo substrato.",
                "Alta afinidade da enzima pelo substrato.",
                "Velocidade máxima de reação baixa.",
                "Velocidade máxima de reação alta."
            },
            correctIndex = 1,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Um KM alto indica:",
            answers = new string[] {
                "Alta afinidade da enzima pelo substrato.",
                "Baixa afinidade da enzima pelo substrato.",
                "Velocidade máxima de reação alta.",
                "Velocidade máxima de reação baixa."
            },
            correctIndex = 1,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A equação de Michaelis-Menten relaciona:",
            answers = new string[] {
                "KM, Vmax e a concentração de substrato.",
                "KM, pH e temperatura.",
                "Vmax, temperatura e pH.",
                "KM, pKa e a concentração de substrato."
            },
            correctIndex = 0,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Na equação de Michaelis-Menten, Vmax representa:",
            answers = new string[] {
                "A velocidade inicial da reação.",
                "A velocidade máxima da reação.",
                "A constante de Michaelis.",
                "A concentração de substrato."
            },
            correctIndex = 1,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Na equação de Lineweaver-Burk, o gráfico obtido é:",
            answers = new string[] {
                "Uma hipérbole.",
                "Uma reta.",
                "Uma parábola.",
                "Uma exponencial."
            },
            correctIndex = 1,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A enzima que quebra o RNA é:",
            answers = new string[] {
                "DNA polimerase",
                "RNA polimerase",
                "Ribonuclease",
                "Protease"
            },
            correctIndex = 2,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A enzima que quebra proteínas é:",
            answers = new string[] {
                "Ribonuclease",
                "Protease",
                "Lipase",
                "Amílase"
            },
            correctIndex = 1,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A enzima que quebra lipídios é:",
            answers = new string[] {
                "Amílase",
                "Protease",
                "Lipase",
                "Ribonuclease"
            },
            correctIndex = 2,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A enzima que quebra amido é:",
            answers = new string[] {
                "Lipase",
                "Protease",
                "Amílase",
                "Ribonuclease"
            },
            correctIndex = 2,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A pepsina é uma enzima:",
            answers = new string[] {
                "Que quebra carboidratos.",
                "Que quebra proteínas.",
                "Que quebra lipídios.",
                "Que quebra ácidos nucléicos."
            },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A pepsina atua melhor em qual pH?",
            answers = new string[] {
                "pH 7",
                "pH 10",
                "pH 2",
                "pH 14"
            },
            correctIndex = 2,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A quimotripsina é uma enzima:",
            answers = new string[] {
                "Que quebra carboidratos.",
                "Que quebra proteínas.",
                "Que quebra lipídios.",
                "Que quebra ácidos nucléicos."
            },
            correctIndex = 1,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A quimotripsina atua melhor em qual pH?",
            answers = new string[] {
                "pH 2",
                "pH 7",
                "pH 8",
                "pH 14"
            },
            correctIndex = 2,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A amilase salivar é uma enzima:",
            answers = new string[] {
                "Que quebra lipídios.",
                "Que quebra proteínas.",
                "Que quebra carboidratos.",
                "Que quebra ácidos nucléicos."
            },
            correctIndex = 2,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A amilase salivar atua melhor em qual pH?",
            answers = new string[] {
                "pH 2",
                "pH 7",
                "pH 8",
                "pH 14"
            },
            correctIndex = 1,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A hexocinase é uma isoforma da enzima que catalisa a conversão de:",
            answers = new string[] {
                "Glicose em glicose-6-fosfato",
                "Glicose em glicogênio",
                "Glicogênio em glicose",
                "Frutose em glicose"
            },
            correctIndex = 0,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A glicocinase é uma isoforma da enzima que catalisa a conversão de:",
            answers = new string[] {
                "Glicose em glicose-6-fosfato",
                "Glicose em glicogênio",
                "Glicogênio em glicose",
                "Frutose em glicose"
            },
            correctIndex = 0,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Qual enzima possui maior afinidade pela glicose, hexocinase ou glicocinase?",
            answers = new string[] {
                "Hexocinase",
                "Glicocinase",
                "Ambas possuem a mesma afinidade.",
                "Depende do tecido."
            },
            correctIndex = 0,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A inibição enzimática irreversível causa:",
            answers = new string[] {
                "Uma diminuição temporária da atividade enzimática.",
                "Uma diminuição permanente da atividade enzimática.",
                "Um aumento da atividade enzimática.",
                "Nenhuma alteração na atividade enzimática."
            },
            correctIndex = 1,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A inibição enzimática reversível competitiva pode ser superada por:",
            answers = new string[] {
                "Aumento da concentração do inibidor.",
                "Diminuição da concentração do inibidor.",
                "Aumento da concentração do substrato.",
                "Diminuição da concentração do substrato."
            },
            correctIndex = 2,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A inibição enzimática reversível não-competitiva pode ser superada por:",
            answers = new string[] {
                "Aumento da concentração do substrato.",
                "Diminuição da concentração do substrato.",
                "Aumento da concentração do inibidor.",
                "Não pode ser superada."
            },
            correctIndex = 3,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A aspirina inibe a enzima:",
            answers = new string[] {
                "Hexokinase",
                "Ciclooxigenase",
                "Lipase",
                "Protease"
            },
            correctIndex = 1,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "O captopril e o enalapril inibem a enzima:",
            answers = new string[] {
                "Ciclooxigenase",
                "ECA (enzima conversora de angiotensina)",
                "Lipase",
                "Protease"
            },
            correctIndex = 1,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A inibição da ALAD pelo chumbo é um exemplo de inibição:",
            answers = new string[] {
                "Reversível competitiva",
                "Reversível não-competitiva",
                "Irreversível",
                "Alostérica"
            },
            correctIndex = 2,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A luciferase é uma enzima que:",
            answers = new string[] {
                "Quebra proteínas.",
                "Quebra lipídios.",
                "Produz luz.",
                "Quebra DNA."
            },
            correctIndex = 2,
            questionNumber = 48,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A luciferina é:",
            answers = new string[] {
                "Um substrato da luciferase.",
                "Uma enzima.",
                "Um inibidor.",
                "Um produto da reação."
            },
            correctIndex = 0,
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A biorremediação utiliza:",
            answers = new string[] {
                "Reagentes químicos para degradar poluentes.",
                "Microorganismos para degradar poluentes.",
                "Plantas para absorver poluentes.",
                "Animais para consumir poluentes."
            },
            correctIndex = 1,
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
        return QuestionSet.enzymes;
    }

    public string GetDatabankName()
    {
        return "EnzymeQuestionDatabase";
    }
}