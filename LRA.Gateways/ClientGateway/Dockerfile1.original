FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LRA.Gateways/ClientGateway/LRA.Gateways.Client.csproj", "LRA.Gateways/ClientGateway/"]
COPY ["LRA.Shared/Abstractions/LRA.Shared.Abstractions.csproj", "LRA.Shared/Abstractions/"]
COPY ["LRA.Shared/Infrastructure/LRA.Shared.Infrastructure.csproj", "LRA.Shared/Infrastructure/"]
RUN dotnet restore "./LRA.Gateways/ClientGateway/LRA.Gateways.Client.csproj"
COPY . .
WORKDIR "/src/LRA.Gateways/ClientGateway"
RUN dotnet build "./LRA.Gateways.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LRA.Gateways.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LRA.Gateways.Client.dll"]
