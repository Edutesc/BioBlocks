using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class WaterQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual a principal razão para a alta capacidade calorífica da água?",
            answers = new string[] {
                "Fortes ligações covalentes entre átomos de hidrogênio e oxigênio.",
                "Intensas ligações de hidrogênio entre moléculas de água.",
                "Alto peso molecular das moléculas de água.",
                "Seu estado líquido em temperatura ambiente."
            },
            correctIndex = 1,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual das propriedades da água é MAIS responsável por sua capacidade de moderar a temperatura?",
            answers = new string[] {
                "Alto calor de vaporização",
                "Alta tensão superficial",
                "Alto calor específico",
                "Alta densidade"
            },
            correctIndex = 2,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Por que o gelo flutua na água?",
            answers = new string[] {
                "O gelo é menos denso que a água líquida.",
                "O gelo possui maior tensão superficial que a água líquida.",
                "O gelo possui menor calor específico que a água líquida.",
                "O gelo é uma substância diferente da água."
            },
            correctIndex = 0,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual propriedade da água permite que insetos caminhem na superfície?",
            answers = new string[] {
                "Alto calor específico",
                "Alto calor de vaporização",
                "Alta tensão superficial",
                "Baixa densidade"
            },
            correctIndex = 2,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText= "Suar ajuda a esfriar o corpo principalmente devido ao:",
            answers = new string[] {
                "Alto calor específico da água",
                "Alta densidade da água",
                "Alto calor de vaporização da água",
                "Baixa viscosidade da água"
            },
            correctIndex = 2,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "O alto calor de vaporização da água se deve principalmente a:",
            answers = new string[] {
                "Fortes ligações covalentes",
                "Intensas ligações de hidrogênio",
                "Alta tensão superficial",
                "Baixa viscosidade"
            },
            correctIndex = 1,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual substância requer mais energia para elevar sua temperatura em 1°C?",
            answers = new string[] {
                "Álcool",
                "Água",
                "Clorofórmio",
                "Todas requerem a mesma quantidade de energia"
            },
            correctIndex = 1,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Uma substância com alto calor de vaporização irá:",
            answers = new string[] {
                "Resfriar rapidamente ao evaporar",
                "Aquecer rapidamente ao evaporar",
                "Manter temperatura constante durante a evaporação",
                "Não evaporar facilmente"
            },
            correctIndex = 0,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Por que a água é um excelente solvente para muitos compostos iônicos?",
            answers = new string[] {
                "Alta tensão superficial",
                "Alta polaridade e formação de ligações de hidrogênio",
                "Baixa viscosidade",
                "Alta capacidade calorífica"
            },
            correctIndex = 1,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "O que causa a alta tensão superficial da água?",
            answers = new string[] {
                "Ligações covalentes entre H e O",
                "Ligações de hidrogênio entre moléculas de água",
                "Interações iônicas entre moléculas de água",
                "Forças de Van der Waals"
            },
            correctIndex = 1,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "A polaridade da molécula de água se deve a:",
            answers = new string[] {
                "Compartilhamento igual de elétrons",
                "Compartilhamento desigual de elétrons",
                "Presença de ligações de hidrogênio",
                "Alto peso molecular"
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "A água é uma molécula polar porque:",
            answers = new string[] {
                "Forma simétrica",
                "Extremidades levemente positivas e negativas",
                "É líquida em temperatura ambiente",
                "Dissolve facilmente substâncias apolares"
            },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "O que NÃO é consequência das ligações de hidrogênio na água?",
            answers = new string[] {
                "Alta tensão superficial",
                "Alto ponto de ebulição",
                "Alta pressão de vapor",
                "Gelo menos denso que água líquida"
            },
            correctIndex = 2,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Que tipo de ligação causa a coesão entre moléculas de água?",
            answers = new string[] {
                "Iônicas",
                "Covalentes",
                "Hidrogênio",
                "Van der Waals"
            },
            correctIndex = 2,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Ligações de hidrogênio se formam entre:",
            answers = new string[] {
                "Dois átomos de hidrogênio",
                "Hidrogênio e oxigênio",
                "Dois átomos de oxigênio",
                "Carbono e hidrogênio"
            },
            correctIndex = 1,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Comparadas a ligações covalentes, ligações de hidrogênio são:",
            answers = new string[] {
                "Mais fortes",
                "Mais fracas",
                "De mesma força",
                "Apenas na água"
            },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "A geometria de uma molécula de água é:",
            answers = new string[] {
                "Linear",
                "Angular",
                "Tetraédrica",
                "Trigonal planar"
            },
            correctIndex = 1,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "A região ligeiramente negativa de uma molécula de água está em torno do:",
            answers = new string[] {
                "Átomos de hidrogênio",
                "Átomo de oxigênio",
                "Centro da molécula",
                "Não há região negativa"
            },
            correctIndex = 1,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Molécula com regiões polares e apolares é chamada de:",
            answers = new string[] {
                "Anfipática",
                "Hidrofílica",
                "Hidrofóbica",
                "Polar"
            },
            correctIndex = 0,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual tipo de molécula se dissolve mais facilmente em água?",
            answers = new string[] {
                "Apolar",
                "Polar",
                "Grande",
                "Pequena"
            },
            correctIndex = 1,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Substância que se dissolve em água é:",
            answers = new string[] {
                "Hidrofóbica",
                "Hidrofílica",
                "Anfipática",
                "Aquosa"
            },
            correctIndex = 1,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Substância que NÃO se dissolve em água é:",
            answers = new string[] {
                "Hidrofílica",
                "Hidrofóbica",
                "Anfipática",
                "Aquosa"
            },
            correctIndex = 1,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "A dissolução de composto iônico em água se deve a:",
            answers = new string[] {
                "Ligações de hidrogênio",
                "Interações hidrofóbicas",
                "Interações íon-dipolo",
                "Forças de Van der Waals"
            },
            correctIndex = 2,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Grupo de moléculas de água em torno de um íon é chamado de:",
            answers = new string[] {
                "Camada de hidratação",
                "Ligação de hidrogênio",
                "Micela",
                "Núcleo hidrofóbico"
            },
            correctIndex = 0,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Alta constante dielétrica indica capacidade de:",
            answers = new string[] {
                "Reduzir força entre íons opostos",
                "Aumentar força entre íons opostos",
                "Não afetar interações iônicas",
                "Dissolver apenas substâncias apolares"
            },
            correctIndex = 0,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "O que NÃO afeta a solubilidade de gases em água?",
            answers = new string[] {
                "Temperatura",
                "Pressão",
                "Polaridade do gás",
                "Peso molecular do solvente"
            },
            correctIndex = 3,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "A solubilidade de gases em água:",
            answers = new string[] {
                "Aumenta com temperatura",
                "Diminui com temperatura",
                "Não é afetada pela temperatura",
                "É afetada apenas pela pressão"
            },
            correctIndex = 1,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Como detergentes removem gordura?",
            answers = new string[] {
                "Dissolvendo diretamente",
                "Formando micelas",
                "Reagindo quimicamente",
                "Aumentando tensão superficial"
            },
            correctIndex = 1,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Estruturas formadas por moléculas anfipáticas em água são:",
            answers = new string[] {
                "Camadas de hidratação",
                "Micelas",
                "Ligações de hidrogênio",
                "Ligações covalentes"
            },
            correctIndex = 1,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Membranas celulares são compostas principalmente por:",
            answers = new string[] {
                "Proteínas",
                "Carboidratos",
                "Ácidos nucléicos",
                "Fosfolipídios"
            },
            correctIndex = 3,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Por que membranas celulares são fluidas?",
            answers = new string[] {
                "Fosfolipídios compactados",
                "Fosfolipídios livres para se mover",
                "Proteínas fixas aos fosfolipídios",
                "Membrana impermeável à água"
            },
            correctIndex = 1,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Caudas hidrofóbicas de fosfolipídios em membranas celulares ficam:",
            answers = new string[] {
                "Ambiente extracelular",
                "Ambiente intracelular",
                "Uma para a outra, no interior da membrana",
                "Ambientes extra e intracelular"
            },
            correctIndex = 2,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual desses gases é MENOS solúvel em água?",
            answers = new string[] {
                "Oxigênio",
                "Dióxido de carbono",
                "Nitrogênio",
                "Amônia"
            },
            correctIndex = 2,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Baixa solubilidade de gases apolares em água se deve a:",
            answers = new string[] {
                "Fortes ligações de hidrogênio",
                "Ausência de ligações de hidrogênio",
                "Alta tensão superficial",
                "Alta capacidade calorífica"
            },
            correctIndex = 1,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Peixes obtêm oxigênio da água através de:",
            answers = new string[] {
                "Pele",
                "Brânquias",
                "Bebendo água",
                "Pulmões"
            },
            correctIndex = 1,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Função principal das brânquias em peixes é:",
            answers = new string[] {
                "Filtrar alimento",
                "Regular temperatura",
                "Extrair oxigênio",
                "Controlar flutuabilidade"
            },
            correctIndex = 2,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Fórmula química da água:",
            answers = new string[] {
                "H<sub><size=150%>2</size></sub> O<sub><size=150%>2</size></sub>",
                "H<sub><size=150%>2</size></sub> O",
                "CO<sub><size=150%>2</size></sub>",
                "NaCl"
            },
            correctIndex = 1,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Peso molecular da água:",
            answers = new string[] {
                "16 g/mol",
                "18 g/mol",
                "32 g/mol",
                "44 g/mol"
            },
            correctIndex = 1,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Densidade da água pura a 4°C:",
            answers = new string[] {
                "0,92 g/mL",
                "1,00 g/mL",
                "1,10 g/mL",
                "1,80 g/mL"
            },
            correctIndex = 1,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Ponto de ebulição da água pura ao nível do mar:",
            answers = new string[] {
                "0°C",
                "120°C",
                "212°C",
                "100°C"
            },
            correctIndex = 3,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Ponto de congelamento da água pura ao nível do mar:",
            answers = new string[] {
                "10°C",
                "100°C",
                "32°C",
                "0°C"
            },
            correctIndex = 3,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "pH da água pura:",
            answers = new string[] {
                "0",
                "7",
                "14",
                "Variável"
            },
            correctIndex = 1,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Água pura é:",
            answers = new string[] {
                "Ácida",
                "Básica",
                "Neutra",
                "Depende da temperatura"
            },
            correctIndex = 2,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual ligação é mais forte?",
            answers = new string[] {
                "Hidrogênio",
                "Covalente",
                "Força igual",
                "Depende dos átomos"
            },
            correctIndex = 1,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Atração entre moléculas de água:",
            answers = new string[] {
                "Coesão",
                "Adesão",
                "Tensão superficial",
                "Ação capilar"
            },
            correctIndex = 0,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Atração entre água e outras substâncias polares:",
            answers = new string[] {
                "Coesão",
                "Adesão",
                "Tensão superficial",
                "Ação capilar"
            },
            correctIndex = 1,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Movimento da água contra a gravidade em plantas:",
            answers = new string[] {
                "Osmose",
                "Difusão",
                "Transpiração",
                "Ação capilar"
            },
            correctIndex = 2,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual propriedade da água é responsável pelo menisco em uma proveta?",
            answers = new string[] {
                "Coesão",
                "Adesão",
                "Tensão superficial",
                "Densidade"
            },
            correctIndex = 2,
            questionNumber = 48,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "Qual o nome dado a uma substância com regiões hidrofóbicas e hidrofílicas?",
            answers = new string[] {
                "Anfipática",
                "Hidrofóbica",
                "Hidrofílica",
                "Polar"
            },
            correctIndex = 0,
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "WaterQuestionDatabase",
            questionText = "O que é uma micela?",
            answers = new string[] {
                "Um tipo de ligação covalente",
                "Um tipo de ligação de hidrogênio",
                "Um agregado de moléculas anfipáticas",
                "Um tipo de proteína"
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
        return QuestionSet.water;
    }

    public string GetDatabankName()
    {
        return "WaterQuestionDatabase";
    }
}