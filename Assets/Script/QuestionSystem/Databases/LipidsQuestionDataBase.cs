using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class LipidsQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O que são lipídios?",
            answers = new string[] {
                "Moléculas polares solúveis em água.",
                "Moléculas apolares, geralmente insolúveis em água.",
                "Polímeros de carboidratos.",
                "Monômeros de proteínas."
            },
            correctIndex = 1,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Os lipídeos são solúveis em:",
            answers = new string[] {
                "Água",
                "Solventes orgânicos (como clorofórmio e éter)",
                "Soluções ácidas",
                "Soluções básicas"
            },
            correctIndex = 1,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O termo lipofílico se refere a:",
            answers = new string[] {
                "Afinidade por água",
                "Afinidade por lipídios",
                "Afinidade por solventes polares",
                "Afinidade por altas temperaturas"
            },
            correctIndex = 1,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O termo hidrofóbico se refere a:",
            answers = new string[] {
                "Repulsão por água",
                "Afinidade por água",
                "Afinidade por solventes orgânicos",
                "Afinidade por altas temperaturas"
            },
            correctIndex = 0,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Moléculas com regiões polares e apolares são chamadas:",
            answers = new string[] {
                "Hidrofílicas",
                "Hidrofóbicas",
                "Anfipáticas",
                "Apolares"
            },
            correctIndex = 2,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Em soluções aquosas, moléculas anfipáticas tendem a formar:",
            answers = new string[] {
                "Soluções homogêneas",
                "Micelas",
                "Suspensões coloidais",
                "Emulsões estáveis"
            },
            correctIndex = 1,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Os lipídeos mais simples são:",
            answers = new string[] {
                "Triglicerídeos",
                "Fosfolipídios",
                "Ácidos graxos",
                "Esteróides"
            },
            correctIndex = 2,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O que caracteriza um ácido graxo?",
            answers = new string[] {
                "Uma longa cadeia de hidrocarbonetos com um grupo carboxila.",
                "Um anel de hidrocarbonetos com um grupo amino.",
                "Uma cadeia curta de hidrocarbonetos com um grupo fosfato.",
                "Um açúcar com múltiplos grupos hidroxila."
            },
            correctIndex = 0,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos saturados possuem:",
            answers = new string[] {
                "Apenas ligações simples carbono-carbono.",
                "Uma ou mais ligações duplas carbono-carbono.",
                "Um grupo amino.",
                "Um grupo fosfato."
            },
            correctIndex = 0,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos insaturados possuem:",
            answers = new string[] {
                "Apenas ligações simples carbono-carbono.",
                "Uma ou mais ligações duplas carbono-carbono.",
                "Um grupo fosfato.",
                "Um grupo amino."
            },
            correctIndex = 1,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O ponto de fusão de um ácido graxo é afetado por:",
            answers = new string[] {
                "Seu comprimento.",
                "Seu grau de insaturação.",
                "Ambos os itens anteriores.",
                "Nenhum dos itens anteriores."
            },
            correctIndex = 2,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos saturados geralmente são:",
            answers = new string[] {
                "Líquidos à temperatura ambiente.",
                "Sólidos à temperatura ambiente.",
                "Gasosos à temperatura ambiente.",
                "Insolúveis em solventes orgânicos."
            },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos insaturados geralmente são:",
            answers = new string[] {
                "Sólidos à temperatura ambiente.",
                "Líquidos à temperatura ambiente.",
                "Gasosos à temperatura ambiente.",
                "Insolúveis em solventes orgânicos."
            },
            correctIndex = 1,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O que é uma ligação éster?",
            answers = new string[] {
                "Ligação entre dois átomos de carbono.",
                "Ligação entre um carbono e um oxigênio.",
                "Ligação entre um ácido carboxílico e um álcool.",
                "Ligação entre dois grupos amino."
            },
            correctIndex = 2,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Os triacilgliceróis são formados por:",
            answers = new string[] {
                "Três ácidos graxos e uma molécula de glicerol.",
                "Dois ácidos graxos e uma molécula de glicerol.",
                "Um ácido graxo e uma molécula de glicerol.",
                "Três ácidos graxos e duas moléculas de glicerol."
            },
            correctIndex = 0,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
             questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Qual a principal função dos triacilgliceróis no organismo?",
            answers = new string[] {
                "Formar membranas celulares.",
                "Armazenar energia.",
                "Sintetizar hormônios.",
                "Transportar oxigênio."
            },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Os triacilgliceróis são armazenados principalmente em:",
            answers = new string[] {
                "Fígado",
                "Músculos",
                "Cérebro",
                "Células adiposas"
            },
            correctIndex = 3,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O tecido adiposo tem como função:",
            answers = new string[] {
                "Armazenar gordura.",
                "Isolar o organismo.",
                "Proteger órgãos.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A hibernação é uma estratégia de sobrevivência que envolve:",
            answers = new string[] {
                "Aumento do consumo de oxigênio.",
                "Diminuição do consumo de oxigênio.",
                "Armazenamento de lipídeos.",
                "Aumento da atividade enzimática."
            },
            correctIndex = 2,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Em comparação com carboidratos e proteínas, os triacilgliceróis armazenam:",
            answers = new string[] {
                "Menor quantidade de energia por grama.",
                "Maior quantidade de energia por grama.",
                "A mesma quantidade de energia por grama.",
                "Não armazenam energia."
            },
            correctIndex = 1,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Os ácidos graxos essenciais são aqueles que:",
            answers = new string[] {
                "O organismo produz em grande quantidade.",
                "O organismo não consegue sintetizar.",
                "São encontrados apenas em animais.",
                "São encontrados apenas em plantas."
            },
            correctIndex = 1,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Exemplos de ácidos graxos essenciais são:",
            answers = new string[] {
                "Ácido esteárico e ácido palmítico.",
                "Ácido linoléico e ácido alfa-linolênico.",
                "Ácido oléico e ácido palmitoléico.",
                "Ácido araquidônico e ácido eicosapentaenóico."
            },
            correctIndex = 1,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O ácido linoléico (ômega-6) é precursor de:",
            answers = new string[] {
                "Prostaglandinas e tromboxanas.",
                "Vitamina D.",
                "Colesterol",
                "Glicogênio"
            },
            correctIndex = 0,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O ácido alfa-linolênico (ômega-3) é importante para:",
            answers = new string[] {
                "O desenvolvimento cerebral.",
                "A função imunológica.",
                "A saúde da retina.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A deficiência de ácidos graxos essenciais pode causar:",
            answers = new string[] {
                "Dermatite.",
                "Problemas neurológicos.",
                "Problemas no desenvolvimento de bebês.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos trans são:",
            answers = new string[] {
                "Ácidos graxos encontrados na natureza.",
                "Ácidos graxos produzidos pelo organismo em grandes quantidades.",
                "Ácidos graxos insaturados com configuração trans.",
                "Ácidos graxos saturados com configuração cis."
            },
            correctIndex = 2,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O consumo de ácidos graxos trans está associado a:",
            answers = new string[] {
                "Diminuição do risco de doenças cardíacas.",
                "Aumento do risco de doenças cardíacas.",
                "Nenhuma alteração no risco de doenças cardíacas.",
                "Aumento da produção de HDL."
            },
            correctIndex = 1,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A margarina é um composto:",
            answers = new string[] {
                "Natural, composto somente por ácidos graxos saturados.",
                "Artificial, composto somente por ácidos graxos insaturados.",
                "Artificial, composto por ácidos graxos saturados e insaturados.",
                "Natural, composto somente por ácidos graxos trans."
            },
            correctIndex = 2,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A hidrogenação de óleos vegetais resulta em:",
            answers = new string[] {
                "Aumento do grau de insaturação.",
                "Diminuição do grau de insaturação.",
                "Aumento do ponto de fusão.",
                "Diminuição do ponto de fusão."
            },
            correctIndex = 2,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A obesidade é definida como:",
            answers = new string[] {
                "Um peso corporal abaixo do ideal.",
                "Um peso corporal acima do ideal.",
                "Um estado de excesso de lipídeos no organismo.",
                "Um distúrbio metabólico."
            },
            correctIndex = 1,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O índice de massa corpórea (IMC) é calculado como:",
            answers = new string[] {
                "Peso dividido pela altura.",
                "Peso dividido pelo quadrado da altura.",
                "Altura dividida pelo peso.",
                "Quadrado da altura dividido pelo peso."
            },
            correctIndex = 1,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A obesidade aumenta o risco de:",
            answers = new string[] {
                "Doenças cardíacas.",
                "Diabetes.",
                "Derrame cerebral.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O que acontece com os adipócitos quando uma pessoa engorda?",
            answers = new string[] {
                "Diminuem em tamanho.",
                "Aumentam em tamanho e número.",
                "Diminuem em número.",
                "Permanecem inalterados."
            },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A principal forma de armazenamento de energia em nosso organismo é:",
            answers = new string[] {
                "Glicogênio",
                "Proteínas",
                "Triacilgliceróis",
                "Ácidos nucléicos"
            },
            correctIndex = 2,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Em comparação com o glicogênio, os triacilgliceróis armazenam:",
            answers = new string[] {
                "Menos energia por grama.",
                "Mais energia por grama.",
                "A mesma quantidade de energia por grama.",
                "Não armazenam energia."
            },
            correctIndex = 1,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "As células especializadas no armazenamento de triacilgliceróis nos mamíferos são:",
            answers = new string[] {
                "Hepatócitos",
                "Miócitos",
                "Neurônios",
                "Adipócitos"
            },
            correctIndex = 3,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O colesterol é um exemplo de:",
            answers = new string[] {
                "Ácido graxo",
                "Triglicerídeo",
                "Esteroide",
                "Fosfolipídio"
            },
            correctIndex = 2,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O colesterol tem como função:",
            answers = new string[] {
                "Formar membranas celulares.",
                "Ser precursor de hormônios.",
                "Ser precursor de sais biliares.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Lipídeos nos alvéolos pulmonares atuam:",
            answers = new string[] {
                "Facilitando a difusão de gases.",
                "Diminuindo a tensão superficial.",
                "Protegendo o tecido pulmonar.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 1,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A bainha de mielina é composta principalmente por:",
            answers = new string[] {
                "Proteínas",
                "Carboidratos",
                "Lipídios",
                "Ácidos nucléicos"
            },
            correctIndex = 2,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A bainha de mielina tem como função:",
            answers = new string[] {
                "Isolar os neurônios.",
                "Aumentar a velocidade do impulso nervoso.",
                "Proteger os neurônios.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Os ácidos graxos são classificados em:",
            answers = new string[] {
                "Saturados e insaturados.",
                "Polares e apolares.",
                "Essenciais e não-essenciais.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos saturados são encontrados principalmente em:",
            answers = new string[] {
                "Óleos vegetais.",
                "Gorduras animais.",
                "Frutas.",
                "Legumes."
            },
            correctIndex = 1,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos insaturados são encontrados principalmente em:",
            answers = new string[] {
                "Gorduras animais.",
                "Óleos vegetais.",
                "Cereais.",
                "Leguminosas."
            },
            correctIndex = 1,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O que é um ácido graxo monoinsaturado?",
            answers = new string[] {
                "Um ácido graxo com uma dupla ligação.",
                "Um ácido graxo com múltiplas ligações duplas.",
                "Um ácido graxo saturado.",
                "Um ácido graxo com um grupo amino."
            },
            correctIndex = 0,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "O que é um ácido graxo poliinsaturado?",
            answers = new string[] {
                "Um ácido graxo saturado.",
                "Um ácido graxo com uma dupla ligação.",
                "Um ácido graxo com múltiplas ligações duplas.",
                "Um ácido graxo com um grupo fosfato."
            },
            correctIndex = 2,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A configuração cis e trans em ácidos graxos se refere a:",
            answers = new string[] {
                "O comprimento da cadeia.",
                "O grau de saturação.",
                "A posição das duplas ligações.",
                "A orientação dos grupamentos ao redor de uma ligação dupla."
            },
            correctIndex = 3,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Ácidos graxos trans são:",
            answers = new string[] {
                "Mais saudáveis que os ácidos graxos cis.",
                "Menos saudáveis que os ácidos graxos cis.",
                "Encontrados principalmente em óleos vegetais.",
                "Encontrados principalmente em gorduras animais."
            },
            correctIndex = 1,
            questionNumber = 48,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "A principal diferença entre manteiga e margarina é:",
            answers = new string[] {
                "O tipo de açúcar presente.",
                "O tipo de proteína presente.",
                "O grau de saturação dos ácidos graxos.",
                "A presença de vitaminas."
            },
            correctIndex = 2,
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "LipidsQuestionDatabase",
            questionText = "Uma dieta rica em lipídios, principalmente triacilgliceróis, é importante para:",
            answers = new string[] {
                "Armazenamento de energia.",
                "Transporte de vitaminas lipossolúveis.",
                "Manutenção da integridade das membranas celulares.",
                "Todas as alternativas anteriores."
            },
            correctIndex = 3,
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
        return QuestionSet.lipids;
    }

    public string GetDatabankName()
    {
        return "LipidsQuestionDatabase";
    }
}