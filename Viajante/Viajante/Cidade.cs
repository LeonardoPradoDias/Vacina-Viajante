using System;

namespace Viajante
{
    public class Cidade
    {
        //Cada cidade tem como propriedades suas coordenadas x e y
        public double X { get; private set; }
        public double Y { get; private set; }

        //Construtor que carrega as coordenadas x e y para a cidade
        public Cidade(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        //Calcula a distância da cidade interna ao objeto (this.cidade) com uma cidade recebida por parâmetro utilizando x^2+y^2 = d^2
        public double Distancia(Cidade cidade)
        {
            return Math.Sqrt(Math.Pow((cidade.X - this.X), 2)   //Tira a raiz da diferença entre as coordenadas X ao quadrado
                        + Math.Pow((cidade.Y - this.Y), 2));    //Somada com a diferença entre as coordenadas Y ao quadrado
        }

        //Método cria nova cidade com coordenadas x e y aleatórias usando o objeto R
        public static Cidade Aleatorio()
        {
            return new Cidade(Program.R.NextDouble(), Program.R.NextDouble());
        }
    }
}
