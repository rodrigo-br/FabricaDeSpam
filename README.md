# FabricaDeSpam

## Como usar

### Windows

- Instalar o [dotnet-sdk](https://dotnet.microsoft.com/pt-br/download)

- Baixar certificado

PowerShell

```PS
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p <YOUR .ENV CERTIFICATE_PASSWORD>
```

- Confiar no certificado

```PS
dotnet dev-certs https --trust
```

- Rodar containers pelo docker-compose

```PS
docker-compose up --build
```

- Os arquivos serão gerados dentro do container do docker na pasta files, tanto do consumer quanto do producer.  
  Para acessá-los pegue o ID do container que deseja acessar.

```PS
docker ps
```

```PS
docker exec -it CONTAINER_ID bash
cd files
```
