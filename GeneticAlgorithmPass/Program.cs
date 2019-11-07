using System;
using System.Text;

namespace TrabalhoFinalInteligenciaComputacional
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Número de indivíduos que deseja gerar");
            int nInd = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Tamanho das chaves que deseja gerar");
            int nChave = Convert.ToInt32(Console.ReadLine());
            AlgoritmoGenetico GeneticAlgorithm = new AlgoritmoGenetico(nChave, nInd);
            Console.ReadKey();
        }
    }
}
