# FabricaDeSpam

## Como usar

- Rodar containers pelo docker-compose

```PS
docker-compose up --build
```

## Acesso ao Swagger

Por padrão, a aplicação irá rodar na porta 5002.  
Para abrir o swagger basta acessar o `http://localhost:5002/swagger`

**PS: USAR SOMENTE REQUISIÇÕES HTTP.**

## Como acessar os arquivos

Os arquivos serão gerados dentro do container do docker na pasta files, tanto do consumer quanto do producer.  
 Para acessá-los pegue o ID do container que deseja acessar.

```PS
docker ps
```

```PS
docker exec -it CONTAINER_ID bash
cd files
```
