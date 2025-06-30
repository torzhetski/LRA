FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LRA.Services/Auth/Presentation/LRA.Auth.Presentation.csproj", "LRA.Services/Auth/Presentation/"]
COPY ["LRA.Shared/Abstractions/LRA.Shared.Abstractions.csproj", "LRA.Shared/Abstractions/"]
COPY ["LRA.Shared/Infrastructure/LRA.Shared.Infrastructure.csproj", "LRA.Shared/Infrastructure/"]
COPY ["LRA.Shared/Presentation/LRA.Shared.Presentation.csproj", "LRA.Shared/Presentation/"]
COPY ["LRA.Shared/Utils/LRA.Shared.Utils.csproj", "LRA.Shared/Utils/"]
COPY ["LRA.Services/Auth/Application/LRA.Auth.Application.csproj", "LRA.Services/Auth/Application/"]
COPY ["LRA.Services/Auth/Domain/LRA.Auth.Domain.csproj", "LRA.Services/Auth/Domain/"]
COPY ["LRA.Services/Auth/Infrastructure/Persistence/LRA.Auth.Infrasturcture.Persistence.csproj", "LRA.Services/Auth/Infrastructure/Persistence/"]
RUN dotnet restore "./LRA.Services/Auth/Presentation/LRA.Auth.Presentation.csproj"
COPY . .
WORKDIR "/src/LRA.Services/Auth/Presentation"
RUN dotnet build "./LRA.Auth.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LRA.Auth.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LRA.Auth.Presentation.dll"]
