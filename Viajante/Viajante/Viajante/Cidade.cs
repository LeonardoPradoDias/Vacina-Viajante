using System;

namespace Viajante
{
    public class Cidade
    {
        //Cada cidade tem como propriedades suas coordenadas x e y
        public int Habitantes { get; private set; }
        public string Nome { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        //Construtor que carrega as coordenadas x e y para a cidade
        public Cidade(double latitude, double longitude, int hab, string nome)
        {
            this.Nome = nome;
            this.Habitantes = hab;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        //Calcula a distância da cidade interna ao objeto (this.cidade) com uma cidade recebida por parâmetro utilizando x^2+y^2 = d^2
        public double Distancia(Cidade cidade)
        {                                                                        //Multiplica por 107 para converter para km
            return 107*(Math.Sqrt(Math.Pow((cidade.Latitude - this.Latitude), 2) //Tira a raiz da diferença entre as coordenadas X ao quadrado
                        + Math.Pow((cidade.Longitude - this.Longitude), 2)));    //Somada com a diferença entre as coordenadas Y ao quadrado
        }

        //Função utilizada quando se quer gerar cidades aleatórias
        //Método cria nova cidade com coordenadas x e y aleatórias usando o objeto R
        /*public static Cidade Aleatorio()
        {
            return new Cidade(Program.R.NextDouble(), Program.R.NextDouble());
        }*/
    }
}
