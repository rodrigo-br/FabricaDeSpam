FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
ENV PATH="${PATH}:/root/.dotnet/tools"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY WebApi/WebApi.csproj WebApi/
COPY Producer/Producer.csproj Producer/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Domain/Domain.csproj Domain/
RUN dotnet restore WebApi/WebApi.csproj && dotnet tool install --global dotnet-ef
COPY . .
WORKDIR /src/WebApi
RUN dotnet build WebApi.csproj -c Release -o /app/build

FROM build AS publish
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish WebApi.csproj -c Release -o /app/publish

FROM base AS final

WORKDIR /app
RUN mkdir /app/files
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "WebApi.dll" ]
