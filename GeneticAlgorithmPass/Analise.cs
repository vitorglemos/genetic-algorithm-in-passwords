using System;

namespace TrabalhoFinalInteligenciaComputacional
{
    class Analise
    {
        public int Consecutivo(string ind)
        {
            int ContConsecutivo = 0;
            for (int i = 0; i < ind.Length - 1; i++)
            {
                int atual = Int32.Parse(ind.Substring(i, 1));
                int proximo = Int32.Parse(ind.Substring(i + 1, 1));
                if (((atual - proximo) == -1) || ((atual - proximo) == 1))
                {
                    ContConsecutivo++;
                }
            }
            return ContConsecutivo;
        }
        public int Repetir(string ind)
        {
            int ContRepetidoConsecutivo = 0;
            for (int i = 0; i < ind.Length - 1; i++)
            {
                int atual = Int32.Parse(ind.Substring(i, 1));
                int proximo = Int32.Parse(ind.Substring(i + 1, 1));
                if (atual == proximo)
                {
                    ContRepetidoConsecutivo++;
                } 
            }
            return ContRepetidoConsecutivo++;
        }
    }
}
