# Etapa 1: Imagem base do ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa 2: Imagem para build com o SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura as dependências
COPY ["src/Loteria.API/Loteria.API.csproj", "src/Loteria.API/"]
RUN dotnet restore "src/Loteria.API/Loteria.API.csproj"

# Copia todo o conteúdo do diretório src
COPY . .

# Define o diretório de trabalho e compila o projeto
WORKDIR "/src/src/Loteria.API"
RUN dotnet build "Loteria.API.csproj" -c Release -o /app/build

# Etapa 3: Publica o aplicativo
FROM build AS publish
RUN dotnet publish "Loteria.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 4: Configura a imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Loteria.API.dll"]
