FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app
RUN mkdir /app/files
COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "Consumer.dll" ]
