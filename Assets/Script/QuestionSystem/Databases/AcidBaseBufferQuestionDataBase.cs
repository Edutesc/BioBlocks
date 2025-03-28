using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class AcidBaseBufferQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Segundo Arrhenius, o que caracteriza um ácido?",
            answers = new string[] {
                "Libera íons H+ em solução aquosa.",
                "Recebe prótons (H+) em solução aquosa.",
                "Libera íons OH- em solução aquosa.",
                "Recebe íons OH- em solução aquosa."
            },
            correctIndex = 0,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Segundo Arrhenius, o que caracteriza uma base?",
            answers = new string[] {
                "Libera íons H+ em solução aquosa.",
                "Recebe prótons (H+) em solução aquosa.",
                "Libera íons OH- em solução aquosa.",
                "Recebe íons OH- em solução aquosa."
            },
            correctIndex = 2,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "De acordo com Brønsted-Lowry, o que é um ácido?",
            answers = new string[] {
                "Doador de prótons (H+).",
                "Receptor de prótons (H+).",
                "Doador de íons OH-. ",
                "Receptor de íons OH-."
            },
            correctIndex = 0,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "De acordo com Brønsted-Lowry, o que é uma base?",
            answers = new string[] {
                "Doador de prótons (H+).",
                "Receptor de prótons (H+).",
                "Doador de íons OH-. ",
                "Receptor de íons OH-."
            },
            correctIndex = 1,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A água pode atuar como:",
            answers = new string[] {
                "Apenas ácido.",
                "Apenas base.",
                "Tanto ácido quanto base.",
                "Nem ácido nem base."
            },
            correctIndex = 2,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O que é a base conjugada do HCl?",
            answers = new string[] {
                "H<sup><size=150%> +</size></sup>",
                "Cl<sup><size=150%> -</size></sup>",
                "H<sub><size=150%>2</size></sub> O",
                "OH<sup><size=150%> -</size></sup>"
            },
            correctIndex = 1,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O que é o ácido conjugado da NH<sub><size=150%>3</size></sub>?",
            answers = new string[] {
                "H<sup><size=150%> +</size></sup>",
                "OH<sup><size=150%> -</size></sup>",
                "NH<sub><size=150%>4</size></sub><sup><size=150%> +</size></sup>",
                "NH<sub><size=150%>2</size></sub><sup><size=150%> -</size></sup>"
            },
            correctIndex = 2,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Um ácido forte em solução aquosa:",
            answers = new string[] {
                "Se dissocia parcialmente.",
                "Se dissocia completamente.",
                "Não se dissocia.",
                "Forma ligações de hidrogênio."
            },
            correctIndex = 1,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Um ácido fraco em solução aquosa:",
            answers = new string[] {
                "Se dissocia completamente.",
                "Se dissocia parcialmente.",
                "Não se dissocia.",
                "Forma ligações iônicas."
            },
            correctIndex = 1,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A constante de equilíbrio (Keq) de uma reação indica:",
            answers = new string[] {
                "A velocidade da reação.",
                "A proporção de reagentes e produtos no equilíbrio.",
                "A energia de ativação da reação.",
                "A concentração dos reagentes."
            },
            correctIndex = 1,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Em uma reação em equilíbrio, se Keq > 1:",
            answers = new string[] {
                "Os reagentes são favorecidos.",
                "Os produtos são favorecidos.",
                "Os reagentes e produtos têm concentrações iguais.",
                "A reação é irreversível."
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Em uma reação em equilíbrio, se Keq < 1:",
            answers = new string[] {
                "Os produtos são favorecidos.",
                "Os reagentes são favorecidos.",
                "Os reagentes e produtos têm concentrações iguais.",
                "A reação é irreversível."
            },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A constante de dissociação ácida (Ka) mede:",
            answers = new string[] {
                "A força de uma base.",
                "A força de um ácido.",
                "A velocidade de uma reação.",
                "O equilíbrio de uma reação."
            },
            correctIndex = 1,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Um ácido forte tem um valor de Ka:",
            answers = new string[] {
                "Baixo",
                "Alto",
                "Próximo a 1",
                "Próximo a 0"
            },
            correctIndex = 1,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Um ácido fraco tem um valor de Ka:",
            answers = new string[] {
                "Alto",
                "Baixo",
                "Próximo a 1",
                "Próximo a 0"
            },
            correctIndex = 1,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O pKa de um ácido é definido como:",
            answers = new string[] {
                "log Ka",
                "-log Ka",
                "1/Ka",
                "10/Ka"
            },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Um ácido com um pKa baixo é:",
            answers = new string[] {
                "Fraco",
                "Forte",
                "De força moderada",
                "Indeterminado"
            },
            correctIndex = 1,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Um ácido com um pKa alto é:",
            answers = new string[] {
                "Forte",
                "Fraco",
                "De força moderada",
                "Indeterminado"
            },
            correctIndex = 1,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A equação de Henderson-Hasselbalch relaciona:",
            answers = new string[] {
                "pH, pKa e a razão entre base conjugada e ácido.",
                "pH, pKa e a concentração de íons H+",
                "pH, pOH e a concentração de íons OH-",
                "pKa, pKb e a concentração de íons H+"
            },
            correctIndex = 0,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Em uma solução-tampão, o pH permanece relativamente constante porque:",
            answers = new string[] {
                "O ácido se dissocia completamente.",
                "A base se dissocia completamente.",
                "Há um equilíbrio entre ácido e sua base conjugada.",
                "Não há interações entre o ácido e a base."
            },
            correctIndex = 2,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A faixa de tamponamento de uma solução-tampão é:",
            answers = new string[] {
                "Muito menor que o pKa.",
                "Igual ao pKa.",
                "Aproximadamente ± 1 unidade de pH em relação ao pKa.",
                "Muito maior que o pKa."
            },
            correctIndex = 2,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O pH do sangue é mantido constante principalmente pelo sistema tampão:",
            answers = new string[] {
                "Fosfato",
                "Acetato",
                "Bicarbonato",
                "Tris"
            },
            correctIndex = 2,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O que acontece com o pH do sangue durante o exercício intenso?",
            answers = new string[] {
                "Aumenta.",
                "Diminui.",
                "Permanece constante.",
                "Varia de forma imprevisível."
            },
            correctIndex = 1,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Como o corpo responde à diminuição do pH sangüíneo durante o exercício?",
            answers = new string[] {
                "Diminui a taxa respiratória.",
                "Aumenta a taxa respiratória.",
                "Mantém a taxa respiratória constante.",
                "Para de respirar."
            },
            correctIndex = 1,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O que é pH?",
            answers = new string[] {
                "Uma medida da concentração de OH-",
                "Uma medida da concentração de H+",
                "Uma medida da temperatura",
                "Uma medida da pressão"
            },
            correctIndex = 1,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Uma solução com pH 3 é:",
            answers = new string[] {
                "Neutra",
                "Básica",
                "Ácida",
                "Tampão"
            },
            correctIndex = 2,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Uma solução com pH 11 é:",
            answers = new string[] {
                "Ácida",
                "Neutra",
                "Básica",
                "Tampão"
            },
            correctIndex = 2,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Uma solução com pH 7 é:",
            answers = new string[] {
                "Ácida",
                "Neutra",
                "Básica",
                "Tampão"
            },
            correctIndex = 1,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O pOH de uma solução é uma medida de:",
            answers = new string[] {
                "Concentração de H+",
                "Concentração de OH-",
                "Acidez",
                "Basicidade"
            },
            correctIndex = 1,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A relação entre pH e pOH é:",
            answers = new string[] {
                "pH + pOH = 0",
                "pH + pOH = 7",
                "pH + pOH = 14",
                "pH + pOH = 21"
            },
            correctIndex = 2,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Qual o produto iônico da água (Kw) a 25<sup><size=100%>o</size></sup> C?",
            answers = new string[] {
                "10<sup><size=150%>-7</size></sup> ",
                "10<sup><size=150%>-14</size></sup> ",
                "10<sup><size=150%>0</size></sup> ",
                "10<sup><size=150%>14</size></sup> "
            },
            correctIndex = 1,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Em água pura, a concentração de H<sup><size=150%>+</size></sup> é:",
            answers = new string[] {
                "10<sup><size=150%>-14</size></sup> M",
                "10<sup><size=150%>-7</size></sup> M",
                "10<sup><size=150%>0</size></sup> M",
                "10<sup><size=150%>7</size></sup> M"
            },
            correctIndex = 1,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Em água pura, a concentração de OH<sup><size=150%>-</size></sup>  é:",
            answers = new string[] {
                "10<sup><size=150%>-14</size></sup> M",
                "10<sup><size=150%>-7</size></sup> M",
                "10<sup><size=150%>0</size></sup> M",
                "10<sup><size=150%>7</size></sup> M"
            },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Qual a fórmula para calcular o pH?",
            answers = new string[] {
                "pH = log[H<sup><size=150%>+</size></sup>]",
                "pH = -log[H<sup><size=150%>+</size></sup>]",
                "pH = log[OH<sup><size=150%>-</size></sup>]",
                "pH = -log[OH<sup><size=150%>-</size></sup>]"
            },
            correctIndex = 1,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Qual a fórmula para calcular o pOH?",
            answers = new string[] {
                "pOH = -log[OH<sup><size=150%>-</size></sup>]",
                "pOH = log[OH<sup><size=150%>-</size></sup>]",
                "pOH = -log[OH<sup><size=150%>+</size></sup>]",
                "pOH = log[OH<sup><size=150%>+</size></sup>]"
            },
            correctIndex = 0,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Qual o valor mínimo de pH possível?",
            answers = new string[] {
                "0",
                "7",
                "14",
                "-14"
            },
            correctIndex = 0,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Qual o valor máximo de pH possível?",
            answers = new string[] {
                "0",
                "7",
                "14",
                "-14"
            },
            correctIndex = 2,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Qual o pH de uma solução neutra?",
            answers = new string[] {
                "0",
                "7",
                "14",
                "Variavel"
            },
            correctIndex = 1,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Uma solução com pH abaixo de 7 é:",
            answers = new string[] {
                "Neutra",
                "Básica",
                "Ácida",
                "Tampão"
            },
            correctIndex = 2,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Uma solução com pH acima de 7 é:",
            answers = new string[] {
                "Ácida",
                "Neutra",
                "Básica",
                "Tampão"
            },
            correctIndex = 2,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O processo de neutralização envolve:",
            answers = new string[] {
                "A adição de um ácido a uma base.",
                "A adição de uma base a um ácido.",
                "A reação entre um ácido e uma base, resultando em água e um sal.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 2,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Durante uma titulação, o ponto de equivalência é atingido quando:",
            answers = new string[] {
                "A concentração de H+ é igual à concentração de OH<sup><size=150%>-</size></sup>. ",
                "O pH é igual a 0.",
                "O pH é igual a 7.",
                "O pH é igual a 14."
            },
            correctIndex = 0,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Um indicador de pH é uma substância que:",
            answers = new string[] {
                "Muda de cor em um determinado intervalo de pH.",
                "Muda de cor em qualquer pH.",
                "Mantém o pH constante.",
                "Neutraliza ácidos e bases."
            },
            correctIndex = 0,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O que é uma solução-tampão?",
            answers = new string[] {
                "Uma solução que resiste a mudanças de temperatura.",
                "Uma solução que resiste a mudanças de pressão.",
                "Uma solução que resiste a mudanças de pH.",
                "Uma solução que resiste a mudanças de volume."
            },
            correctIndex = 2,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Uma solução-tampão é tipicamente composta de:",
            answers = new string[] {
                "Um ácido forte e uma base forte.",
                "Um ácido fraco e sua base conjugada.",
                "Um ácido forte e sua base conjugada.",
                "Um ácido fraco e uma base forte."
            },
            correctIndex = 1,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A capacidade de tamponamento de uma solução-tampão é máxima em:",
            answers = new string[] {
                "pH = 0",
                "pH = 7",
                "pH = pKa",
                "pH = 14"
            },
            correctIndex = 2,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "A faixa de tamponamento de uma solução-tampão é aproximadamente:",
            answers = new string[] {
                "Igual ao pKa",
                "± 1 unidade de pH em relação ao pKa",
                "± 2 unidades de pH em relação ao pKa",
                "± 3 unidades de pH em relação ao pKa"
            },
            correctIndex = 1,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Qual a principal função do sistema tampão do sangue?",
            answers = new string[] {
                "Regular a temperatura corporal",
                "Manter o pH do sangue constante",
                "Regular a pressão sanguínea",
                "Transportar oxigênio"
            },
            correctIndex = 1,
            questionNumber = 48,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "O principal sistema tampão do sangue é o sistema:",
            answers = new string[] {
                "Fosfato",
                "Acetato",
                "Bicarbonato",
                "Hemoglobina"
            },
            correctIndex = 2,
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "AcidBaseBufferQuestionDatabase",
            questionText = "Durante o exercício intenso, o aumento da produção de ácido lático causa:",
            answers = new string[] {
                "Aumento do pH do sangue",
                "Diminuição do pH do sangue",
                "Aumento da taxa respiratória",
                "Diminuição da taxa respiratória"
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
        return QuestionSet.acidsBase;
    }

    public string GetDatabankName()
    {
        return "AcidBaseBufferQuestionDatabase";
    }
}