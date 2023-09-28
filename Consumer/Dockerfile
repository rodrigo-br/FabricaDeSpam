FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /appcsharp

COPY *.csproj .

ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet restore && dotnet tool install --global dotnet-ef
RUN mkdir /appcsharp/files

COPY . .
