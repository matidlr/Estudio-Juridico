# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos el archivo de proyecto y restauramos dependencias
COPY *.csproj .
RUN dotnet restore

# Copiamos el resto del código y compilamos
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Etapa de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Puerto que expone la app
EXPOSE 8080

ENTRYPOINT ["dotnet", "EstudioJuridico.API2.dll"]