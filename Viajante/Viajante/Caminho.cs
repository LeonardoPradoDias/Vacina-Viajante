using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajante
{
    public class Caminho
    {
        //Inicializa lista de tamanho variável com as cidades
        public List<Cidade> c { get; private set; }
        //Distancia e aptidao serão calculadas por funcoes
        public double distancia { get; private set; }
        public double aptidao { get; private set; }

        public Caminho(List<Cidade> l)
        {
            this.c = l;
            this.distancia = this.calcDist();
            this.aptidao = this.calcAptidao();
        }

        public static Caminho aleatorio(int n)
        {
            List<Cidade> cidades = new List<Cidade>();

            for (int i = 0; i < n; ++i)
                cidades.Add(Cidade.aleatorio());

            return new Caminho(cidades);
        }

        public Caminho embaralhar()
        {
            List<Cidade> aux = new List<Cidade>(this.c);
            int n = aux.Count;

            while(n > 1)
            {
                n--;
                int k = Program.r.Next(n + 1);
                Cidade v = aux[k];
                aux[k] = aux[n];
                aux[n] = v;
            }

            return new Caminho(aux);


        }


        public Caminho cruzamento(Caminho m)
        {
            int i = Program.r.Next(0, m.c.Count);
            int j = Program.r.Next(i, m.c.Count);
            List<Cidade> s = this.c.GetRange(i, j - i + 1);
            List<Cidade> ms = m.c.Except(s).ToList();
            List<Cidade> c = ms.Take(i)
                             .Concat(s)
                             .Concat(ms.Skip(i))
                             .ToList();
            return new Caminho(c);
        }

        public Caminho mutacao()
        {
            List<Cidade> aux = new List<Cidade>(this.c);

            if (Program.r.NextDouble() < Amb.taxaMut)
            {
                int i = Program.r.Next(0, this.c.Count);
                int j = Program.r.Next(0, this.c.Count);
                Cidade v = aux[i];
                aux[i] = aux[j];
                aux[j] = v; 
            }

            return new Caminho(aux);
        }

        private double calcDist()
        {
            double total = 0;

            for (int i = 0; i < this.c.Count; ++i)
                total += this.c[i].distancia(this.c[(i + 1) % this.c.Count]);

            return total;

        }

        private double calcAptidao()
        {
            return (1 / (this.distancia + 1)); //+1 para o caso de a distancia ser zero
        }


    }
}
