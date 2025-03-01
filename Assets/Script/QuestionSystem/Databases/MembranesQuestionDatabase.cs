using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class MembranesQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question
        {
            questionDatabankName =  "MembranesQuestionDatabase",
            questionText = "Qual o principal componente de uma membrana biológica?",
            answers = new string[] {
                "Carboidratos", 
                "Lipídeos", 
                "Proteínas", 
                "Ácidos Nucleicos"},
            correctIndex = 1,
            questionNumber = 1,
            isImageAnswer = false
       },
        new Question
                {
            questionDatabankName =  "MembranesQuestionDatabase",
            questionText = "Quais os três principais tipos de lipídeos encontrados em membranas biológicas?",
            answers = new string[] {
                "Triacilgliceróis, fosfolipídeos, esfingolipídeos", 
                "Glicerofosfolipídeos, esfingolipídeos, esteroides", 
                "Ácidos graxos, colesterol, triglicerídeos", 
                "Ceras, esteroides, glicerofosfolipídeos"},
            correctIndex = 1,
            questionNumber = 2,
            isImageAnswer = false
       },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "O que significa anfipático?",
            answers = new string[] { 
                "Apresentar regiões polares e apolares", 
                "Apenas polar", 
                "Apenas apolar", 
                "Solúvel apenas em água"},
            correctIndex = 0,
            questionNumber = 3,
            isImageAnswer = false
       },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a principal função da membrana plasmática?",
            answers = new string[] { 
                "Produção de energia", 
                "Síntese de proteínas", 
                "Manutenção do ambiente celular", 
                "Remoção de resíduos"},
            correctIndex = 2,
            questionNumber = 4,
            isImageAnswer = false
       },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Que tipo de ligação une os ácidos graxos ao glicerol nos glicerofosfolipídeos?",
            answers = new string[] { 
                "Ligação peptídica", 
                "Ligação glicosídica", 
                "Ligação éster", 
                "Ligação fosfodiéster"},
            correctIndex = 2,
            questionNumber = 5,
            isImageAnswer = false
       },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual destes NÃO é um componente dos glicerofosfolipídeos?",
            answers = new string[] { 
                "Ácidos graxos", 
                "Glicerol", 
                "Esfingosina", 
                "Fosfato"},
            correctIndex = 2,
            questionNumber = 6,
            isImageAnswer = false
       },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a função do grupo de cabeça polar em um fosfolipídeo?",
            answers = new string[] { 
                "Interage com a água", 
                "Forma a cauda hidrofóbica", 
                "Fornece rigidez estrutural", 
                "Armazena energia"},
            correctIndex = 0,
            questionNumber = 7,
            isImageAnswer = false
       },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Que tipo de ligação une o grupo fosfato ao glicerol em um glicerofosfolipídeo?",
            answers = new string[] { 
                "Ligação peptídica", 
                "Ligação glicosídica",
                 "Ligação éster", 
                 "Ligação fosfodiéster" },
            correctIndex = 3,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o nome do glicerofosfolipídeo mais comum?",
            answers = new string[] { 
                "Esfingomielina", 
                "Fosfatidilcolina", 
                "Cardiolipina", 
                "Cerebrosídeo" },
            correctIndex = 1,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a função da cardiolipina?",
            answers = new string[] { 
                "Isolamento", 
                "Armazenamento de energia", 
                "Encontrada principalmente em membranas mitocondriais", 
                "Sinalização celular" },
            correctIndex = 2,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel dos esfingolipídeos nas membranas celulares?",
            answers = new string[] { 
                "Armazenamento de energia", 
                "Sinalização e reconhecimento celular", 
                "Suporte estrutural", 
                "Catálise" },
            correctIndex = 1,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a unidade estrutural básica dos esfingolipídeos?",
            answers = new string[] { 
                "Glicerol", 
                "Esfingosina", 
                "Ácido graxo", 
                "Fosfato" },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Do que a ceramida é composta?",
            answers = new string[] { 
                "Esfingosina e um ácido graxo", 
                "Glicerol e ácidos graxos", 
                "Esfingosina e fosfato", 
                "Glicerol e esfingosina" },
            correctIndex = 0,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a principal diferença entre cerebrosídeos e globosídeos?",
            answers = new string[] { 
                "Número de ácidos graxos", 
                "Presença de fosfato", 
                "Número de moléculas de açúcar", 
                "Tipo de esfingosina" },
            correctIndex = 2,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "O que são gangliosídeos?",
            answers = new string[] { 
                "Esfingolipídeos simples", 
                "Esfingolipídeos complexos com oligossacarídeos e ácido siálico", 
                "Glicerofosfolipídeos", 
                "Esteroides" },
            correctIndex = 1,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a estrutura básica dos esteroides?",
            answers = new string[] { 
                "Três anéis de seis carbonos e um anel de cinco carbonos", 
                "Uma longa cadeia de hidrocarbonetos", 
                "Uma estrutura de glicerol", 
                "Um grupo fosfato" },
            correctIndex = 0,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o esteroide mais abundante em células animais?",
            answers = new string[] { 
                "Estrogênio", 
                "Testosterona", 
                "Colesterol", 
                "Cortisol" },
            correctIndex = 2,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a principal função do colesterol nas membranas?",
            answers = new string[] { 
                "Armazenamento de energia", 
                "Transdução de sinal", 
                "Modulação da fluidez da membrana", 
                "Atividade enzimática" },
            correctIndex = 2,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "O que é a monocamada de Langmuir?",
            answers = new string[] { 
                "Uma bicamada lipídica", 
                "Uma única camada de lipídeos na superfície da água", 
                "Uma micela lipídica", 
                "Um tipo de proteína" },
            correctIndex = 1,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "O que Gorter e Grendel concluíram sobre o arranjo dos lipídeos na membrana celular?",
            answers = new string[] { 
                "Os lipídeos formam uma monocamada", 
                "Os lipídeos formam uma bicamada", 
                "Os lipídeos formam micelas", 
                "Os lipídeos estão distribuídos aleatoriamente" },
            correctIndex = 1,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Que técnica Frye e Edidin usaram para estudar a fluidez da membrana?",
            answers = new string[] { 
                "Microscopia eletrônica", 
                "Difração de raios-X", 
                "Fusão celular com marcadores fluorescentes", 
                "Ressonância magnética nuclear" },
            correctIndex = 2,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "O que é o modelo do mosaico fluido?",
            answers = new string[] { 
                "Um modelo da estrutura da membrana celular", 
                "Um modelo de enovelamento de proteínas", 
                "Um modelo de replicação do DNA", 
                "Um modelo de metabolismo de carboidratos" },
            correctIndex = 0,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o principal tipo de movimento dos fosfolipídeos dentro de uma membrana?",
            answers = new string[] { 
                "Flip-flop", 
                "Difusão lateral", 
                "Rotação", 
                "Difusão transversal" },
            correctIndex = 1,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "O que é a temperatura de transição?",
            answers = new string[] { 
                "A temperatura na qual uma bicamada lipídica passa de um estado gel para um estado líquido-cristalino", 
                "O ponto de fusão da água", 
                "O ponto de ebulição da água", 
                "A temperatura na qual as proteínas desnaturam" },
            correctIndex = 0,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Como os ácidos graxos saturados afetam a fluidez da membrana?",
            answers = new string[] { 
                "Aumentam a fluidez", 
                "Diminuem a fluidez", 
                "Não têm efeito", 
                "Aumentam a permeabilidade" },
            correctIndex = 1,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Como os ácidos graxos insaturados afetam a fluidez da membrana?",
            answers = new string[] { 
                "Aumentam a fluidez", 
                "Diminuem a fluidez", 
                "Não têm efeito", 
                "Aumentam a permeabilidade" },
            correctIndex = 0,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Como o colesterol afeta a fluidez da membrana?",
            answers = new string[] { 
                "Aumenta a fluidez em baixas temperaturas e diminui em altas temperaturas", 
                "Diminui a fluidez em todas as temperaturas", 
                "Aumenta a fluidez em todas as temperaturas", 
                "Não tem efeito na fluidez" },
            correctIndex = 0,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel das proteínas na membrana celular?",
            answers = new string[] { 
                "Suporte estrutural", 
                "Transporte", 
                "Receptores", 
                "Todas as alternativas acima" },
            correctIndex = 3,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "O que é uma ligação fosfodiéster?",
            answers = new string[] { 
                "Uma ligação entre dois açúcares", 
                "Uma ligação entre um fosfato e dois álcoois", 
                "Uma ligação entre dois ácidos graxos", 
                "Uma ligação entre um fosfato e o glicerol" },
            correctIndex = 1,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a diferença entre uma micela e uma bicamada?",
            answers = new string[] { 
                "As micelas são esféricas, as bicamadas são planares", 
                "As micelas são polares, as bicamadas são apolares", 
                "As micelas são encontradas no citoplasma, as bicamadas são encontradas na membrana", 
                "As micelas são pequenas, as bicamadas são grandes" },
            correctIndex = 0,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a função da bicamada fosfolipídica?",
            answers = new string[] { 
                "Formar uma barreira seletivamente permeável", 
                "Fornecer energia para a célula", 
                "Armazenar informação genética", 
                "Sintetizar proteínas" },
            correctIndex = 0,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a importância da fluidez da membrana?",
            answers = new string[] { 
                "Permite o movimento e a função das proteínas da membrana", 
                "Permite a sinalização celular", 
                "Mantém a integridade da membrana", 
                "Todas as alternativas acima" },
            correctIndex = 3,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Quais fatores afetam a fluidez da membrana?",
            answers = new string[] { 
                "Temperatura, composição lipídica, teor de colesterol", 
                "pH, pressão, atividade enzimática", 
                "Intensidade de luz, concentração de oxigênio, salinidade", 
                "Todas as alternativas acima" },
            correctIndex = 0,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel dos glicolipídeos na membrana celular?",
            answers = new string[] { 
                "Reconhecimento celular", 
                "Armazenamento de energia", 
                "Suporte estrutural", 
                "Atividade enzimática" },
            correctIndex = 0,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a diferença entre um cerebrosídeo e um gangliosídeo?",
            answers = new string[] { 
                "Os cerebrosídeos têm um açúcar, os gangliosídeos têm múltiplos açúcares", 
                "Os cerebrosídeos são esfingolipídeos, os gangliosídeos são glicerofosfolipídeos", 
                "Os cerebrosídeos são encontrados em plantas, os gangliosídeos são encontrados em animais", 
                "Os cerebrosídeos são polares, os gangliosídeos são apolares" },
            correctIndex = 0,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel do ácido siálico nos gangliosídeos?",
            answers = new string[] { 
                "Confere carga negativa", 
                "Fornece suporte estrutural", 
                "Atua como receptor", 
                "Armazena energia" },
            correctIndex = 0,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a função do movimento flip-flop dos fosfolipídeos?",
            answers = new string[] { 
                "Manter a assimetria da membrana", 
                "Facilitar o transporte de proteínas", 
                "Regular a fluidez da membrana", 
                "Armazenar energia" },
            correctIndex = 0,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o significado do experimento conduzido por Frye e Edidin?",
            answers = new string[] { 
                "Demonstrou a fluidez da membrana", 
                "Confirmou a estrutura da bicamada lipídica", 
                "Identificou as proteínas da membrana", 
                "Mediu a espessura da membrana" },
            correctIndex = 0,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel dos marcadores fluorescentes no experimento de Frye e Edidin?",
            answers = new string[] { 
                "Monitorar o movimento das proteínas", 
                "Medir a espessura da membrana", 
                "Visualizar a composição lipídica", 
                "Identificar lipídeos específicos" },
            correctIndex = 0,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a importância da composição lipídica na manutenção da fluidez da membrana?",
            answers = new string[] { 
                "Ácidos graxos saturados diminuem a fluidez, ácidos graxos insaturados aumentam a fluidez", 
                "Ácidos graxos saturados aumentam a fluidez, ácidos graxos insaturados diminuem a fluidez", 
                "A composição lipídica não tem efeito na fluidez", 
                "O comprimento das caudas dos ácidos graxos determina a fluidez" },
            correctIndex = 0,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o efeito da temperatura na fluidez da membrana?",
            answers = new string[] { 
                "Temperaturas mais altas aumentam a fluidez, temperaturas mais baixas diminuem a fluidez", 
                "Temperaturas mais altas diminuem a fluidez, temperaturas mais baixas aumentam a fluidez", 
                "A temperatura não tem efeito na fluidez", 
                "A fluidez é afetada apenas pela composição lipídica" },
            correctIndex = 0,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel do grupo hidroxila no colesterol?",
            answers = new string[] { 
                "Forma o grupo de cabeça polar", 
                "Forma parte do núcleo esteroide rígido", 
                "Interage com os ácidos graxos", 
                "Todas as alternativas acima" },
            correctIndex = 0,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Como o colesterol afeta a permeabilidade da membrana?",
            answers = new string[] { 
                "Aumenta a permeabilidade", 
                "Diminui a permeabilidade", 
                "Não tem efeito", 
                "O efeito depende da temperatura" },
            correctIndex = 1,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o significado do modelo do mosaico fluido da membrana?",
            answers = new string[] { 
                "Explica a fluidez da membrana e a mobilidade das proteínas", 
                "Descreve a interação entre lipídeos e proteínas", 
                "Fornece uma estrutura para entender a função da membrana", 
                "Todas as alternativas acima" },
            correctIndex = 3,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel dos componentes de carboidratos na membrana celular?",
            answers = new string[] { 
                "Sinalização e reconhecimento celular", 
                "Suporte estrutural", 
                "Respostas imunológicas", 
                "Todas as alternativas acima" },
            correctIndex = 3,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a diferença entre proteínas integrais e periféricas da membrana?",
            answers = new string[] { 
                "As proteínas integrais atravessam a membrana, as proteínas periféricas estão frouxamente associadas", 
                "As proteínas integrais são hidrofílicas, as proteínas periféricas são hidrofóbicas", 
                "As proteínas integrais são encontradas no citoplasma, as proteínas periféricas são encontradas na superfície", 
                "As proteínas integrais são enzimas, as proteínas periféricas são estruturais" },
            correctIndex = 0,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel do citoesqueleto na manutenção da estrutura da membrana?",
            answers = new string[] { 
                "Fornece suporte estrutural", 
                "Regula a fluidez da membrana", 
                "Influencia a localização das proteínas", 
                "Todas as alternativas acima" },
            correctIndex = 3,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual a importância da assimetria da membrana?",
            answers = new string[] { 
                "Funções diferentes em cada lado da membrana", 
                "Compartimentalização dos processos celulares", 
                "Regulação da sinalização celular", 
                "Todas as alternativas acima" },
            correctIndex = 3,
            questionNumber = 48,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Qual o papel das proteínas de transporte na membrana?",
            answers = new string[] { 
                "Facilitam o movimento de moléculas através da membrana", 
                "Regulam a concentração de íons", 
                "Mantêm a homeostase celular", 
                "Todas as alternativas acima" },
            correctIndex = 3,
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "MembranesQuestionDatabase",
            questionText = "Quais algumas doenças associadas a defeitos no metabolismo lipídico?",
            answers = new string[] { 
                "Doença de Tay-Sachs, doença de Niemann-Pick", 
                "Doença de Alzheimer, doença de Parkinson", 
                "Fibrose cística, doença de Huntington", 
                "Todas as alternativas acima" },
            correctIndex = 0,
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
        return QuestionSet.membranes;
    }

    public string GetDatabankName()
    {
        return "MembranesQuestionDatabase";
    }
}