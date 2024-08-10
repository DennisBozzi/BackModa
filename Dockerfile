FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Back.csproj", "."]
RUN dotnet restore "./Back.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Back.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Back.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["wwwroot/css/swaggerDark.css", "wwwroot/css/swaggerDark.css"]

RUN if [ -f .env ]; then cp .env /app/.env; fi

ENTRYPOINT ["dotnet", "Back.dll"]