FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base

WORKDIR /app

# change sdk to aspnet in prodution
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY Producer/Producer.csproj Producer/
RUN dotnet restore Producer/Producer.csproj
COPY . .
WORKDIR /src/Producer
RUN dotnet build Producer.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Producer.csproj -c Release -o /app/publish

FROM base AS final

WORKDIR /app
RUN mkdir /app/files
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Producer.dll" ]