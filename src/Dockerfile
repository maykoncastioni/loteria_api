# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Loteria.API/Loteria.API.csproj", "src/Loteria.API/"]
RUN dotnet restore "src/Loteria.API/Loteria.API.csproj"
COPY . .
WORKDIR "/src/src/Loteria.API"
RUN dotnet build "Loteria.API.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "Loteria.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Loteria.API.dll"]
