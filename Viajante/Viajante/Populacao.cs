using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajante
{
    public class Populacao
    {
        // Variaveis
        public List<Caminho> p { get; private set; }
        public double maxApt { get; private set; }

        // ctor
        public Populacao(List<Caminho> l)
        {
            this.p = l;
            this.maxApt = this.calcMaxApt();
        }

        // Métodos
        public static Populacao randomizado(Caminho t, int n)
        {
            List<Caminho> tmp = new List<Caminho>();

            for (int i = 0; i < n; ++i)
                tmp.Add(t.embaralhar());

            return new Populacao(tmp);
        }

        private double calcMaxApt()
        {
            return this.p.Max(c => c.aptidao);
        }

        public Caminho selecionar()
        {
            while (true)
            {
                int i = Program.r.Next(0, Amb.tamPop);

                if (Program.r.NextDouble() < this.p[i].aptidao / this.maxApt)
                    return new Caminho(this.p[i].c);
            }
        }

        public Populacao gerarNovaPop(int n)
        {
            List<Caminho> p = new List<Caminho>();

            for (int i = 0; i < n; ++i)
            {
                Caminho t = this.selecionar().cruzamento(this.selecionar());

                foreach (Cidade c in t.c)
                    t = t.mutacao();

                p.Add(t);
            }

            return new Populacao(p);
        }

        public Populacao elite(int n)
        {
            List<Caminho> melhor = new List<Caminho>();
            Populacao tmp = new Populacao(p);

            for (int i = 0; i < n; ++i)
            {
                melhor.Add(tmp.acharMelhor());
                tmp = new Populacao(tmp.p.Except(melhor).ToList());
            }

            return new Populacao(melhor);
        }

        public Caminho acharMelhor()
        {
            foreach (Caminho t in this.p)
            {
                if (t.aptidao == this.maxApt)
                    return t;
            }

            // Nunca deve chegar aqui
            return null;
        }

        public Populacao evoluir()
        {
            Populacao best = this.elite(Amb.elitismo);
            Populacao np = this.gerarNovaPop(Amb.tamPop - Amb.elitismo);
            return new Populacao(best.p.Concat(np.p).ToList());
        }

    }
}
