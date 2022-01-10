library(ggplot2)
library(geobr)
library(ggrepel)
library(ggforce)

dataSP <- read.csv('SP.csv', nrows = 50)

aux1 <- dataSP$Long[-1]
xend_cities <- append(aux1, dataSP$Long[1])

aux2 <- dataSP$Lat[-1]
yend_cities <- append(aux2, dataSP$Lat[1])

sp <- read_state(code_state = "SP")

citiesSP <- data.frame(lon = dataSP$Long, lat = dataSP$Lat, muni = dataSP$Nome)

options(ggrepel.max.overlaps = Inf)

ggplot(sp)+
  geom_sf(fill = '#FDF9E4')+
  geom_point(data = citiesSP, aes(x = lon, y = lat), size = 2, pch = 21)+
  geom_segment(data = citiesSP, aes(x = lon, y = lat, xend = xend_cities, 
                                    yend = yend_cities),color = "blue", arrow = arrow(length = unit(0.2,"cm")))+
  theme_light()+labs(x = NULL, y = NULL)
