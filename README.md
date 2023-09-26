# FabricaDeSpam


## Como usar

### Windows

- Instalar o [dotnet-sdk](https://dotnet.microsoft.com/pt-br/download)

- Baixar certificado

PowerShell
```PS
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx
```

- Confiar no certificado

```PS
dotnet dev-certs https --trust
```

- Rodar containers pelo docker-compose

```PS
docker-compose up --build
```

