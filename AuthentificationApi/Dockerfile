# =========================
# 1. Étape de Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copie des fichiers projets et restauration
COPY AuthentificationApi/AuthentificationApi.csproj AuthentificationApi/
COPY GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj GestionOrdinateurs.Shared/
RUN dotnet restore AuthentificationApi/AuthentificationApi.csproj

# Copie de tout le code source et publication
COPY AuthentificationApi/ AuthentificationApi/
COPY GestionOrdinateurs.Shared/ GestionOrdinateurs.Shared/
RUN dotnet publish AuthentificationApi/AuthentificationApi.csproj -c Release -o /app/out

# =========================
# 2. Étape Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5157

ENTRYPOINT ["dotnet", "AuthentificationApi.dll"]