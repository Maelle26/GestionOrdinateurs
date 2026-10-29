FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restauration
COPY ["GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj", "GestionOrdinateurs.Gateway/"]
RUN dotnet restore "GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj"

# Copie et publication vers /app/publish
COPY . .
WORKDIR "/src/GestionOrdinateurs.Gateway"
RUN dotnet publish "GestionOrdinateurs.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Image runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5010
ENTRYPOINT ["dotnet", "GestionOrdinateurs.Gateway.dll"]