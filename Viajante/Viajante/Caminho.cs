
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Viajante
{
    public class Caminho    //Objeto caminho possui uma lista com a ordem das cidades
    {
        //Inicializa lista de tamanho variável com as cidades
        public List<Cidade> ListaCidades { get; private set; }
        //Distancia e aptidao serão calculadas por funcoes
        public double Distancia { get; private set; }
        public double Aptidao { get; private set; }
        public bool OcorreuMut { get; private set; } //Variável que indica se a geração atual foi gerada com uma mutação ou não

        //Construtor recebe a lista de cidades que corresponde ao caminho e calcula a distancia total e aptidao (inverso da distancia)

        public Caminho(List<Cidade> caminho)
        {
            this.ListaCidades = caminho;
            this.Distancia = this.CalcDist();
            this.Aptidao = this.CalcAptidao();
            this.OcorreuMut = false;
        }

        //Nao utilizado quando for ler csv
        //Cria lista de cidades aleatória e recebe como parâmetro a quantidade de cidades
        /*public static Caminho Aleatorio(int n)
        {
            List<Cidade> cidades = new List<Cidade>();  //Cria lista de tamanho variável com cidades

            for (int i = 0; i < n; ++i)
                //cidades.Add(Cidade.Aleatorio());        //Adiciona cidades aleatórias na lista

            return new Caminho(cidades);                //Retorna o caminho com cidades aleatórias
        }*/


        //Função que lê arquivo csv com a lista de cidades de um estado e retorna um caminho inicial composto de todas as cidades em sequência
        public static Caminho LerCsv(int nLinhas)
        {
            List<Cidade> cidades = new List<Cidade>();  //Lista que armazenará as cidades
            using(StreamReader reader = new StreamReader(@"MG.csv"))    //Abre o arquivo para leitura
            {
                
                string[] valores;   //Sequência de strings que armazenará cada uma das colunas por vez
                var linha = reader.ReadLine();  //Primeira linha é lida e ignorada (cabeçalho)

                for (int i = 0; i < nLinhas; i++)
                {
                    linha = reader.ReadLine();  //Leitura de uma linha
                    valores = linha.Split(';'); //Separação dos valores de cada coluna, indicada por ponto e vírgula
                    //Cria nova cidade que será adicionada a lista de cidades com os valores extraídos do csv
                    Cidade cidadeAux = new Cidade(Convert.ToDouble(valores[2]), Convert.ToDouble(valores[1]), Convert.ToInt32(valores[3]), valores[0]);

                    cidades.Add(cidadeAux); //Adiciona a nova cidade na lista de cidades
                }
            }
            return new Caminho(cidades);    //Retorna o caminho com as cidades
        }

        //Embaralha um caminho, usado para gerar uma população
        public Caminho Embaralhar()
        {
            List<Cidade> aux = new List<Cidade>(this.ListaCidades);  //Lista de cidades auxiliar igual aquela recebida na criação do objeto
            int n = aux.Count;                                  //n contém o número de cidades no caminho a ser embaralhado

            while (n > 1)                                       //Uma cidade aleatória será trocada de posição com a cidade indíce n. N vezes
            {
                n--;
                int k = Program.R.Next(n + 1);                  //k recebe um valor aletório entre 1 e n+1
                Cidade cidadeAux = aux[k];                      //Cidade auxiliar recebe uma cidade aleatória da lista                  
                aux[k] = aux[n];                                //Cidade aleatória recebe a cidade n
                aux[n] = cidadeAux;                             //Cidade n recebe a cidade aleatória
            }

            return new Caminho(aux);                            //Retorna novo caminho embaralhado


        }

        //Faz o cruzamento entre o caminho "interno" (this.ListaCidades) e um caminho passado como parâmetro
        public Caminho Cruzamento(Caminho caminho)
        {
            int i = Program.R.Next(0, caminho.ListaCidades.Count);  //Recebe número aleatório entre 0 e o número de cidades do caminho
            int j = Program.R.Next(i, caminho.ListaCidades.Count);  //Recebe número aleatório entre i e o número de cidades do caminho para que j sempre seja > que i
            List<Cidade> c1 = this.ListaCidades.GetRange(i, j - i + 1);  //Lista com as cidades com indices entre i e j de this.ListaCidades
            List<Cidade> c2 = caminho.ListaCidades.Except(c1).ToList();  //Lista com as cidades de caminho, exceto aquelas que já estão em c1
            List<Cidade> cr = c2.Take(i)                             //Cria novo caminho/gene colocando as cidades c1 no meio das cidades c2
                             .Concat(c1)
                             .Concat(c2.Skip(i))
                             .ToList();
            return new Caminho(cr);         //Retorna novo gene após cruzamento
        }

        //Faz mutação ou não em um caminho, a depender da probabilidade da taxa de mutação
        public Caminho Mutacao()
        {
            List<Cidade> aux = new List<Cidade>(this.ListaCidades); //Lista auxiliar em que ocorrerá ou não a mutação
            bool mutou = false;
            if (Program.R.NextDouble() < Amb.taxaMut)   //Gera número aleatório entre 0 e 1, que será comparado a taxa de mutação, quanto mais alta mais chance de ter mutação
            {
                mutou = true;
                int i = Program.R.Next(0, this.ListaCidades.Count); //Recebe número aleatório entre zero e o número de cidades
                int j = Program.R.Next(0, this.ListaCidades.Count); //Recebe número aleatório entre zero e o número de cidades
                Cidade cidadeAux = aux[i];  //Cidade auxiliar será utilizada para trocar duas cidades aleatórias de ordem
                aux[i] = aux[j];
                aux[j] = cidadeAux; 
            }

            Caminho novoCaminho = new Caminho(aux);
            novoCaminho.OcorreuMut = mutou;

            return novoCaminho;    //Caminho após a mutação (ou não mutação) é retornado
        }

        //Calcula a distância
        private double CalcDist()
        {
            double total = 0; //Variavel que representará a distância total

            for (int i = 0; i < this.ListaCidades.Count; ++i)   //Percorre todas as cidades e somas as ditâncias entre elas
                total += this.ListaCidades[i].Distancia(this.ListaCidades[(i + 1) % this.ListaCidades.Count]); 
                                            //Aqui o operador módulo não causa alteração até a penultima cidade. Quando chega na última ele transforma o parâmetro de "ListaCidades"
                                            //Em zero, então têm se a soma da primeira com a última cidade para finalizar o cálculo da distância total.
            return total;   //Retorna distância total

        }

        //Calcula a aptidao do caminho como o inverso da distância
        private double CalcAptidao()
        {                                             //Distancia divida por mil para o número de aptidão ficar maior
            return (1 / ((this.Distancia/1000) + 1)); //+1 para o caso de a distancia ser zero
        }


    }
}
