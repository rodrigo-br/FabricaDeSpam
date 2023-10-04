# FabricaDeSpam

## Como usar

- Clonar o repositório
```PS
https://github.com/rodrigo-br/FabricaDeSpam.git
```
- Criar um arquivo .env na raiz do projeto e preencher variáveis de acordo com o .envexample
- Garantir que nenhuma das portas a seguir estarão ocupadas: 80, 443, 8080, 5002, 5432 e 9092
- Rodar containers pelo docker-compose utilizando os comandos abaixo
```PS
cd src
docker-compose up --build
```
- Acessar http://localhost:80


