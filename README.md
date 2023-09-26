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

```dotnet dev-certs https --trust```

- Rodar containers pelo docker-compose

```docker-compose up --build```

