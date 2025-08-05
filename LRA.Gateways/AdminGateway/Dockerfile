FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LRA.Gateways/AdminGateway/LRA.Gateways.AdminGateway.csproj", "LRA.Gateways/AdminGateway/"]
COPY ["LRA.Shared/Abstractions/LRA.Shared.Abstractions.csproj", "LRA.Shared/Abstractions/"]
COPY ["LRA.Shared/Utils/LRA.Shared.Utils.csproj", "LRA.Shared/Utils/"]
COPY ["LRA.Shared/Infrastructure/LRA.Shared.Infrastructure.csproj", "LRA.Shared/Infrastructure/"]
COPY ["LRA.Shared/Presentation/LRA.Shared.Presentation.csproj", "LRA.Shared/Presentation/"]
RUN dotnet restore "./LRA.Gateways/AdminGateway/LRA.Gateways.AdminGateway.csproj"
COPY . .
WORKDIR "/src/LRA.Gateways/AdminGateway"
RUN dotnet build "./LRA.Gateways.AdminGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LRA.Gateways.AdminGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LRA.Gateways.AdminGateway.dll"]