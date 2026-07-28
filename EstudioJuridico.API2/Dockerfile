FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EstudioJuridico.API2/EstudioJuridico.API2.csproj", "EstudioJuridico.API2/"]
RUN dotnet restore "EstudioJuridico.API2/EstudioJuridico.API2.csproj"
COPY EstudioJuridico.API2/ EstudioJuridico.API2/
WORKDIR "/src/EstudioJuridico.API2"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EstudioJuridico.API2.dll"]