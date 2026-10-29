FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["GestionOrdinateurs.API/GestionOrdinateurs.API.csproj", "GestionOrdinateurs.API/"]
COPY ["GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj", "GestionOrdinateurs.Shared/"]
RUN dotnet restore "GestionOrdinateurs.API/GestionOrdinateurs.API.csproj"

COPY . .
WORKDIR "/src/GestionOrdinateurs.API"
RUN dotnet publish "GestionOrdinateurs.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 7001
ENTRYPOINT ["dotnet", "GestionOrdinateurs.API.dll"]