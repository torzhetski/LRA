FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LRA.Services/Subscription/Presentation/LRA.Subscription.Presentation.csproj", "LRA.Services/Subscription/Presentation/"]
COPY ["LRA.Shared/Abstractions/LRA.Shared.Abstractions.csproj", "LRA.Shared/Abstractions/"]
COPY ["LRA.Shared/Infrastructure/LRA.Shared.Infrastructure.csproj", "LRA.Shared/Infrastructure/"]
COPY ["LRA.Shared/Presentation/LRA.Shared.Presentation.csproj", "LRA.Shared/Presentation/"]
COPY ["LRA.Shared/Utils/LRA.Shared.Utils.csproj", "LRA.Shared/Utils/"]
COPY ["LRA.Services/Subscription/Application/LRA.Subscription.Application.csproj", "LRA.Services/Subscription/Application/"]
COPY ["LRA.Services/Subscription/Domain/LRA.Subscription.Domain.csproj", "LRA.Services/Subscription/Domain/"]
COPY ["LRA.Services/Subscription/Infrastructure/Persistence/LRA.Subscription.Infrasturcture.Persistence.csproj", "LRA.Services/Subscription/Infrastructure/Persistence/"]
RUN dotnet restore "./LRA.Services/Subscription/Presentation/LRA.Subscription.Presentation.csproj"
COPY . .
WORKDIR "/src/LRA.Services/Subscription/Presentation"
RUN dotnet build "./LRA.Subscription.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LRA.Subscription.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LRA.Subscription.Presentation.dll"]
