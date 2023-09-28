FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
RUN mkdir /app/files
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "WebApi.dll" ]
