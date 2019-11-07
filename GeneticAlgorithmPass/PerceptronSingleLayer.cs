using System;

namespace TrabalhoFinalInteligenciaComputacional
{
    class PerceptronSingleLayer
    {
        private double[,] MatrizTreinar;
        private double[] saidaDesejada;
        private double CombinadorLinear;
        private double[] pesos;
        private double taxa_Aprendizado;
        private int MaxEpoca = 30;
        private static int contador = 0;
        public PerceptronSingleLayer()
        {

            MatrizTreinar = new double[,] { { 1, 0},//RUIM
                                            { 0, 1},//RUIM
                                            { 0.20, 0.15},//RUIM
                                            { 0.20 , 0.20 },//RUIM
                                            { 0.50 , 0.20 },//RUIM
                                            { 0.17, 0.15 },//RUIM
                                            { 0, 0 },//BOA
                                            { 0.15 , 0.13 },//BOA
                                            { 0.16, 0.10 },//BOA
                                            { 0.13 , 0.06 },//BOA
                                            { 0.09, 0.1 },//BOA
                                            { 0.10 , 0.04}, };//BOA


            pesos = new double[] { 0, 0, 0 };
            saidaDesejada = new double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 ,1};
            taxa_Aprendizado = 0.05;
        }

        public double ClassificarAmostra(float x1, float x2, int bias)
        {
            
            string resposta;
            double potencialAtivacao = x1 * pesos[0] + x2 * pesos[1] + bias * pesos[2];
            double y = g(potencialAtivacao);
            if(y > 0)
            {
                resposta = "A amostra[{0}] é uma Chave Boa u = {1}";
  
            }else
            {
                resposta = "A amostra[{0}] é uma Chave Ruim  u = {1}";
            }
            
            Console.WriteLine(resposta, contador, potencialAtivacao);
            contador++;
            return potencialAtivacao; 
        }
 
        private static int g(double u)
        {
            return (u >= 0) ? 1 : -1;
        }
        private void AjustePeso(int i, double saida)
        {
            double erro = saidaDesejada[i] - saida;
            Console.WriteLine();
            Console.Write(" Novos Pesos :[");

            for (int j = 0; j < pesos.Length; j++)
            {
                if (j != pesos.Length - 1)
                {
                    pesos[j] = pesos[j] + (taxa_Aprendizado * erro * MatrizTreinar[i, j]);
                }
                else
                {
                    pesos[j] = pesos[j] + (taxa_Aprendizado * erro * (-1)); //BIAS
                }
                Console.Write(" w({0})={1} ;", j, pesos[j]);
            }
            Console.Write("]");
            Console.WriteLine();
        }
        public void TreinarRede()
        {
            int epoca = 0;
            double saida = 0;
            string erro = "existe";

            while ((erro == "existe") || (epoca <= MaxEpoca))
            {
                erro = "inexiste";
                Console.WriteLine("****************************************************************************");
                Console.WriteLine(" Época:{0}", epoca);

                for (int i = 0; i < MatrizTreinar.GetLength(0); i++)
                {
                    CombinadorLinear = 0;
                    for (int j = 0; j < pesos.Length; j++)
                    {
                        if (j != pesos.Length - 1)
                        {
                            CombinadorLinear += MatrizTreinar[i, j] * pesos[j];
                        }
                        else
                        {
                            CombinadorLinear += (-1) * pesos[j];
                        }
                    }
                    saida = (CombinadorLinear >= 0) ? 1 : 0;
                    if (saida != saidaDesejada[i])
                    {
                        AjustePeso(i, saida);
                        erro = "existe";
                    }
                }
                epoca++;
            }
        }
    }
}
