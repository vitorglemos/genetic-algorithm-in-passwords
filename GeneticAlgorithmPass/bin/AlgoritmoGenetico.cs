using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics; 

namespace TrabalhoFinalInteligenciaComputacional
{
    class ArquivoChaves
    {
        public double IdU;
        public string geracao; 
    }
    class AlgoritmoGenetico:Analise
    {
        private string pai01;
        private string pai02;
        private int TamanhoKey;
      
        private PerceptronSingleLayer Perceptron;
        private List<ArquivoChaves> Melhores = new List<ArquivoChaves>();

        private double TimeSimpleMutation;
        private double TimeMultiMutation;
        private double TimeSimplePermutation;
        private double TimeMultiPermutation;

        private List<string> individuo = new List<string>();
        private List<string> BestKey = new List<string>();
        private List<string> filhos = new List<string>();
        private double TimeCrossMulti;
        private Stopwatch tempoMulti = new Stopwatch();
        private Stopwatch tempoOne = new Stopwatch();
        private double TimeCrossSingle;
        private List<string> MultiCross = new List<string>();
        private List<string> SingleCross = new List<string>();
        public AlgoritmoGenetico(int TamanhoChave, int nIndividuo)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            TamanhoKey = TamanhoChave;
            Perceptron = new PerceptronSingleLayer();
            Perceptron.TreinarRede();
            ProcedurePopulacaoInicial(nIndividuo);
            
        }
        private void ProcedurePopulacaoInicial(int numeroIndividuos)
        {
            string FirstIndividuos = "";
            string painel = new string('=', 50);
            Random NumbersRandom = new Random();
            Console.WriteLine(painel + " Geração Inicial de Indivíduos " + painel + "\n");

            for (int i = 0; i < numeroIndividuos; i++)
            {
                for (int j = 0; j < TamanhoKey; j++)
                {
                    FirstIndividuos += NumbersRandom.Next(0, 9);
                }
                individuo.Add(FirstIndividuos);
                FirstIndividuos = "";
            }

            ProcedureNextStep();
        }
        private void ProcedureNextStep()
        {
            ProcedureShowPopulation();
            Console.WriteLine("************************* Simple Crossover *********************** \n");
            ProcedureGenerateNewChildren();
            ProcedureGenerateNewChildrenMulti();
            Console.WriteLine();
            Console.WriteLine("***************** ONE POINT CROSSOVER ******************");
            ProcedureExibir(SingleCross);
            Console.WriteLine(TimeCrossSingle);
            Console.WriteLine();
            Console.WriteLine("***************** TWO POINT CROSSOVER ******************");
            ProcedureExibir(MultiCross);
            Console.WriteLine(TimeCrossMulti);

             List<ArquivoChaves> ASimpleMutation = new List<ArquivoChaves>();
             List<ArquivoChaves> ASimplePermutation = new List<ArquivoChaves>();
             List<ArquivoChaves> AMultiMutation = new List<ArquivoChaves>();
             List<ArquivoChaves> AMultiPermutation = new List<ArquivoChaves>();

             ASimpleMutation = ProcedureSimpleMutation(filhos);
             ASimplePermutation = ProcedureMutationPermutation(filhos);
             AMultiMutation = ProcedureMultiMutation(filhos);
             AMultiPermutation = ProcedureMultiPermutation(filhos);
             Console.WriteLine("");

             Console.WriteLine(" ***************************** Mutação Simples ********************************");
             Console.WriteLine("");
             ProcedureOrdenar(ASimpleMutation);
             Console.WriteLine(" Tempo de execução: {0} ms", Math.Round(TimeSimpleMutation,2));
             Console.WriteLine("");

             Console.WriteLine(" ***************************** Mutação Por Permutação ********************************");
             Console.WriteLine("");
             ProcedureOrdenar(ASimplePermutation);
             Console.WriteLine(" Tempo de execução: {0} ms", Math.Round(TimeSimplePermutation,2));
             Console.WriteLine("");

             Console.WriteLine(" **************************** Mutação Multipla ********************************");
             Console.WriteLine("");
             ProcedureOrdenar(AMultiMutation);
             Console.WriteLine(" Tempo de execução: {0} ms", Math.Round(TimeMultiMutation,2));
             Console.WriteLine("");
             Console.WriteLine(" *************************** Mutação Multipla Por Permutação ********************************");

             ProcedureOrdenar(AMultiPermutation);
             Console.WriteLine("");
             Console.WriteLine(" Tempo de execução: {0} ms", Math.Round(TimeMultiPermutation,2));
             Console.WriteLine(" *****************************  CHAVES : ANTES DA MUTACAO ********************************");
             Console.WriteLine("");
             ProcedureAntMutation();
        }
           
        private void ProcedureAntMutation()
        {
            Console.WriteLine(" ********************************************************");
            for(int i = 0; i < filhos.Count; i++)
            {
                double PorcentagemC = Math.Round((Consecutivo(filhos[i]) / (float)TamanhoKey) * 100,2);
                double PorcentagemR = Math.Round((Repetir(filhos[i]) / (float)TamanhoKey) * 100,2);
                double Total = PorcentagemC + PorcentagemR;

                Console.WriteLine(" CHAVE[{0}]: {1}   *C:{2}%  *R:{3}%  Total:{4}% ", i, filhos[i], PorcentagemC, PorcentagemR, Total);
            }
        }
        private void ProcedureOrdenar(List<ArquivoChaves> Chaves)
        {
            for(int i = 0; i < Chaves.Count; i++)
            {
                    Chaves = Chaves.OrderByDescending(x => x.IdU).ToList();
                    if(Chaves[i].IdU >= 0)
                    {
                    double PorcentagemC = Math.Round((Consecutivo(Chaves[i].geracao) / (float)TamanhoKey) * 100, 2);
                    double PorcentagemR = Math.Round((Repetir(Chaves[i].geracao) / (float)TamanhoKey) * 100, 2);
                    double Total = PorcentagemC + PorcentagemR;
                    Console.WriteLine("[{0}] Potencial(u):{1} CHAVE: {2}  *C:{3}%  *R:{4}%  Total:{5}% ", i, Chaves[i].IdU, Chaves[i].geracao, PorcentagemC, PorcentagemR, Total);
                     }

            }
        }
       
        private double FuncaoFitness(float x1, float x2)
        {
           double potencialU = Perceptron.ClassificarAmostra(x1, x2, -1);
           return potencialU;
        }
        private void ProcedureGenerateNewChildrenMulti()
        {
            int SelecionarPar = 0;
            for (int i = 0; i < BestKey.Count; i++)
            {
                if ((SelecionarPar % 2 == 0) || (SelecionarPar == 0))
                {
                    pai01 = BestKey[i];
                    Console.WriteLine("Pai {0} : {1}", i, pai01);
                }
                else
                {
                    pai02 = BestKey[i];
                    Console.WriteLine("Pai {0} : {1}", i, pai02);
                    MultiCrossover();
                }
                SelecionarPar++;
            }
        }

        private void ProcedureGenerateNewChildren()
        {
            int SelecionarPar = 0;
            for (int i = 0; i < BestKey.Count; i++)
            {
                if ((SelecionarPar % 2 == 0) || (SelecionarPar == 0))
                {
                    pai01 = BestKey[i];
                    Console.WriteLine("Pai {0} : {1}", i, pai01);
                }
                else
                {
                    pai02 = BestKey[i];
                    Console.WriteLine("Pai {0} : {1}", i, pai02);
                    Crossover();
                }
                SelecionarPar++;
            }
        }
     
        private void Crossover()
        {
              string painel = new string('_', TamanhoKey + 40);
              Stopwatch temposingle = new Stopwatch();
              temposingle.Start();
              Random CrossoverRandomico = new Random();
              int PontoCrossover = CrossoverRandomico.Next(1, TamanhoKey-1);
              string NovoFilho = pai01.Substring(0, PontoCrossover) + pai02.Substring(PontoCrossover);

            float PorcentagemC = (Consecutivo(NovoFilho) / (float)TamanhoKey) * 100;
            float PorcentagemR = (Repetir(NovoFilho) / (float)TamanhoKey) * 100;
            filhos.Add(NovoFilho);
            temposingle.Stop();
            TimeCrossSingle = temposingle.Elapsed.TotalMilliseconds;
            SingleCross.Add(NovoFilho);
         
            Console.WriteLine(" Filho : {0} - Crossover : {1} C:={2}%  R:={3}%", NovoFilho, PontoCrossover, PorcentagemC, PorcentagemR);
            Console.WriteLine(painel); 
        }
        private void ProcedureExibir(List<string> chaves)
        {
            for(int i = 0; i< chaves.Count; i++)
            {
                float PorcentagemC = (Consecutivo(chaves[i]) / (float)TamanhoKey) * 100;
                float PorcentagemR = (Repetir(chaves[i]) / (float)TamanhoKey) * 100;
                Console.WriteLine(" Filho:["+i+"]:{0}  *C:{1}%  *R:{2}%  Total:{3}% ", chaves[i], PorcentagemC, PorcentagemR, PorcentagemR + PorcentagemC);
            }
        }
        private void MultiCrossover()
        {
            string painel = new string('_', TamanhoKey + 40);
            Stopwatch tempoMulti = new Stopwatch();
            tempoMulti.Start();
            Random CrossP0 = new Random();
            Random CrossP1 = new Random();
            int ponto1 = CrossP0.Next(1, pai01.Length / 2);
            int ponto2 = CrossP1.Next(ponto1, pai01.Length);
            Console.WriteLine("Ponto 1:{0} Ponto 2:{1}", ponto1, ponto2);
            string inicio = pai01.Substring(0, ponto1);
            string meio = pai02.Substring(ponto1, ponto2 - ponto1);
            string fim = pai01.Substring(ponto2);

            string total = inicio + meio + fim;
             float PorcentagemC = (Consecutivo(total) / (float)TamanhoKey) * 100;
            float PorcentagemR = (Repetir(total) / (float)TamanhoKey) * 100;
            filhos.Add(total);
            tempoMulti.Stop();
            TimeCrossMulti = tempoMulti.Elapsed.TotalMilliseconds;
            MultiCross.Add(total);
           Console.WriteLine(" Filho : {0} - Pontos de Crossover: {1} , {2} C:={3}%  R:={4}%", total, ponto1,ponto2 ,PorcentagemC, PorcentagemR);
           Console.WriteLine(painel);

        }
        private List<ArquivoChaves> ProcedureMultiMutation(List<string> Chaves)
        {
            Stopwatch tempo = new Stopwatch();
            List<ArquivoChaves> Armazenamento = new List<ArquivoChaves>();
            tempo.Start();

            for (int i = 0; i < Chaves.Count; i++)
            {
                ArquivoChaves Keys = new ArquivoChaves();
                StringBuilder NovaString = new StringBuilder(filhos[i]);

                int TamanhoMax = TamanhoKey / 10;
                for (int j = 0; j < TamanhoMax ; j++)
                {
                    Random PontoRandom = new Random();
                    Random NumeroRandom = new Random();

                    int Ponto = PontoRandom.Next(0, NovaString.Length);
                    string Numero = (NumeroRandom.Next(0, 9)).ToString();

                    NovaString.Remove(Ponto, 1);
                    NovaString.Insert(Ponto, Numero);
                }

                    float PorcentagemC = Consecutivo(NovaString.ToString()) / (float)TamanhoKey;
                    float PorcentagemR = Repetir(NovaString.ToString()) / (float)TamanhoKey;

                    double u = FuncaoFitness(PorcentagemC, PorcentagemR);
                    Keys.geracao = NovaString.ToString();
                    Keys.IdU = u;
                    Armazenamento.Add(Keys);
                   /* Console.WriteLine("________________________________________________________");
                    Console.WriteLine(" IND APÓS MUTACAO({0}): {1}  *C:{2}% *R:{3}%", i, NovaString, PorcentagemC * 100, PorcentagemR * 100);
                    Console.WriteLine("_____________________________________________________________");*/
              
            }
           tempo.Stop();
           TimeMultiMutation = tempo.Elapsed.TotalMilliseconds;
           return Armazenamento;
         
        }

        private List<ArquivoChaves> ProcedureMultiPermutation(List<string> Chaves)
        {
            Stopwatch tempo = new Stopwatch();
            List<ArquivoChaves> Armazenamento = new List<ArquivoChaves>();
            tempo.Start();
            for(int i = 0; i < Chaves.Count; i++)
            {
                ArquivoChaves keys = new ArquivoChaves();
                StringBuilder NovaString = new StringBuilder(filhos[i]);
                int TamanhoMax = TamanhoKey / 10;
                for (int j = 0; j < TamanhoMax; j++)
                {
                    Random PontoA = new Random();
                    Random PontoB = new Random();

                    int Ponto0 = PontoA.Next(0, NovaString.Length);
                    int Ponto1 = PontoB.Next(0, NovaString.Length);
                    char StringA = NovaString[Ponto0];
                    char StringB = NovaString[Ponto1];

                    NovaString.Remove(Ponto0, 1);
                    NovaString.Insert(Ponto0, StringB);
                    NovaString.Remove(Ponto1, 1);
                    NovaString.Insert(Ponto1, StringA);
                }
              
                float PorcentagemC = Consecutivo(NovaString.ToString())/ (float)TamanhoKey;
                float PorcentagemR = Repetir(NovaString.ToString())/ (float)TamanhoKey;

                double u = FuncaoFitness(PorcentagemC, PorcentagemR);
                keys.geracao = NovaString.ToString();
                keys.IdU = u;
                Armazenamento.Add(keys);
               /* Console.WriteLine("________________________________________________________");
                Console.WriteLine(" IND APÓS MUTACAO({0}): {1}  *C:{2}% *R:{3}%", i, NovaString, PorcentagemC*100, PorcentagemR * 100);
                Console.WriteLine("_____________________________________________________________");*/

            }
            tempo.Stop();
            TimeMultiPermutation = tempo.Elapsed.TotalMilliseconds;
            return Armazenamento;
        }
       
        private List<ArquivoChaves> ProcedureMutationPermutation(List<string> Chaves)
        {
            Stopwatch tempo = new Stopwatch();
            List<ArquivoChaves> Armazenamento = new List<ArquivoChaves>();

            tempo.Start();
            for (int i = 0; i < Chaves.Count; i++)
            {
                ArquivoChaves Keys = new ArquivoChaves();
                StringBuilder NovaString = new StringBuilder(Chaves[i]);
  
                int DivisaoSenha = (NovaString.Length) / 2;
                Random p0 = new Random();
                Random p1 = new Random();

                float PorcentagemC = Consecutivo(NovaString.ToString()) / (float)TamanhoKey;
                float PorcentagemR = Repetir(NovaString.ToString()) / (float)TamanhoKey;

                int Ponto0 = p0.Next(0, DivisaoSenha);
                int Ponto1 = p1.Next(DivisaoSenha, NovaString.Length);
                char StringA = NovaString[Ponto0];
                char StringB = NovaString[Ponto1];

                NovaString.Remove(Ponto0, 1);
                NovaString.Insert(Ponto0, StringB);
                NovaString.Remove(Ponto1, 1);
                NovaString.Insert(Ponto1, StringA);

                double u = FuncaoFitness(PorcentagemC, PorcentagemR);
                Keys.geracao = NovaString.ToString();
                Keys.IdU = u;
                Armazenamento.Add(Keys);

               /* Console.WriteLine("IND APÓS MUTACAO({0}): {1}  *PontoMut:{2} : {3}  *String:{3} *C:{4}% *R:{5}%", i, NovaString, Ponto0, Ponto1, PorcentagemC * 100, PorcentagemR * 100);
                Console.WriteLine("_____________________________________________________________");*/
                
            }
            tempo.Stop();
            TimeSimplePermutation = tempo.Elapsed.TotalMilliseconds;
            return Armazenamento;  
        }
        private List<ArquivoChaves> ProcedureSimpleMutation(List<string> Chaves)
        {
            Stopwatch tempo = new Stopwatch();
            Random MutacaoRandomico = new Random();
            List<ArquivoChaves> Armazenamento = new List<ArquivoChaves>();

            tempo.Start();
            for(int i = 0; i < filhos.Count ; i++)
            {
                ArquivoChaves Keys = new ArquivoChaves();
                StringBuilder NovaString = new StringBuilder(Chaves[i]);

                float PorcentagemC = Consecutivo(NovaString.ToString()) / (float)TamanhoKey;
                float PorcentagemR = Repetir(NovaString.ToString()) / (float)TamanhoKey;
                float Total = (PorcentagemC + PorcentagemR) / 2;
                int pontoMutacao = 0;
                int NovoChar = 0;
              
                double u = FuncaoFitness(PorcentagemC, PorcentagemR);
                Keys.geracao = NovaString.ToString();
                Keys.IdU = u;
                Armazenamento.Add(Keys);
              
              /*  Console.WriteLine("IND APÓS MUTACAO({0}): {1}  *PontoMut:{2}  *String:{3} *C:{4}% *R:{5}%",i,NovaString,pontoMutacao,NovoChar,PorcentagemC*100,PorcentagemR*100);
                Console.WriteLine("_____________________________________________________________");*/
                
            }
            tempo.Stop();
            TimeSimpleMutation = tempo.Elapsed.TotalMilliseconds;
            return Armazenamento; 
        }
        private void ProcedureShowPopulation()
        {
            string painel = new string('_', TamanhoKey + 50);
            for(var i = 0; i < individuo.Count; i++)
            {
             
                float PorcentagemC = Consecutivo(individuo[i]) / (float)TamanhoKey;
                float PorcentagemR = Repetir(individuo[i]) / (float)TamanhoKey;
                Console.WriteLine("________________________________________________________");
                Console.WriteLine("  CHAVE[{0}]: {1}", i, individuo[i]);
                double Potencial = FuncaoFitness(PorcentagemC, PorcentagemR);
                if(Potencial > 0)
                {
                    BestKey.Add(individuo[i]);
                    Console.WriteLine("ESTA CHAVE FOI SELECIONADA P/CROSSOVER");
                }else
                {
                    Console.WriteLine("ESTA CHAVE NÃO FOI SELECIONADA P/CROSSOVER");
                }
                Console.WriteLine("  *CONS.: {0}   *REPET.:{1} ", PorcentagemC * 100, PorcentagemR * 100);
   
            }
            Console.WriteLine(painel);
        }
    }
}

