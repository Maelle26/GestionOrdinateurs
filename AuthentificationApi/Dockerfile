FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AuthentificationApi/AuthentificationApi.csproj", "AuthentificationApi/"]
COPY ["GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj", "GestionOrdinateurs.Shared/"]
RUN dotnet restore "AuthentificationApi/AuthentificationApi.csproj"

COPY . .
WORKDIR "/src/AuthentificationApi"
RUN dotnet publish "AuthentificationApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5157
ENTRYPOINT ["dotnet", "AuthentificationApi.dll"]