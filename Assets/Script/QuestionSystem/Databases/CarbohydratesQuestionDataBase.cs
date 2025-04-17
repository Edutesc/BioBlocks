
using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class CarbohydratesQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a fórmula geral dos monossacarídeos?",
            answers = new string[] {
                "(CH<sub>2</sub> O)<sub>n</sub>",
                "C<sub>n</sub> H<sub>2n</sub> O<sub>n</sub>",
                "C<sub>n</sub> H<sub>2n-2</sub> O<sub>n</sub>",
                "C<sub>n</sub> H<sub>2n+2</sub> O<sub>n</sub>"
            },
            correctIndex = 0,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que diferencia uma aldose de uma cetose?",
            answers = new string[] {"Grupo funcional aldeído vs. cetona", "Número de átomos de carbono", "Presença de oxigênio", "Solubilidade em água"},
            correctIndex = 0,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual das estruturas representa uma cetose?",
            answers = new string[] {
                "AnswerImages/SugarDB/aldotriose",
                "AnswerImages/SugarDB/aldotetrose",
                "AnswerImages/SugarDB/cetotetrose",
                "AnswerImages/SugarDB/aldopentose"
            },
            correctIndex = 2,
            questionNumber = 3,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual estrutura representa uma aldotetrose",
            answers = new string[] {
                "AnswerImages/SugarDB/aldotriose",
                "AnswerImages/SugarDB/aldotetrose",
                "AnswerImages/SugarDB/cetotetrose",
                "AnswerImages/SugarDB/aldopentose"
            },
            correctIndex = 1,
            questionNumber = 4,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
             answers = new string[] {
                "AnswerImages/SugarDB/l_glicose",
                "AnswerImages/SugarDB/d_glicose",
                "AnswerImages/SugarDB/d_galactose",
                "AnswerImages/SugarDB/d_frutose"
            },
            correctIndex = 0,
            questionNumber = 5,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_5"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são epímeros?",
            answers = new string[] {
                "Diastereoisômeros que diferem em um único centro quiral", 
                "Enanciômeros que diferem em todos os centros quirais", 
                "Isômeros que diferem no número de átomos de carbono", 
                "Isômeros que diferem no tipo de ligação"},
            correctIndex = 0,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
             answers = new string[] {
                "AnswerImages/SugarDB/l_glicose",
                "AnswerImages/SugarDB/d_frutose",
                "AnswerImages/SugarDB/l_frutose",
                "AnswerImages/SugarDB/d_tagatose"
            },
            correctIndex = 3,
            questionNumber = 7,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_7"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são anômeros?",
            answers = new string[] {
                "Isômeros cíclicos que diferem na configuração do carbono anomérico", 
                "Isômeros de cadeia aberta",
                "Isômeros que diferem na posição do grupo hidroxila", 
                "Isômeros que diferem no tipo de ligação"
            },
            correctIndex = 0,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "AnswerImages/SugarDB/l_glicose",
                "AnswerImages/SugarDB/alfa-d-galactopiranose",
                "AnswerImages/SugarDB/beta-d-n-galactosamina",
                "AnswerImages/SugarDB/alfa_d_glicopiranose"
            },
            correctIndex = 3,
            questionNumber = 9,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_9"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o principal dissacarídeo encontrado na cana-de-açúcar?",
            answers = new string[] {"Maltose", "Lactose", "Sacarose", "Celobiose"},
            correctIndex = 2,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual dissacarídeo é o açúcar do leite?",
            answers = new string[] {"Maltose", "Lactose", "Sacarose", "Celobiose"},
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o tipo de ligação que une os monossacarídeos em um dissacarídeo?",
            answers = new string[] {"Ligação peptídica", "Ligação glicosídica", "Ligação éster", "Ligação fosfodiéster"},
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "monossacarídeo",
                "dissacarídeo",
                "oligossacarídeo",
                "polissacarídeo"
            },
            correctIndex = 1,
            questionNumber = 13,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_13"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "1-beta-4",
                "1-alfa-4",
                "1-beta-2",
                "1-alfa-2"
            },
            correctIndex = 0,
            questionNumber = 14,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_14"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o principal polissacarídeo de reserva energética em plantas?",
            answers = new string[] {"Celulose", "Quitina", "Amido", "Glicogênio"},
            correctIndex = 2,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o principal polissacarídeo de reserva energética em animais?",
            answers = new string[] {"Celulose", "Quitina", "Amido", "Glicogênio"},
            correctIndex = 3,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a principal diferença estrutural entre amilose e amilopectina?",
            answers = new string[] {"Ramificação", "Tipo de ligação glicosídica", "Tipo de monossacarídeo", "Peso molecular"},
            correctIndex = 0,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o tipo de ligação glicosídica predominante na celulose?",
            answers = new string[] {"alfa(1 -> 4)", "alfa(1 -> 6)", "beta(1 -> 4)", "beta(1 -> 6)"},
            correctIndex = 2,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o tipo de ligação glicosídica predominante no amido?",
            answers = new string[] {"alfa(1 -> 4)", "alfa(1 -> 6)", "beta(1 -> 4)", "beta(1 -> 6)"},
            correctIndex = 0,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual enzima hidrolisa o amido?",
            answers = new string[] {"Celulase", "Quitina", "Amilase", "Lactase"},
            correctIndex = 2,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual enzima hidrolisa a celulose?",
            answers = new string[] {"Celulase", "Quitina", "Amilase", "Lactase"},
            correctIndex = 0,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são glicoproteínas?",
            answers = new string[] {
                "Proteínas ligadas a carboidratos", 
                "Proteínas ligadas a lipídeos", 
                "Proteínas ligadas a ácidos nucleicos", 
                "Proteínas ligadas a outras proteínas"
            },
            correctIndex = 0,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são glicosaminoglicanos?",
            answers = new string[] {
                "Homopolissacarídeos que presentes no citoplasma celular.", 
                "Heteropolissacarídeos que compõe a matriz extracelular.", 
                "São glicoses ligadas a aminas presentes no sangue", 
                "São dissacarídeos ligados à proteínas"
            },
            correctIndex = 1,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são proteoglicanos?",
            answers = new string[] {
                "Glicosaminoglicanos ligados a proteínas", 
                "Glicoproteínas ligadas a lipídeos", 
                "Glicolipídeos ligados a proteínas", 
                "Proteínas ligadas a ácidos nucleicos"
            },
            correctIndex = 0,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Onde são encontradas os glicosaminoglicanos?",
            answers = new string[] {"Matriz extracelular", "Membrana celular", "Citoplasma", "Todas as alternativas acima"},
            correctIndex = 0,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Identifique abaixo um componente estrutural dos glicosaminoglicanos.",
            answers = new string[] {
                "AnswerImages/SugarDB/alfa_d_glicopiranose",
                "AnswerImages/SugarDB/beta_d_glicopiranose",
                "AnswerImages/SugarDB/beta-d-n-galactosamina",
                "AnswerImages/SugarDB/aldopentose"
            },
            correctIndex = 2,
            questionNumber = 26,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Indique abaixo a estrutura do ácido hialurônico, que é um glicosaminoglicano cujo monômero é formado pelo ácido glicurônico e pelo ácido N-acetil-glicosamina.",
            answers = new string[] {
                "AnswerImages/SugarDB/dermatan",
                "AnswerImages/SugarDB/condroitin",
                "AnswerImages/SugarDB/queratan",
                "AnswerImages/SugarDB/hialuronico"
            },
            correctIndex = 3,
            questionNumber = 27,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função do ácido hialurônico?",
            answers = new string[] {"Lubrificação", "Suporte estrutural", "Viscosidade", "Todas as alternativas acima"},
            correctIndex = 3,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "A heparina é um glicosaminoglicano com uma das maiores densidades de carga negativa dentre todas as biomoléculas dos organismos vivos. Indique abaixo o monômero que forma a heparina.",
            answers = new string[] {
                "AnswerImages/SugarDB/dermatan",
                "AnswerImages/SugarDB/condroitin",
                "AnswerImages/SugarDB/heparina",
                "AnswerImages/SugarDB/hialuronico"
            },
            correctIndex = 2,
            questionNumber = 29,
            isImageAnswer = true
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual é uma das principais função da heparina?",
            answers = new string[] {"Anticoagulante", "Lubrificante", "Suporte estrutural", "Hormônio"},
            correctIndex = 0,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "1-beta-4",
                "1-alfa-4",
                "1-beta-3",
                "1-alfa-3"
            },
            correctIndex = 2,
            questionNumber = 31,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_31"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a principal função do condroitin?",
            answers = new string[] {"Suporte estrutural em cartilagens", "hormônio", "Neurotransmissor", "Todas as alternativas acima"},
            correctIndex = 0,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "1-beta-4",
                "1-alfa-4",
                "1-beta-3",
                "1-alfa-3"
            },
            correctIndex = 2,
            questionNumber = 33,
            isImageQuestion = true,
            isImageAnswer = false,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_33"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
             answers = new string[] {
                "AnswerImages/SugarDB/l_glicose",
                "AnswerImages/SugarDB/d_frutose",
                "AnswerImages/SugarDB/alfa-d-galactopiranose",
                "AnswerImages/SugarDB/d_galactose"
            },
            correctIndex = 1,
            questionNumber = 34,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_34"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
             answers = new string[] {
                "AnswerImages/SugarDB/l_glicose",
                "AnswerImages/SugarDB/d_frutose",
                "AnswerImages/SugarDB/d_tagatose",
                "AnswerImages/SugarDB/d_glicose"
            },
            correctIndex = 3,
            questionNumber = 35,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_35"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "AnswerImages/SugarDB/beta_d_galactopiranose",
                "AnswerImages/SugarDB/alfa-d-galactopiranose",
                "AnswerImages/SugarDB/beta_d_glicopiranose",
                "AnswerImages/SugarDB/alfa_d_glicopiranose"
            },
            correctIndex = 2,
            questionNumber = 36,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_36"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Identifique abaixo a estrutura da glicose, que também pode ser nomeada como beta-D-glicopiranose",
            answers = new string[] {
                "AnswerImages/SugarDB/beta_d_galactopiranose",
                "AnswerImages/SugarDB/alfa-d-galactopiranose",
                "AnswerImages/SugarDB/alfa_d_glicopiranose",
                "AnswerImages/SugarDB/beta_d_glicopiranose"
            },
            correctIndex = 3,
            questionNumber = 37,
            isImageQuestion = false,
            isImageAnswer = true,
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "AnswerImages/SugarDB/beta_d_galactopiranose",
                "AnswerImages/SugarDB/alfa-d-galactopiranose",
                "AnswerImages/SugarDB/alfa_d_glicopiranose",
                "AnswerImages/SugarDB/beta_d_glicopiranose"
            },
            correctIndex = 2,
            questionNumber = 38,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_38"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a diferença entre um homopolissacarídeo e um heteropolissacarídeo?",
            answers = new string[] {
                "Um homopolissacarídeo contém apenas um tipo de monossacarídeo, um heteropolissacarídeo contém múltiplos tipos", 
                "Um homopolissacarídeo é linear, um heteropolissacarídeo é ramificado", 
                "Um homopolissacarídeo é solúvel em água, um heteropolissacarídeo é insolúvel", 
                "Um homopolissacarídeo é um polímero, um heteropolissacarídeo é um monômero"},
            correctIndex = 0,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Dê exemplos de homopolissacarídeos.",
            answers = new string[] {
                "Amido, glicogênio, celulose", 
                "Sacarose, lactose, maltose", 
                "Glicose, frutose, galactose", 
                "Todos os polissacarídeos são homopolissacarídeos"},
            correctIndex = 0,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Dê exemplos de heteropolissacarídeos.",
            answers = new string[] { 
                "Amido, glicogênio, celulose", 
                "Sacarose, lactose, maltose", 
                "Peptídeoglicano, glicosaminoglicanos",
                "Glicose, frutose, galactose"},
            correctIndex = 2,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a principal diferença entre a estrutura da amilose e da celulose?",
            answers = new string[] {
                "A amilose é ramificada, a celulose é linear.",
                "A amilose possui ligações alfa(1 -> 4), a celulose possui ligações beta(1 -> 4).",
                "A amilose é um polímero de frutose, a celulose é um polímero de glicose.",
                "A amilose é insolúvel em água, a celulose é solúvel em água."},
            correctIndex = 1,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/leucina",
                "AnswerImages/AminoacidsDB/prolina",
                "AnswerImages/AminoacidsDB/triptofano",
                "AnswerImages/AminoacidsDB/asparagina"
            },
            correctIndex = 3,
            questionNumber = 43,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_43"
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            answers = new string[] {
                "AnswerImages/AminoacidsDB/leucina",
                "AnswerImages/AminoacidsDB/serina",
                "AnswerImages/AminoacidsDB/triptofano",
                "AnswerImages/AminoacidsDB/asparagina"
            },
            correctIndex = 1,
            questionNumber = 44,
            isImageQuestion = true,
            isImageAnswer = true,
            questionImagePath =  "AnswerImages/SugarDB/sugar_question_44"
        },
    };

    public List<Question> GetQuestions()
    {
        return questions;
    }

    public QuestionSet GetQuestionSetType()
    {
        return QuestionSet.carbohydrates;
    }

    public string GetDatabankName()
    {
        return "CarbohydratesQuestionDatabase";
    }
}
