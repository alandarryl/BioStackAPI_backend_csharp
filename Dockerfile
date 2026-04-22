# 1. ÉTAPE DE RÉCUPÉRATION DU SDK (BUILD)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie du fichier projet et restauration des packages NuGet
# (On fait ça en premier pour profiter du cache Docker)
COPY *.csproj ./
RUN dotnet restore

# Copie de tout le reste du code et compilation
COPY . ./
RUN dotnet publish -c Release -o out

# 2. ÉTAPE DE RUNTIME (EXÉCUTION)
# On utilise une image beaucoup plus petite qui ne contient pas le SDK
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# On récupère uniquement les fichiers compilés de l'étape précédente
COPY --from=build /app/out .

# On s'assure que le fichier SQLite peut être écrit par l'application
# (Important pour éviter les erreurs de permission "Read-only")
USER root
RUN chmod -R 777 /app

# Commande pour démarrer l'API
# REMPLACE "BioStockApi.dll" par le nom exact de ton fichier .dll 
# si ton projet s'appelle différemment.
ENTRYPOINT ["dotnet", "BioStockApi.dll"]