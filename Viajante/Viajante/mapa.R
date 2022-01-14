#Bibliotecas necessárias
library(ggplot2)
library(geobr)
library(ggrepel)
library(animation)

#Definição manual do número de cidades a serem analisadas
n <- 100

#Leitura e adequação dos dados (saída do programa principal)
dados <- read.csv('0,03-5-10-100-30000 (5311).csv', sep = ";", row.names = NULL)
colnames(dados) <- colnames(dados)[2:ncol(dados)]
dados <- dados[ , - ncol(dados)]  
dados$Latitude <- scan(text=dados$Latitude, dec=",", sep=".")
dados$Longitude <-scan(text=dados$Longitude, dec=",", sep=".")
dados$Geracao <- as.integer(dados$Geracao)

#Número de gerações
num_gen <- nrow(dados)/n

#Leitura dos dados geográficos
state <- read_state(code_state = "SP")
options(ggrepel.max.overlaps = Inf)

#Loop de geração dos gráficos
i <- 0
saveGIF({
  while (i < num_gen) {
    
    #Para leitura das linhas do arquivo de n em n (1 a n, n+1 a 2n, ..., (num_gen-1)n+1, num_gen*n))
    arg1 <- i*n+1
    arg2 <- n*(i+1)
    
    data_state <- dados[arg1:arg2, ]
    
    #Vetores de fim para segmentos - vetores lat e long shiftados por um
    aux1 <- data_state$Longitude[-1]
    xend_cities <- append(aux1, data_state$Longitude[1])
    
    aux2 <- data_state$Latitude[-1]
    yend_cities <- append(aux2, data_state$Latitude[1])
    
    #Data frame para plotagem
    cities_state <- data.frame(gen = data_state$Geracao,
                               lon = data_state$Longitude, 
                               lat = data_state$Latitude, 
                               muni = data_state$Nome)
    
    #Legendas
    leg <- paste("Geração: ", data_state$Geracao[1])
    subleg <- paste("Distância: ", data_state$Distancia[1], "km\nCidade de saída: ",
                    cities_state[1, ]$muni)
    
    #Plotagem dos gráficos para geração do GIF final
    print(ggplot(state)+
            geom_sf(fill = '#FDF9E4')+
            geom_point(data = cities_state[1, ], aes(x = lon, y = lat), size = 3)+
            geom_point(data = cities_state, aes(x = lon, y = lat), size = 2)+
            geom_label_repel(size = 2.5, data = cities_state[1, ], aes(x = lon, y = lat, label = muni))+
            geom_segment(data = cities_state, aes(x = lon, y = lat, xend = xend_cities, 
                                                  yend = yend_cities),color = "blue", 
                         arrow = arrow(length = unit(0.25,"cm")))+
            theme_light()+labs(title = leg, subtitle = subleg, x = "Longitude", y = "Latitude"))
    
    i <- i+1
  }}, movie.name = 'evolução.gif', interval = 0.25, ani.width = 700, ani.lenght = 700)
