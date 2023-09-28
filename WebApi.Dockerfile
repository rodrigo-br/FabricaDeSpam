FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY WebApi/WebApi.csproj WebApi/
COPY Producer/Producer.csproj Producer/
RUN dotnet restore WebApi/WebApi.csproj
COPY . .
WORKDIR /src/WebApi
RUN dotnet build WebApi.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish WebApi.csproj -c Release -o /app/publish

FROM base AS final

WORKDIR /app
RUN mkdir /app/files
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "WebApi.dll" ]
