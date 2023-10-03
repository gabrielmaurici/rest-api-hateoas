# rest-api-hateoas
Projeto com finalidade de aplicar classes para aplicar o nível de maturidade 3 (HATEOAS) em uma API Rest

Para rodar o projeto entre na pasta rest-api-hateoas e rode o comando docker compose up -d, a api irá rodar na porta 5105.

As classes criadas para o HATEOAS se encontram na pasta Hateoas, sendo elas LinkModel e ResourceHateoas

Classe LinkModel: Modelo para gerar os links dos recursos disponíveis na API
<img src="images/link-model.png" alt="link-model" width="100%">

Classe ResourceHateoas: Possui um método para retornar de forma dinâmica qualquer modelo de dados junto do array de LinkModel em formato de objeto 
![resource-hateoas](images/resource-hateoas.png)


Para utilização das classes foi criado um método na ProductsController onde recebe o Product e a lista de recursos relacionados ao recurso atual, e nesse método é utilizado a classe ResourceHateoas para padronizar o formato da response.
![controller](images/controller.png)


O resultado da api com HATEOAS é o seguinte
![resultado](images/resultado.png)