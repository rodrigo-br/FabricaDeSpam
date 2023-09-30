FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base

WORKDIR /app

# change sdk to aspnet in prodution
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY Consumer/Consumer.csproj Consumer/
RUN dotnet restore Consumer/Consumer.csproj
COPY . .
WORKDIR /src/Consumer
RUN dotnet build Consumer.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Consumer.csproj -c Release -o /app/publish

FROM base AS final

WORKDIR /app
RUN mkdir /app/files
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Consumer.dll" ]