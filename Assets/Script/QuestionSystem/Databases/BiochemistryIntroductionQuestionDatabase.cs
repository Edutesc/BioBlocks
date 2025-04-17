using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class BiochemistryIntroductionQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "São características de sistemas vivos, exceto:",
            answers = new string[] {
                "Utilizar a energia do ambiente",
                "Possuir organização microcópica.",
                "Capacidade de se autoreplicar",
                "Não respondem as alterações do ambiente"
            },
                correctIndex = 3,
                questionNumber = 1,
                isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a teoria que afirmava que os seres vivos possuíam uma força vital?",
            answers = new string[] {
                "Teoria da abiogênese",
                "Teoria da biogênese",
                "Vitalismo",
                "Teoria celular"
            },
                correctIndex = 2,
                questionNumber = 2,
                isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual fato mais contribuiu para a perda de credibilidade da doutrina do vitalismo?",
            answers = new string[] {
                "A criação da tabela periódica dos elementos",
                "O fim da crença na geração espontânea dos seres vivos",
                "O desenvolvimento da teoria atómica dos elementos",
                "A lei da conservação das massas"
            },
                correctIndex = 1,
                questionNumber = 3,
                isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a importância de Louis Pasteur para o desenvolvimento da Bioquímica?",
            answers = new string[] {
                "Pasteur contribuiu para demonstrar que a vida surgia espontaneamente.",
                "Pasteur estabeleceu a relação entre microorganismos e doenças, desafiando a geração espontânea.",
                "Pasteur desenvolveu métodos de esterilização de alimentos por luz ultravioleta.",
                "Pasteur foi o primeiro a descobrir o DNA como material genético."
            },
            correctIndex = 1,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Quais os quatro elementos mais abundantes nos organismos vivos?",
            answers = new string[] { 
                "H, O, N, C", 
                "C, H, O, P", 
                "C, N, O, S", 
                "H, O, P, S" 
                },
            correctIndex = 0,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que caracteriza as biomoléculas?",
            answers = new string[] {
                "São compostos inorgânicos.",
                "São compostos orgânicos baseados em carbono.",
                "São sempre polímeros.",
                "São sempre moléculas pequenas."
            },
            correctIndex = 1,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que são grupos funcionais?",
            answers = new string[] {
                "Grupos de átomos que conferem propriedades químicas específicas às moléculas.",
                "Grupos de átomos que conferem estrutura às moléculas.",
                "Grupos de átomos que conferem cor às moléculas.",
                "Grupos de átomos que conferem sabor às moléculas."
            },
            correctIndex = 0,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Identifique a estrutura que representa um hidrocarboneto ramificado",
            answers = new string[] {
                "AnswerImages/IntroductionDB/benzeno",
                "AnswerImages/IntroductionDB/enol",
                "AnswerImages/IntroductionDB/hidrocarboneto_ramificado",
                "AnswerImages/IntroductionDB/propanal"
            },
            correctIndex = 2,
            questionNumber = 8,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Identifique a estrutura molecular do ácido carboxílico",
            answers = new string[] {
                "AnswerImages/IntroductionDB/2-butanona",
                "AnswerImages/IntroductionDB/propanoato_de_metila",
                "AnswerImages/IntroductionDB/propamida",
                "AnswerImages/IntroductionDB/acido_propanoico"
            },
            correctIndex = 3,
            questionNumber = 9,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Identifique a estrutura molecular da amina",
            answers = new string[] {
                "AnswerImages/IntroductionDB/2-butamina",
                "AnswerImages/IntroductionDB/propanoato_de_metila",
                "AnswerImages/IntroductionDB/propamida",
                "AnswerImages/IntroductionDB/acido_propanoico"
            },
            correctIndex = 0,
            questionNumber = 10,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é um isômero?",
            answers = new string[] {
                "Molécula com fórmula molecular diferente.",
                "Molécula com fórmula molecular igual, mas arranjo atômico diferente.",
                "Molécula com mesma função biológica.",
                "Molécula com mesmo ponto de fusão."
            },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que são enantiômeros?",
            answers = new string[] {
                "Isômeros que são imagens especulares não sobreponíveis.",
                "Isômeros que são imagens especulares sobreponíveis.",
                "Isômeros com diferentes grupos funcionais.",
                "Isômeros com diferente número de átomos."
            },
            correctIndex = 0,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é um carbono quiral?",
            answers = new string[] {
                "Um carbono com ligação dupla.",
                "Um carbono com ligação tripla.",
                "Um carbono ligado a quatro grupos diferentes.",
                "Um carbono terminal."
            },
            correctIndex = 2,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Identifique a estrutura molecular que possui um carbono quiral.",
            answers = new string[] {
                "AnswerImages/IntroductionDB/2-butamina",
                "AnswerImages/IntroductionDB/propanoato_de_metila",
                "AnswerImages/IntroductionDB/propamida",
                "AnswerImages/IntroductionDB/acido_propanoico"
            },
            correctIndex = 0,
            questionNumber = 14,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Indique a estrutura molecular de um aldeído.",
            answers = new string[] {
                "AnswerImages/IntroductionDB/propanal",
                "AnswerImages/IntroductionDB/propanoato_de_metila",
                "AnswerImages/IntroductionDB/propamida",
                "AnswerImages/IntroductionDB/acido_propanoico"
                },
            correctIndex = 0,
            questionNumber = 15,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Indique a estrutura molecular de um álcool.",
            answers = new string[] {
                "AnswerImages/IntroductionDB/propanal",
                "AnswerImages/IntroductionDB/propanol",
                "AnswerImages/IntroductionDB/propamida",
                "AnswerImages/IntroductionDB/acido_propanoico"
                },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Indique a estrutura molecular de um amida.",
            answers = new string[] {
                "AnswerImages/IntroductionDB/propanal",
                "AnswerImages/IntroductionDB/propanoato_de_metila",
                "AnswerImages/IntroductionDB/propamida",
                "AnswerImages/IntroductionDB/acido_propanoico"
                },
            correctIndex = 2,
            questionNumber = 17,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Indique a estrutura molecular de um éster.",
            answers = new string[] {
                "AnswerImages/IntroductionDB/propanal",
                "AnswerImages/IntroductionDB/propanoato_de_metila",
                "AnswerImages/IntroductionDB/propamida",
                "AnswerImages/IntroductionDB/acido_propanoico"
                },
            correctIndex = 1,
            questionNumber = 18,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Indique a estrutura molecular de uma cetona.",
            answers = new string[] {
                "AnswerImages/IntroductionDB/amida_carga_positiva",
                "AnswerImages/IntroductionDB/propanoato",
                "AnswerImages/IntroductionDB/propil_imino",
                "AnswerImages/IntroductionDB/2-butanona"
                },
            correctIndex = 3,
            questionNumber = 19,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Identifique o composto que não é um isômero dos outros 3 compostos.",
            answers = new string[] {
                "AnswerImages/IntroductionDB/ciclohexano",
                "AnswerImages/IntroductionDB/2-3-dimetil-butano",
                "AnswerImages/IntroductionDB/2metil-pentano",
                "AnswerImages/IntroductionDB/3metil-pentano"
            },
            correctIndex = 0,
            questionNumber = 20,
            isImageAnswer = true
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Quais os quatro grupos principais de macromoléculas?",
            answers = new string[] {
                "Proteínas, lipídeos, carboidratos e ácidos nucléicos",
                "Proteínas, carboidratos, água e sais minerais",
                "Lipídeos, carboidratos, água e sais minerais",
                "Proteínas, ácidos nucléicos, água e sais minerais"
                },
            correctIndex = 0,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que são macromoléculas?",
            answers = new string[] {
                "Moléculas com massa molecular inferior a 1000 daltons",
                "Moléculas com massa molecular superior a milhares de daltons",
                "Moléculas de água",
                "Moléculas de sais minerais"
                },
            correctIndex = 1,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que significa polimerizar?",
            answers = new string[] {
                "Quebrar moléculas",
                "Formar monômeros",
                "Unir unidades monoméricas",
                "Formar isômeros"
                },
            correctIndex = 2,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual o objetivo da Bioquímica?",
            answers = new string[] {
                "Estudar os seres vivos",
                "Estudar as moléculas que constituem a vida",
                "Estudar a água",
                "Estudar os ácidos nucléicos"
                },
            correctIndex = 1,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual observação foi feita em relação aos aminoácidos encontrados no experimento de Miller-Urey?",
            answers = new string[] {
                "Eles encontraram tipos variados de aminoácidos",
                "Nenhum aminoácido foi encontrado",
                "Apenas um aminoácido foi produzido",
                "Os aminoácidos eram sólidos"
                },
            correctIndex = 0,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é um estado estacionário dinâmico?",
            answers = new string[] {
                "Um sistema em equilíbrio com o seu meio.",
                "Um sistema fora do equilíbrio, mas com composição aproximadamente constante.",
                "Um sistema com alta entropia.",
                "Um sistema com baixa entropia."
            },
            correctIndex = 1,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é entropia?",
            answers = new string[] {
                "A capacidade de realizar trabalho.",
                "A capacidade de armazenar energia.",
                "Uma medida da desordem ou aleatoriedade de um sistema.",
                "Uma medida da ordem de um sistema."
            },
            correctIndex = 2,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a primeira lei da termodinâmica?",
            answers = new string[] {
                "A energia não pode ser criada nem destruída, apenas transformada.",
                "A entropia do universo tende a aumentar.",
                "A energia livre de um sistema tende a diminuir.",
                "A energia não pode ser criada nem destruída."
            },
            correctIndex = 0,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a segunda lei da termodinâmica?",
            answers = new string[] {
                "A energia não pode ser criada nem destruída, apenas transformada.",
                "A entropia do universo tende a aumentar.",
                "A energia livre de um sistema tende a diminuir.",
                "A energia não pode ser criada nem destruída."
            },
            correctIndex = 1,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que significa uma reação exergônica?",
            answers = new string[] {
                "Libera energia",
                "Requer energia",
                "Está em equilíbrio",
                "Não envolve energia"
                },
            correctIndex = 0,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que significa uma reação endergônica?",
            answers = new string[] {
                "Libera energia",
                "Requer energia",
                "Está em equilíbrio",
                "Não envolve energia"
                },
            correctIndex = 1,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é a constante de equilíbrio (Keq)?",
            answers = new string[] {
                "Uma medida da velocidade da reação.",
                "Uma medida da tendência da reação em atingir o equilíbrio.",
                "Uma medida da energia ativada de uma reação.",
                "Uma medida da entropia de uma reação."
            },
            correctIndex = 1,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é acoplamento energético?",
            answers = new string[] {
                "A combinação de duas reações em equilíbrio.",
                "A combinação de uma reação exergônica e uma reação endergônica.",
                "A combinação de duas reações exergônicas.",
                "A combinação de duas reações endergônicas."
            },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a principal fonte de energia livre em reações biológicas?",
            answers = new string[] {
                "Glicose",
                "ATP",
                "Lipídeos",
                "Proteínas"
                },
            correctIndex = 1,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é a energia de ativação (ΔG‡)?",
            answers = new string[] {
                "A energia necessária para que uma reação ocorra espontaneamente.",
                "A energia liberada por uma reação exergônica.",
                "A energia necessária para transpor a barreira de ativação.",
                "A energia armazenada em uma ligação química."
            },
            correctIndex = 2,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a principal função do ATP na célula?",
            answers = new string[] {
                "Armazenamento de informação genética",
                "Transporte de moléculas",
                "Carreador de energia",
                "Catálise de reações"
                },
            correctIndex = 2,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a atividade que mais consome energia nas células?",
            answers = new string[] {
                "Transporte de íons",
                "Síntese de macromoléculas",
                "Respiração celular",
                "Fotossíntese"
                },
            correctIndex = 1,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que são complexos supramoleculares?",
            answers = new string[] {
                "Moléculas pequenas.",
                "Macromoléculas.",
                "Agregados de macromoléculas.",
                "Componentes celulares."
            },
            correctIndex = 2,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que é o citoesqueleto?",
            answers = new string[] {
                "Rede de proteínas que dá forma e rigidez às células.",
                "Rede de carboidratos que dá forma e rigidez às células.",
                "Rede de lipídeos que dá forma e rigidez às células.",
                "Rede de ácidos nucléicos que dá forma e rigidez às células."
            },
            correctIndex = 0,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que a entropia mede?",
            answers = new string[] {
                "Energia",
                "Trabalho",
                "Desordem",
                "Ordem"
                },
            correctIndex = 2,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Em qual estado os organismos vivos existem em relação ao seu meio?",
            answers = new string[] {
                "Equilíbrio",
                "Estado estacionário dinâmico",
                "Alta entropia",
                "Baixa entropia"
                },
            correctIndex = 1,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Em que tipo de ambiente a vida provavelmente surgiu?",
            answers = new string[] {
                "Ambiente quente",
                "Ambiente frio",
                "Ambiente seco",
                "Ambiente oceânico"
                },
            correctIndex = 3,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Quais os três estágios geralmente aceitos para o desenvolvimento da vida?",
            answers = new string[] {
                "Evolução química, auto-organização, evolução biológica",
                "Evolução química, replicação, mutação",
                "Auto-organização, replicação, seleção natural",
                "Evolução biológica, seleção natural, extinção"
            },
            correctIndex = 0,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual a característica principal que torna o carbono único na formação de biomoléculas?",
            answers = new string[] {
                "Forma apenas ligações simples.",
                "Forma ligações simples, duplas e triplas.",
                "É o elemento mais abundante na Terra.",
                "É altamente reativo."
            },
            correctIndex = 1,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual experimento simulou a síntese de compostos orgânicos na Terra primitiva?",
            answers = new string[] {
                "Experimento de Miller-Urey",
                "Experimento de Hershey-Chase",
                "Experimento de Meselson-Stahl",
                "Experimento de Griffith"
                },
            correctIndex = 0,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Qual o tipo de atmosfera que provavelmente existia na Terra primitiva?",
            answers = new string[] {
                "Atmosfera oxidante",
                "Atmosfera redutora",
                "Atmosfera neutra",
                "Atmosfera rica em oxigênio"
                },
            correctIndex = 1,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "O que impulsionou o desenvolvimento de rotas metabólicas nas células?",
            answers = new string[] {
                "Aumento da temperatura.",
                "Diminuição da temperatura.",
                "Competição por recursos.",
                "Mutações genéticas."
            },
            correctIndex = 2,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Que processo metabólico representou uma solução para a crise de energia enfrentada pelos primeiros organismos vivos?",
            answers = new string[] {
                "Respiração aeróbica",
                "Fermentação",
                "Fotossíntes",
                "Quimiossíntese"
            },
            correctIndex = 2,
            questionNumber = 48,
            isImageAnswer = false
        },
         new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Selecione o aminoácido que não possui carbono quiral",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/cisteina",
                "AnswerImages/AminoacidsDB/glicina",
                "AnswerImages/AminoacidsDB/alanina",
                "AnswerImages/AminoacidsDB/histidina"
                },
            correctIndex = 1,
            questionNumber = 49,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "BiochemistryIntroductionQuestionDatabase",
            questionText = "Identifique o isômeto cis nas estruturas abaixo",
            answers = new string[] {
                "AnswerImages/IntroductionDB/trans-dihidroxi-eteno",
                "AnswerImages/IntroductionDB/cis-dihidroxi-eteno",
                "AnswerImages/IntroductionDB/dihidroxi-eteno",
                "AnswerImages/IntroductionDB/dihidroxi-eteno2"
            },
            correctIndex = 1,
            questionNumber = 50,
            isImageAnswer = true
        }
    };

    public List<Question> GetQuestions()
    {
        return questions;
    }

    public QuestionSet GetQuestionSetType()
    {
        return QuestionSet.biochem;
    }

    public string GetDatabankName()
    {
        return "BiochemistryIntroductionQuestionDatabase";
    }
}
