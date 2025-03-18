# Etapa base com o runtime do .NET 7
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de build com o SDK do .NET 7
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia apenas o arquivo do projeto primeiro para melhorar o cache
COPY ["src/Loteria.API.csproj", "src/"]
RUN dotnet restore "src/Loteria.API.csproj"

# Copia o restante do código para dentro do container
COPY src/. src/
WORKDIR "/src"
RUN dotnet build "Loteria.API.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "Loteria.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final para executar a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Loteria.API.dll"]
