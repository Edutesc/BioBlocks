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
            questionText = "Qual o monossacarídeo mais importante como fonte de energia?",
            answers = new string[] {"Frutose", "Galactose", "Glicose", "Ribose"},
            correctIndex = 2,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são isômeros?",
            answers = new string[] {"Moléculas com a mesma fórmula molecular, mas estruturas diferentes", "Moléculas com a mesma função biológica", "Moléculas com o mesmo peso molecular", "Moléculas com o mesmo ponto de fusão"},
            correctIndex = 0,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que é um carbono assimétrico?",
            answers = new string[] {"Um carbono ligado a quatro grupos diferentes", "Um carbono com dupla ligação", "Um carbono com ligação tripla", "Um carbono terminal"},
            correctIndex = 0,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são epímeros?",
            answers = new string[] {"Diastereoisômeros que diferem em um único centro quiral", "Enanciômeros que diferem em todos os centros quirais", "Isômeros que diferem no número de átomos de carbono", "Isômeros que diferem no tipo de ligação"},
            correctIndex = 0,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são anômeros?",
            answers = new string[] {"Isômeros cíclicos que diferem na configuração do carbono anomérico", "Isômeros de cadeia aberta", "Isômeros que diferem na posição do grupo hidroxila", "Isômeros que diferem no tipo de ligação"},
            correctIndex = 0,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o principal dissacarídeo encontrado na cana-de-açúcar?",
            answers = new string[] {"Maltose", "Lactose", "Sacarose", "Celobiose"},
            correctIndex = 2,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual dissacarídeo é o açúcar do leite?",
            answers = new string[] {"Maltose", "Lactose", "Sacarose", "Celobiose"},
            correctIndex = 1,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o tipo de ligação que une os monossacarídeos em um dissacarídeo?",
            answers = new string[] {"Ligação peptídica", "Ligação glicosídica", "Ligação éster", "Ligação fosfodiéster"},
            correctIndex = 1,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o principal polissacarídeo de reserva energética em plantas?",
            answers = new string[] {"Celulose", "Quitina", "Amido", "Glicogênio"},
            correctIndex = 2,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o principal polissacarídeo de reserva energética em animais?",
            answers = new string[] {"Celulose", "Quitina", "Amido", "Glicogênio"},
            correctIndex = 3,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual polissacarídeo forma a parede celular das plantas?",
            answers = new string[] {"Amido", "Glicogênio", "Celulose", "Quitina"},
            correctIndex = 2,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual polissacarídeo forma o exoesqueleto de insetos?",
            answers = new string[] {"Amido", "Glicogênio", "Celulose", "Quitina"},
            correctIndex = 3,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que é uma ligação glicosídica?",
            answers = new string[] {"Uma ligação covalente entre dois monossacarídeos", "Uma ligação iônica entre dois monossacarídeos", "Uma ligação de hidrogênio entre dois monossacarídeos", "Uma ligação peptídica entre dois monossacarídeos"},
            correctIndex = 0,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a principal diferença estrutural entre amilose e amilopectina?",
            answers = new string[] {"Ramificação", "Tipo de ligação glicosídica", "Tipo de monossacarídeo", "Peso molecular"},
            correctIndex = 0,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o tipo de ligação glicosídica predominante na celulose?",
            answers = new string[] {"alfa(1 -> 4)", "alfa(1 -> 6)", "beta(1 -> 4)", "beta(1 -> 6)"},
            correctIndex = 2,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o tipo de ligação glicosídica predominante no amido?",
            answers = new string[] {"alfa(1 -> 4)", "alfa(1 -> 6)", "beta(1 -> 4)", "beta(1 -> 6)"},
            correctIndex = 0,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual enzima hidrolisa o amido?",
            answers = new string[] {"Celulase", "Quitina", "Amilase", "Lactase"},
            correctIndex = 2,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual enzima hidrolisa a celulose?",
            answers = new string[] {"Celulase", "Quitina", "Amilase", "Lactase"},
            correctIndex = 0,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a importância biológica dos oligossacarídeos?",
            answers = new string[] {"Reconhecimento celular", "Sinalização celular", "Componentes de glicoproteínas e glicolipídeos", "Todas as alternativas acima"},
            correctIndex = 3,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são glicoproteínas?",
            answers = new string[] {"Proteínas ligadas a carboidratos", "Proteínas ligadas a lipídeos", "Proteínas ligadas a ácidos nucleicos", "Proteínas ligadas a outras proteínas"},
            correctIndex = 0,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são proteoglicanas?",
            answers = new string[] {"Glicosaminoglicanos ligados a proteínas", "Glicoproteínas ligadas a lipídeos", "Glicolipídeos ligados a proteínas", "Proteínas ligadas a ácidos nucleicos"},
            correctIndex = 0,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Onde são encontradas as glicosaminoglicanas?",
            answers = new string[] {"Matriz extracelular", "Membrana celular", "Citoplasma", "Todas as alternativas acima"},
            correctIndex = 0,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função do ácido hialurônico?",
            answers = new string[] {"Lubrificação", "Suporte estrutural", "Viscosidade", "Todas as alternativas acima"},
            correctIndex = 3,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função da heparina?",
            answers = new string[] {"Anticoagulante", "Lubrificante", "Suporte estrutural", "Hormônio"},
            correctIndex = 0,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função da condroitina?",
            answers = new string[] {"Suporte estrutural em cartilagens", "Lubrificação", "Viscosidade", "Todas as alternativas acima"},
            correctIndex = 3,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função da queratana?",
            answers = new string[] {"Suporte estrutural em córneas", "Lubrificação", "Viscosidade", "Todas as alternativas acima"},
            correctIndex = 3,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função do dermatana?",
            answers = new string[] {"Suporte estrutural na pele", "Lubrificação", "Viscosidade", "Todas as alternativas acima"},
            correctIndex = 3,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são lectinas?",
            answers = new string[] {"Proteínas que se ligam a carboidratos", "Proteínas que se ligam a lipídeos", "Proteínas que se ligam a ácidos nucleicos", "Proteínas que se ligam a outras proteínas"},
            correctIndex = 0,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que são selectinas?",
            answers = new string[] {"Proteínas que medeiam a adesão celular", "Proteínas que se ligam a carboidratos", "Proteínas que se ligam a lipídeos", "Proteínas que se ligam a ácidos nucleicos"},
            correctIndex = 0,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual o papel do ácido siálico nos eritrócitos?",
            answers = new string[] {"Marcar a idade das células", "Manter a forma das células", "Prevenir a degradação das células", "Todas as alternativas acima"},
            correctIndex = 2,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Como a ligação glicosídica é formada?",
            answers = new string[] {"Reação de condensação entre dois monossacarídeos", "Reação de hidrólise entre dois monossacarídeos", "Ligação iônica entre dois monossacarídeos", "Ligação peptídica entre dois monossacarídeos"},
            correctIndex = 0,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Quais são os produtos da hidrólise da sacarose?",
            answers = new string[] {"Glicose e frutose", "Glicose e galactose", "Frutose e galactose", "Glicose e glicose"},
            correctIndex = 0,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Quais são os produtos da hidrólise da lactose?",
            answers = new string[] {"Glicose e frutose", "Glicose e galactose", "Frutose e galactose", "Glicose e glicose"},
            correctIndex = 1,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Quais são os produtos da hidrólise da maltose?",
            answers = new string[] {"Glicose e frutose", "Glicose e galactose", "Frutose e galactose", "Glicose e glicose"},
            correctIndex = 3,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que é uma mistura racêmica?",
            answers = new string[] {"Uma mistura de isômeros D e L em quantidades iguais", "Uma mistura de isômeros D e L em quantidades diferentes", "Uma mistura de diastereoisômeros", "Uma mistura de enanciômeros"},
            correctIndex = 0,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "O que é mutarrotação?",
            answers = new string[] {"A interconversão entre anômeros alfa e beta", "A conversão de um açúcar em outro", "A hidrólise de um dissacarídeo", "A oxidação de um açúcar"},
            correctIndex = 0,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a diferença entre um homopolissacarídeo e um heteropolissacarídeo?",
            answers = new string[] {"Um homopolissacarídeo contém apenas um tipo de monossacarídeo, um heteropolissacarídeo contém múltiplos tipos", "Um homopolissacarídeo é linear, um heteropolissacarídeo é ramificado", "Um homopolissacarídeo é solúvel em água, um heteropolissacarídeo é insolúvel", "Um homopolissacarídeo é um polímero, um heteropolissacarídeo é um monômero"},
            correctIndex = 0,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Dê exemplos de homopolissacarídeos.",
            answers = new string[] {"Amido, glicogênio, celulose, quitina", "Sacarose, lactose, maltose", "Glicose, frutose, galactose", "Todos os polissacarídeos são homopolissacarídeos"},
            correctIndex = 0,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Dê exemplos de heteropolissacarídeos.",
            answers = new string[] {"Peptídeoglicano, glicosaminoglicanos", "Amido, glicogênio, celulose", "Sacarose, lactose, maltose", "Glicose, frutose, galactose"},
            correctIndex = 0,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função do peptídeoglicano?",
            answers = new string[] {"Forma a parede celular das bactérias", "Reserva de energia em plantas", "Armazenamento de energia em animais", "Formação do exoesqueleto de insetos"},
            correctIndex = 0,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual enzima hidrolisa o peptídeoglicano?",
            answers = new string[] {"Lisozima", "Amilase", "Celulase", "Lactase"},
            correctIndex = 0,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a importância da lisozima?",
            answers = new string[] {"Antibacteriana", "Anticoagulante", "Lubrificante", "Enzimática"},
            correctIndex = 0,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Onde é encontrada a inulina?",
            answers = new string[] {"Bulbos de plantas", "Parede celular de plantas", "Matriz extracelular", "Fígado de animais"},
            correctIndex = 0,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Onde é encontrada a pectina?",
            answers = new string[] {"Parede celular de plantas", "Frutas cítricas", "Matriz extracelular", "Todas as alternativas acima"},
            correctIndex = 3,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Onde é encontrado o agar?",
            answers = new string[] {"Algas", "Parede celular de plantas", "Matriz extracelular", "Fígado de animais"},
            correctIndex = 0,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a função da quitina?",
            answers = new string[] {"Suporte estrutural em animais", "Reserva de energia em plantas", "Armazenamento de energia em animais", "Formação do exoesqueleto de insetos"},
            correctIndex = 0,
            questionNumber = 48,
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
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "CarbohydratesQuestionDatabase",
            questionText = "Qual a importância das pontes de hidrogênio na estrutura da celulose?",
            answers = new string[] {
                "As pontes de hidrogênio não afetam a estrutura da celulose.",
                "As pontes de hidrogênio conferem flexibilidade à celulose.",
                "As pontes de hidrogênio conferem rigidez e força à celulose.",
                "As pontes de hidrogênio tornam a celulose solúvel em água."},
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
        return QuestionSet.carbohydrates;
    }

    public string GetDatabankName()
    {
        return "CarbohydratesQuestionDatabase";
    }
}