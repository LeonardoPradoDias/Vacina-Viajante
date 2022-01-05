
using System.Collections.Generic;
using System.Linq;

namespace Viajante
{
    public class Populacao
    {
        public List<Caminho> ListaCaminhos { get; private set; }    //Lista com os caminhos que compõem a população
        public double MaxApt { get; private set; }  //Variável que armazena a aptidão máxima em uma população

        // Construtor que calula aptidão máxima e recebe a população a ser trabalhada
        public Populacao(List<Caminho> populacao)
        {
            this.ListaCaminhos = populacao;
            this.MaxApt = this.CalcMaxApt();
        }

        // Método para gerar população randomizada com base em um caminho passado como parâmetro
        public static Populacao Randomizado(Caminho caminho, int n)
        {
            List<Caminho> tmp = new List<Caminho>();    //Lista de tamanho variável com os caminhos a serem gerados

            for (int i = 0; i < n; ++i)
                tmp.Add(caminho.Embaralhar());          //Adiciona n novos caminhos embaralhados à lista

            return new Populacao(tmp);                  //Retorna população de caminhos randomizados 
        }

        //Procura qual o caminho com maior valor de aptidão na população
        private double CalcMaxApt()
        {
            return this.ListaCaminhos.Max(c => c.Aptidao);
        }

        //Seleciona um caminho aleatório da população
        public Caminho SelecionaCaminhoAleatorio()
        {
            while (true)
            {
                int i = Program.R.Next(0, Amb.tamPop);  //i recebe um número aleatório entre 0 o tamanho da população

                if (Program.R.NextDouble() <= this.ListaCaminhos[i].Aptidao / this.MaxApt)   //Normaliza a aptidao do individuo i, compara com um double aleatorio entre 0 e 1
                    return new Caminho(this.ListaCaminhos[i].ListaCidades);  //Retorna o caminho aleatório
            }
        }

        //Função que gera uma nova população com base em cruzamento e possibilidade de mutação
        public Populacao GerarNovaPop(int n)
        {
            List<Caminho> novaPop = new List<Caminho>();    //Lista que vai conter a nova população

            for (int i = 0; i < n; ++i)
            {
                Caminho caminho = this.SelecionaCaminhoAleatorio().Cruzamento(this.SelecionaCaminhoAleatorio()); //Faz o cruzamento de dois caminhos aleatórios da população

                foreach (Cidade c in caminho.ListaCidades)    //Aplica a função de mutação em cada um dos caminhos
                    caminho = caminho.Mutacao();              //Mutação pode ou não ocorrer dependendo da taxa de mutação

                novaPop.Add(caminho);   //Novo caminho é adicionado a lista da nova população
            }

            return new Populacao(novaPop);  //Retorna a nova população
        }

        //Escolhe os n individuos mais aptos da população
        public Populacao Elite(int n)
        {
            List<Caminho> melhores = new List<Caminho>();   //Lista com os melhores caminhos que vao sendo extraidos da população total
            Populacao populacaoAux = new Populacao(ListaCaminhos);      //Lista temporaria com a população total, a elite vai sendo retirada dela e coloca na "melhores"

            for (int i = 0; i < n; ++i)
            {
                melhores.Add(populacaoAux.AcharMelhor());   //Adiciona o melhor individuo na lista dos melhores caminhos
                populacaoAux = new Populacao(populacaoAux.ListaCaminhos.Except(melhores).ToList()); //População auxiliar recebe a população total sem os melhores
            }

            return new Populacao(melhores); //Retorna os n melhores caminhos
        }

        //Acha o melhor caminho da população
        public Caminho AcharMelhor()
        {
            foreach (Caminho caminho in this.ListaCaminhos) //Passa caminho a caminho da população
            {
                if (caminho.Aptidao == this.MaxApt)     //Se a aptidao do caminho atual for igual ao melhor da população atual, retorna
                    return caminho;
            }

            // Nunca deve chegar aqui
            return null;
        }

        //Faz a evolução da população
        public Populacao Evoluir()
        {
            Populacao best = this.Elite(Amb.elitismo);  //Escolhe os (Amb.elistismo) individuos mais aptos da população
            Populacao novaPop = this.GerarNovaPop(Amb.tamPop - Amb.elitismo);    //Nova população de (tamanho total - elite) é gerada com cruzamentos e mutações
            return new Populacao(best.ListaCaminhos.Concat(novaPop.ListaCaminhos).ToList());    //População nova gerada concatenando a elite com a gerada por evolução
        }

    }
}
