using System;
using System.IO;
using System.Text;

namespace Viajante
{
    public class Program
    {
        public static Random R { get; private set; }

        static void Main(string[] args)
        {
            R = new Random();       //Inicializa um objeto para gerar números aleatórios

            //Caminho dest = Caminho.Aleatorio(Amb.numCidades);       //Cria caminho aleatório com a quantidade de cidades especificadas em Amb.numcidades
            Caminho dest = Caminho.LerCsv(Amb.numCidades);
            Populacao populacao = Populacao.Randomizado(dest, Amb.tamPop);  //Gera a população de tamPop individuos randomizada a partir do caminho que se tem

            int geracao = 0;    //Variavel que conta as gerações
            bool melhor = true; //Variavel booleana que identifica se o caminho melhorou

            while (geracao < Amb.numGeracoes)
            {
                //if (melhor)
                    ExibeResultado(populacao, geracao); //Mostra para o usuario o melhor caminho da geração atual
                EscreveResultadoCSV(populacao, geracao);
                if(melhor)
                    EscreveMelhorCaminhoCSV(populacao, geracao);

                melhor = false;                         //Reseta variável melhor para false
                double antigo = populacao.MaxApt;       //Antigo agora recebe o melhor caminho da população

                populacao = populacao.Evoluir();        //Evolui a população
                if (populacao.MaxApt > antigo)          //Verifica se o melhor caminho melhorou em relação ao da pop. anterior
                    melhor = true;

                geracao++;                              //Incrementa o contador de gerações
            }
        }

        //Exibe os resultados para o usuário
        public static void ExibeResultado(Populacao populacao, int geracao)
        {
            Caminho melhor = populacao.AcharMelhor();   //Encontra o melhor caminho na população e exibe aptidao e distância
            System.Console.WriteLine("Geracao {0}\n" +
                "Melhor aptidao:  {1}\n" +
                "Menor distancia: {2}\n", geracao, melhor.Aptidao, melhor.Distancia);
        }

        //Função que cria arquivo CSV para ser interpretado pelo Matlab
        //Escreve Distancia;Aptidao;Geraçao;Se caminho veio de uma mutação ou não
        //Uma linha por geração, independente de ter melhorado o nao o caminho

        public static void EscreveResultadoCSV(Populacao populacao, int geracao)
        {
            Caminho melhor = populacao.AcharMelhor();
            StreamWriter arquivo = new StreamWriter("resultadoAG.csv", true);
            if(geracao == 0)
                arquivo.WriteLine("Distancia;Aptidao;Geracao;Mutou");

            arquivo.WriteLine(melhor.Distancia + ";" + melhor.Aptidao + ";" + geracao.ToString() + ";" + melhor.OcorreuMut);
            arquivo.Close();
        }

        //função que escreve em um .csv o melhor caminho encontrado na geração atual no formato:
        //Geração;Distancia total;Caminho veio de uma mutação ou nao; Nome da cidade; lat; long
        //Uma cidade para cada linha para facilitar o uso para a parte grafica em R
        public static void EscreveMelhorCaminhoCSV(Populacao populacao, int geracao)
        {
            Caminho melhor = populacao.AcharMelhor();
            StreamWriter arquivo = new StreamWriter("caminhos.csv", true);

            //Para gerar um cabeçalho no arquivo
            if(geracao == 0)
            {
                arquivo.WriteLine("Geracao;Distancia;Mutacao;Nome;Latitude;Longitude");
            }

            for(int i = 0; i < melhor.ListaCidades.Count; i++) { 
                arquivo.Write(geracao.ToString() + ";");
                arquivo.Write(melhor.Distancia + ";");
                arquivo.Write(melhor.OcorreuMut + ";");
                
                arquivo.Write(melhor.ListaCidades[i].Nome + ";");
                arquivo.Write(melhor.ListaCidades[i].Latitude + ";");
                arquivo.Write(melhor.ListaCidades[i].Longitude + ";");
                arquivo.WriteLine();
            }
            

            arquivo.Close();

        }

    }
    //Classe referente ao ambiente, onde são aplicados os parâmetros do algoritmo
    public static class Amb
    {
        public const double taxaMut = 0.03; //Número entre zero e um que indica a probabilidade de uma mutação ocorrer
        public const int elitismo = 3; //Quantidade de melhores indivíduos que permanecerão inalterados para a próxima geração
        public const int tamPop = 10;   //Tamanho total da população (de caminhos) que será utilizada no algoritmo
        public const int numCidades = 100;   //Número total de cidades em que se deve encontrar um caminho
        public const int numGeracoes = 30000;    //Critério de parada referente ao número de gerações do algoritmo
    }
}
