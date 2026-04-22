# Image de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier le fichier projet et restaurer les dépendances
COPY ["BioStockApi.csproj", "./"]
RUN dotnet restore

# Copier tout le reste et publier
COPY . .
RUN dotnet publish -c Release -o /app/out

# Image finale (plus légère)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Commande de lancement
ENTRYPOINT ["dotnet", "BioStockApi.dll"]