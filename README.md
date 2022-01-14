# Vacina-Viajante
Inspirado pelo contexto atual da pandemia de Coronavírus, foi colocado em evidência a problemática do desafio logístico que consiste a distribuição de vacinas para o território nacional, que, dada a emergência da pandemia, deve ser o mais eficiente e rápida possível.

Pode-se relacionar esta questão ao problema do Caixeiro Viajante (Travelling Salesman Problem), que consiste na pergunta: "Dada uma lista de cidades e a distância entre elas, qual é a rota mais curta possível que visite todas as cidades uma vez e retorna à cidade de origem?". Esta questão pode ser transladada perfeitamente ao contexto da logística de distribuição de vacinas pelos estados brasileiros, a fim de eficientizar a distribuição.

Com base nisto, nosso trabalho consiste na aplicação de algoritmos evolutivos a fim de resolver este problema do Caixeiro Viajante contextualizado na distribuição de vacinas pelos estados de São Paulo e Minas Gerais, se utilizando de listas do IBGE com informações geográficas sobre as cidades (latitude e longitude), bem como demográficas (população), selecionando as *n* maiores cidades do estado desejado e permitindo ao algoritmo encontrar a melhor rota para percorrer estas cidades, através da evolução de gerações e critérios evolutivos como elitismo, população e taxa de mutação.

A análise dos resultados do algoritmo evolutivo implementado foi feita com Matlab, para análise da evolução através das gerações, e com R, para visualização gráfica da evolução dos caminhos encontrados.

**Vídeo do trabalho:** https://drive.google.com/file/d/1WrEDrEhO6E-O02fVomJY8KAY0nAyC5wy/view?usp=sharing

## Grupo
- Daniel Bernardes Pozzan - 10716608
- Isabela Oliveira Costa - 10747972
- Leonardo Prado Dias - 10684642

## Como executar
Para execução do programa, basta abrir o arquivo "Viajante.exe", presente na pasta "\bin\release". O arquivo .csv de entrada deve estar na mesma pasta.
