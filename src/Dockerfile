FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia apenas o arquivo do projeto para aproveitar melhor o cache.
COPY ["src/Loteria.API.csproj", "."]
RUN dotnet restore "Loteria.API.csproj"

# Copia o restante dos arquivos e compila o projeto.
COPY src/. .

RUN dotnet build "Loteria.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Loteria.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
