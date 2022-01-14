%plot(Geracao, Aptidao, 'LineWidth', 2)

plot(Geracao, Distancia, 'LineWidth', 2)
hold on
anterior = 0;



for n = 1:length(Mutou)
    if(strcmp(Mutou(n), 'True') && anterior == 0)
        plot(n, Distancia(n), '.', 'MarkerSize', 25, 'Color', 'Red')
        anterior = 1;
    end
    if(strcmp(Mutou(n), 'False'))
        anterior = 0;
    end 

end