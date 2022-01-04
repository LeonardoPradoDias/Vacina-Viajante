using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajante
{
    public class Program
    {
        public static Random r { get; private set; }

        static void Main(string[] args)
        {
            r = new Random();

            Caminho dest = Caminho.aleatorio(Amb.numCidades);

            Populacao p = Populacao.randomizado(dest, Amb.tamPop);

            int geracao = 0;
            bool melhor = true;

            while (geracao < 100)
            {
                if (melhor)
                    resultado(p, geracao);

                melhor = false;
                double antigo = p.maxApt;

                p = p.evoluir();
                if (p.maxApt > antigo)
                    melhor = true;

                geracao++;
            }
        }

        public static void resultado(Populacao p, int geracao)
        {
            Caminho melhor = p.acharMelhor();
            System.Console.WriteLine("Geracao {0}\n" +
                "Melhor aptidao:  {1}\n" +
                "Menor distancia: {2}\n", geracao, melhor.aptidao, melhor.distancia);
        }

    }
    public static class Amb
    {
        public const double taxaMut = 0.03;
        public const int elitismo = 30;
        public const int tamPop = 60;
        public const int numCidades = 40;
    }
}
