FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj", "GestionOrdinateurs.Client/"]
COPY ["GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj", "GestionOrdinateurs.Shared/"]
RUN dotnet restore "GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj"

COPY . .
WORKDIR "/src/GestionOrdinateurs.Client"
RUN dotnet publish "GestionOrdinateurs.Client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY GestionOrdinateurs.Client/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 5000
CMD ["nginx", "-g", "daemon off;"]