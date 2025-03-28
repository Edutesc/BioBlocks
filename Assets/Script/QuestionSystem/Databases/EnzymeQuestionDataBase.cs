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
            questionText = "Qual fator é essencial para que uma enzima exerça sua ativiade plenamente",
            answers = new string[] {
                "A sua estrutura primária",
                "A estabilidade de sua estrutura terciária",
                "A quantidade de alfa-hélices na estrutura da enzima",
                "A formação de estrutura quaternária"
            },
            correctIndex = 1,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Por que enzimas podem ser usadas na indústria",
            answers = new string[] {
                "Reação enzimática ocorre em temperaturas brandas.",
                "Enzimas são altamente específicas.",
                "Necessita-se de quantidades bem pequenas de enzimas, mesmo em escala industrial.",
                "Todas as alternativas são corretas."
            },
            correctIndex = 3,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "As enzimas podem ser agrupadas em seis grandes grupos, de acordo com o tipo de reação que ela catalisa. Abaixo temos alguns nome de grupos de enzimas, exceto: ",
            answers = new string[] {
                "Hidrolases",
                "Ribolase",
                "Oxidoredutases",
                "Liases"
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            answers = new string[] {
                "Região da enzima responsável por interagir com a água",
                "Região da enzima com grande afinidade por íons",
                "Região da enzima que participa diretamente da catálise",
                "Região da enzima altamente hidrofóbica"
            },
            correctIndex = 2,
            questionNumber = 12,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath = "AnswerImages/EnzymeDB/enzymeQuestion_12"
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
            answers = new string[] {
                "Inibição Irreversível",
                "Inibição Competitiva",
                "Inibição  Não-Competitiva",
                "Inibição A-Competitiva"
            },
            correctIndex = 2,
            questionNumber = 21,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath = "AnswerImages/EnzymeDB/enzymeQuestion_21"
        },
       new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            answers = new string[] {
                "Inibição Irreversível",
                "Inibição Competitiva",
                "Inibição  Não-Competitiva",
                "Inibição A-Competitiva"
            },
            correctIndex = 1,
            questionNumber = 22,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath = "AnswerImages/EnzymeDB/enzymeQuestion_22"
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A constante de Michaelis (Km) indica:",
            answers = new string[] {
                "A velocidade máxima da reação.",
                "A concentração de enzima.",
                "A concentração de substrato necessária para a enzima atingir metade da sua velocidade máxima.",
                "A energia de ativação."
            },
            correctIndex = 2,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "Um Km baixo indica:",
            answers = new string[] {
                "Baixa interação da enzima com substrato.",
                "Alta interação da enzima com substrato.",
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
            questionText = "Um Km alto indica:",
            answers = new string[] {
                "Baixa interação da enzima com substrato.",
                "Alta interação da enzima com substrato.",
                "Velocidade máxima de reação alta.",
                "Velocidade máxima de reação baixa."
            },
            correctIndex = 0,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A equação de Michaelis-Menten relaciona:",
            answers = new string[] {
                "Km, Vmax e a concentração de substrato.",
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
            answers = new string[] {
                "Gráfico de Michaelis-Menten",
                "Gráfico Enzimático",
                "Gaáfico de Lineweaver-Burk",
                "Gráfico Competitivo"
            },
            correctIndex = 2,
            questionNumber = 28,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath = "AnswerImages/EnzymeDB/enzymeQuestion_28"
        },
        new Question
        {
            questionDatabankName = "EnzymeQuestionDatabase",
            questionText = "A enzima que hidrolisa o RNA é:",
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
            questionText = "A enzima que hidrolisa proteínas é:",
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
            questionText = "A enzima que hidrolisa lipídios é:",
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
            questionText = "A enzima que hidrolisa amido é:",
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
                "Que hidrolisa carboidratos.",
                "Que hidrolisa proteínas.",
                "Que hidrolisa lipídios.",
                "Que hidrolisa ácidos nucléicos."
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
                "Que hidrolisa carboidratos.",
                "Que hidrolisa proteínas.",
                "Que hidrolisa lipídios.",
                "Que hidrolisa ácidos nucléicos."
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
                "Que hidrolisa lipídios.",
                "Que hidrolisa proteínas.",
                "Que hidrolisa carboidratos.",
                "Que hidrolisa ácidos nucléicos."
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
            questionText = "A inibição enzimática irreversível causa:",
            answers = new string[] {
                "Uma diminuição temporária da atividade enzimática.",
                "Uma diminuição permanente da atividade enzimática.",
                "Um aumento da atividade enzimática.",
                "Nenhuma alteração na atividade enzimática."
            },
            correctIndex = 1,
            questionNumber = 39,
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
            questionNumber = 40,
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
            questionNumber = 41,
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
            questionNumber = 42,
            isImageAnswer = false
        },
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