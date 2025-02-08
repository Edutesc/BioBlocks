using UnityEngine;
using System.Collections.Generic;
using QuestionSystem;

public class ProteinQuestionDatabase : MonoBehaviour, IQuestionDatabase
{
    private List<Question> questions = new List<Question>
    {
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Qual o nível estrutural de uma proteína que corresponde à sequência linear de aminoácidos?",
            answers = new string[] {
                "Estrutura secundária",
                "Estrutura terciária",
                "Estrutura quaternária",
                "Estrutura primária"
            },
            correctIndex = 3,
            questionNumber = 1,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Que tipo de ligação une os aminoácidos na estrutura primária de uma proteína?",
            answers = new string[] {
                "Ligações de hidrogênio",
                "Ligações iônicas",
                "Ligações peptídicas",
                "Pontes dissulfeto"
            },
            correctIndex = 2,
            questionNumber = 2,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A formação de uma ligação peptídica envolve:",
            answers = new string[] {
                "A união de dois aminoácidos com a perda de uma molécula de água.",
                "A união de dois aminoácidos com a adição de uma molécula de água.",
                "A união de três aminoácidos com a perda de uma molécula de água.",
                "A união de três aminoácidos com a adição de uma molécula de água."
            },
            correctIndex = 0,
            questionNumber = 3,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Pontes dissulfeto são formadas entre resíduos de:",
            answers = new string[] {
                "Alanina",
                "Glicina",
                "Cisteína",
                "Prolina"
            },
            correctIndex = 2,
            questionNumber = 4,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Que tipo de estrutura protéica é formada pela união de duas ou mais cadeias polipeptídicas?",
            answers = new string[] {
                "Primária",
                "Secundária",
                "Terciária",
                "Quaternária"
            },
            correctIndex = 3,
            questionNumber = 5,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A estrutura secundária de uma proteína é estabilizada principalmente por:",
            answers = new string[] {
                "Pontes dissulfeto",
                "Ligações peptídicas",
                "Ligações de hidrogênio",
                "Interações hidrofóbicas"
            },
            correctIndex = 2,
            questionNumber = 6,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Quais são os principais elementos de estrutura secundária?",
            answers = new string[] {
                "Alfa-hélices e folhas beta",
                "Alfa-hélices e pontes dissulfeto",
                "Folhas beta e ligações peptídicas",
                "Voltas e ligações iônicas"
            },
            correctIndex = 0,
            questionNumber = 7,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Em uma alfa-hélice, os grupamentos R dos aminoácidos ficam:",
            answers = new string[] {
                "No interior da hélice.",
                "No exterior da hélice.",
                "Aleatoriamente distribuídos.",
                "Em um plano específico."
            },
            correctIndex = 1,
            questionNumber = 8,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "As folhas beta são estabilizadas por:",
            answers = new string[] {
                "Interações hidrofóbicas",
                "Pontes de hidrogênio",
                "Ligações iônicas",
                "Pontes dissulfeto"
            },
            correctIndex = 1,
            questionNumber = 9,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Em folhas beta, as cadeias polipeptídicas podem ser:",
            answers = new string[] {
                "Apenas paralelas.",
                "Apenas antiparalelas.",
                "Paralelas ou antiparalelas.",
                "Não são encontradas em folhas beta."
            },
            correctIndex = 2,
            questionNumber = 10,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "As voltas (dobras) em proteínas conectam:",
            answers = new string[] {
                "Somente alfa-hélices.",
                "Somente folhas beta.",
                "Alfa-hélices e folhas beta.",
                "Apenas regiões da mesma cadeia polipeptídica."
            },
            correctIndex = 2,
            questionNumber = 11,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A prolina é uma má formadora de alfa-hélices porque:",
            answers = new string[] {
                "É um aminoácido pequeno.",
                "Seu nitrogênio faz parte de um anel.",
                "É um aminoácido apolar.",
                "Possui uma cadeia lateral grande."
            },
            correctIndex = 1,
            questionNumber = 12,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A glicina é uma má formadora de alfa-hélices porque:",
            answers = new string[] {
                "É um aminoácido grande.",
                "É um aminoácido carregado.",
                "Possui alta flexibilidade.",
                "Forma pontes dissulfeto."
            },
            correctIndex = 2,
            questionNumber = 13,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A estrutura terciária de uma proteína se refere a:",
            answers = new string[] {
                "Seqüência de aminoácidos.",
                "Arranjo espacial de alfa-hélices e folhas beta.",
                "Interações entre diferentes cadeias polipeptídicas.",
                "Dobramento tridimensional da proteína."
            },
            correctIndex = 3,
            questionNumber = 14,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Quais forças mantêm a estrutura terciária de uma proteína?",
            answers = new string[] {
                "Apenas ligações peptídicas.",
                "Pontes de hidrogênio, interações hidrofóbicas, interações iônicas, pontes dissulfeto, e forças de van der Waals.",
                "Apenas pontes dissulfeto.",
                "Apenas ligações de hidrogênio."
            },
            correctIndex = 1,
            questionNumber = 15,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Qual técnica utiliza cristais de proteína para determinar sua estrutura?",
            answers = new string[] {
                "Espectrometria de massas",
                "Cristalografia de raios-X",
                "Ressonância magnética nuclear (RMN)",
                "Microscopia eletrônica"
            },
            correctIndex = 1,
            questionNumber = 16,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Qual técnica utiliza proteínas em solução para determinar sua estrutura?",
            answers = new string[] {
                "Cristalografia de raios-X",
                "Espectrometria de massas",
                "Ressonância magnética nuclear (RMN)",
                "Microscopia eletrônica"
            },
            correctIndex = 2,
            questionNumber = 17,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Em geral, aminoácidos apolares são encontrados principalmente:",
            answers = new string[] {
                "Na superfície da proteína.",
                "No interior da proteína.",
                "Em ambos os locais igualmente.",
                "Ligados a grupamentos prostéticos."
            },
            correctIndex = 1,
            questionNumber = 18,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Em geral, aminoácidos polares são encontrados principalmente:",
            answers = new string[] {
                "No interior da proteína.",
                "Na superfície da proteína.",
                "Em ambos os locais igualmente.",
                "Ligados a grupamentos prostéticos."
            },
            correctIndex = 1,
            questionNumber = 19,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O enovelamento protéico é o processo de:",
            answers = new string[] {
                "Formação da estrutura primária de uma proteína.",
                "Formação da estrutura secundária de uma proteína.",
                "Formação da estrutura terciária de uma proteína.",
                "Degradação de uma proteína."
            },
            correctIndex = 2,
            questionNumber = 20,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O experimento de Anfinsen demonstrou que:",
            answers = new string[] {
                "As proteínas se enovelam com a ajuda de chaperonas.",
                "A seqüência primária determina a estrutura terciária.",
                "A estrutura terciária é aleatória.",
                "As proteínas não se enovelam espontaneamente."
            },
            correctIndex = 1,
            questionNumber = 21,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Chaperonas moleculares são:",
            answers = new string[] {
                "Proteínas que auxiliam o enovelamento protéico.",
                "Proteínas que degradam outras proteínas.",
                "Moléculas de RNA que auxiliam a síntese protéica.",
                "Moléculas de DNA que auxiliam a síntese protéica."
            },
            correctIndex = 0,
            questionNumber = 22,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "As proteínas de choque térmico atuam principalmente:",
            answers = new string[] {
                "Em baixas temperaturas.",
                "Em altas temperaturas.",
                "Em pHs ácidos.",
                "Em pHs básicos."
            },
            correctIndex = 1,
            questionNumber = 23,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Chaperoninas são:",
            answers = new string[] {
                "Proteínas que atuam em baixas temperaturas.",
                "Proteínas que auxiliam no enovelamento protéico.",
                "Proteínas que degradam proteínas.",
                "Enzimas que quebram ligações peptídicas."
            },
            correctIndex = 1,
            questionNumber = 24,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A dissulfeto isomerase catalisa a formação de:",
            answers = new string[] {
                "Ligações peptídicas",
                "Pontes de hidrogênio",
                "Pontes dissulfeto",
                "Ligações iônicas"
            },
            correctIndex = 2,
            questionNumber = 25,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A peptidil-prolil isomerase atua sobre:",
            answers = new string[] {
                "Ligações peptídicas",
                "Pontes dissulfeto",
                "Isômeros de prolina",
                "Grupamentos carboxila"
            },
            correctIndex = 2,
            questionNumber = 26,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O que são proteínas homólogas?",
            answers = new string[] {
                "Proteínas com a mesma função em diferentes espécies.",
                "Proteínas com estruturas terciárias idênticas.",
                "Proteínas que interagem entre si.",
                "Proteínas que são sintetizadas simultaneamente."
            },
            correctIndex = 0,
            questionNumber = 27,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Proteínas homólogas são usadas para estudos de:",
            answers = new string[] {
                "Metabolismo celular",
                "Filogenia",
                "Estruturas terciárias",
                "Enovelamento protéico"
            },
            correctIndex = 1,
            questionNumber = 28,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Proteínas fibrosas geralmente apresentam:",
            answers = new string[] {
                "Uma forma globular.",
                "Uma forma filamentosa.",
                "Uma estrutura terciária complexa.",
                "Muitos grupamentos prostéticos."
            },
            correctIndex = 1,
            questionNumber = 29,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A alfa-queratina é uma proteína fibrosa encontrada em:",
            answers = new string[] {
                "Ossos",
                "Cabelos, unhas e pêlos.",
                "Músculos",
                "Enzimas"
            },
            correctIndex = 1,
            questionNumber = 30,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A estrutura da alfa-queratina é rica em:",
            answers = new string[] {
                "Folhas beta",
                "Alfa-hélices",
                "Voltas beta",
                "Pontes dissulfeto"
            },
            correctIndex = 1,
            questionNumber = 31,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A alta resistência da alfa-queratina se deve a:",
            answers = new string[] {
                "Sua baixa massa molecular.",
                "Sua alta solubilidade em água.",
                "A formação de super-hélices.",
                "Sua estrutura primária simples."
            },
            correctIndex = 2,
            questionNumber = 32,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O colágeno é uma proteína fibrosa encontrada em:",
            answers = new string[] {
                "Cabelos",
                "Ossos, cartilagens e tendões.",
                "Músculos",
                "Enzimas"
            },
            correctIndex = 1,
            questionNumber = 33,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A estrutura do colágeno é:",
            answers = new string[] {
                "Uma única hélice alfa.",
                "Uma dupla hélice alfa.",
                "Uma tripla hélice.",
                "Uma folha beta."
            },
            correctIndex = 2,
            questionNumber = 34,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Qual aminoácido é mais abundante no colágeno?",
            answers = new string[] {
                "Alanina",
                "Glicina",
                "Prolina",
                "Hidroxiprolina"
            },
            correctIndex = 1,
            questionNumber = 35,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A hidroxiprolina é uma modificação da:",
            answers = new string[] {
                "Alanina",
                "Glicina",
                "Prolina",
                "Serina"
            },
            correctIndex = 2,
            questionNumber = 36,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A fibroína da seda é uma proteína fibrosa cuja estrutura é rica em:",
            answers = new string[] {
                "Alfa-hélices",
                "Folhas beta",
                "Voltas",
                "Pontes dissulfeto"
            },
            correctIndex = 1,
            questionNumber = 37,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A fibroína da seda é um exemplo de proteína com estrutura rica em:",
            answers = new string[] {
                "Alfa-hélices",
                "Folhas beta antiparalelas",
                "Voltas",
                "Pontes dissulfeto"
            },
            correctIndex = 1,
            questionNumber = 38,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A resistência da fibroína da seda se deve principalmente a:",
            answers = new string[] {
                "Sua estrutura helicoidal.",
                "Sua alta massa molecular.",
                "O empacotamento denso das folhas beta.",
                "Sua alta quantidade de cisteína."
            },
            correctIndex = 2,
            questionNumber = 39,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O que causa a mudança conformacional na proteína prion?",
            answers = new string[] {
                "Mutações genéticas",
                "Modificações pós-traducionais",
                "Ação de enzimas",
                "Ainda não se conhece a causa"
            },
            correctIndex = 3,
            questionNumber = 40,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A forma patogênica da proteína prion é chamada de:",
            answers = new string[] {
                "PrPC",
                "PrPSC",
                "PrP",
                "APP"
            },
            correctIndex = 1,
            questionNumber = 41,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A Doença da Vaca Louca é causada por:",
            answers = new string[] {
                "Um vírus",
                "Uma bactéria",
                "Uma proteína mal enovelada",
                "Um fungo"
            },
            correctIndex = 2,
            questionNumber = 42,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O mal de Alzheimer é causado por:",
            answers = new string[] {
                "Uma proteína mal enovelada",
                "Um vírus",
                "Uma bactéria",
                "Um fungo"
            },
            correctIndex = 0,
            questionNumber = 43,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O peptídeo β-amilóide é derivado da proteína:",
            answers = new string[] {
                "PrP",
                "APP",
                "Tau",
                "alfa-sinucleína"
            },
            correctIndex = 1,
            questionNumber = 44,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "As placas senis no cérebro de pacientes com Alzheimer são formadas por:",
            answers = new string[] {
                "Agregados de proteína Tau",
                "Agregados de peptídeo β-amilóide",
                "Fibras de colágeno",
                "Micelas de lipídios"
            },
            correctIndex = 1,
            questionNumber = 45,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Qual a principal característica das proteínas amiloidogênicas?",
            answers = new string[] {
                "Alta solubilidade em água",
                "Formação de fibrilas amilóides",
                "Baixa massa molecular",
                "Função enzimática"
            },
            correctIndex = 1,
            questionNumber = 46,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "A agregação de proteínas amiloidogênicas ocorre principalmente devido a:",
            answers = new string[] {
                "Interações hidrofílicas",
                "Interações hidrofóbicas",
                "Ligações peptídicas",
                "Pontes de hidrogênio"
            },
            correctIndex = 1,
            questionNumber = 47,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Doenças priônicas são causadas por:",
            answers = new string[] {
                "Vírus",
                "Bactérias",
                "Prions",
                "Fungos"
            },
            correctIndex = 2,
            questionNumber = 48,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "Um exemplo de encefalopatia espongiforme transmissível é:",
            answers = new string[] {
                "Doença de Alzheimer",
                "Mal de Parkinson",
                "Doença da Vaca Louca",
                "Febre aftosa"
            },
            correctIndex = 2,
            questionNumber = 49,
            isImageAnswer = false
        },
        new Question
        {
            questionDatabankName = "ProteinQuestionDatabase",
            questionText = "O que caracteriza um vírus?",
            answers = new string[] {
                "Organismo unicelular.",
                "Organismo multicelular.",
                "Partícula infecciosa composta de ácido nucléico e capsídeo protéico.",
                "Uma proteína com função enzimática."
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
        return QuestionSet.proteins;
    }

    public string GetDatabankName()
    {
        return "ProteinQuestionDatabase";
    }
}