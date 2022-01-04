using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajante
{
    public class Cidade
    {
        //Cada cidade tem como propriedades suas coordenadas x e y
        public double x { get; private set; }
        public double y { get; private set; }

        //Construtor 
        public Cidade(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        //Métodos
        //Calcula a distância por x^2+y^2 = d^2
        public double distancia(Cidade c)
        {
            return Math.Sqrt(Math.Pow((c.x - this.x), 2)
                        + Math.Pow((c.y - this.y), 2));
        }

        public static Cidade aleatorio()
        {
            return new Cidade(Program.r.NextDouble(), Program.r.NextDouble());
        }
    }
}
