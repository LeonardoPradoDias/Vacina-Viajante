library(ggplot2)
library(geobr)
library(ggrepel)
library(animation)

n <- 40

dados <- read.csv('caminhos.csv', sep = ";", row.names = NULL)
colnames(dados) <- colnames(dados)[2:ncol(dados)]
dados <- dados[ , - ncol(dados)]  
dados$Latitude <- scan(text=dados$Latitude, dec=",", sep=".")
dados$Longitude <-scan(text=dados$Longitude, dec=",", sep=".")
dados$Geracao <- as.integer(dados$Geracao)

num_gen <- nrow(dados)/n

sp <- read_state(code_state = "SP")
options(ggrepel.max.overlaps = Inf)

i <- 0
saveGIF({
while (i < num_gen) {
  
  arg1 <- i*n+1
  arg2 <- n*(i+1)
  
  dataSP <- dados[arg1:arg2, ]

  aux1 <- dataSP$Longitude[-1]
  xend_cities <- append(aux1, dataSP$Longitude[1])

  aux2 <- dataSP$Latitude[-1]
  yend_cities <- append(aux2, dataSP$Latitude[1])


  citiesSP <- data.frame(gen = dataSP$Geracao,
                         lon = dataSP$Longitude, 
                         lat = dataSP$Latitude, 
                         muni = dataSP$Nome)

  leg <- paste("Geração: ", dataSP$Geracao[1])
  subleg <- paste("Distância: ", dataSP$Distancia[1], "km")

  print(ggplot(sp)+
    geom_sf(fill = '#FDF9E4')+
    geom_point(data = citiesSP, aes(x = lon, y = lat), size = 2, pch = 21)+
    geom_segment(data = citiesSP, aes(x = lon, y = lat, xend = xend_cities, 
                                    yend = yend_cities),color = "blue", 
                                   arrow = arrow(length = unit(0.2,"cm")))+
  theme_light()+labs(title = leg, subtitle = subleg, x = NULL, y = NULL))

  i <- i+1
}})
