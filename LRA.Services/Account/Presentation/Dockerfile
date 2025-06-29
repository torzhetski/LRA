FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LRA.Services/Account/Presentation/LRA.Account.Presentation.csproj", "LRA.Services/Account/Presentation/"]
COPY ["LRA.Shared/Abstractions/LRA.Shared.Abstractions.csproj", "LRA.Shared/Abstractions/"]
COPY ["LRA.Shared/Infrastructure/LRA.Shared.Infrastructure.csproj", "LRA.Shared/Infrastructure/"]
COPY ["LRA.Shared/Presentation/LRA.Shared.Presentation.csproj", "LRA.Shared/Presentation/"]
COPY ["LRA.Shared/Utils/LRA.Shared.Utils.csproj", "LRA.Shared/Utils/"]
COPY ["LRA.Services/Account/Application/LRA.Account.Application.csproj", "LRA.Services/Account/Application/"]
COPY ["LRA.Services/Account/Domain/LRA.Account.Domain.csproj", "LRA.Services/Account/Domain/"]
COPY ["LRA.Services/Account/Infrastructure/Persistence/LRA.Account.Infrasturcture.Persistence.csproj", "LRA.Services/Account/Infrastructure/Persistence/"]
RUN dotnet restore "./LRA.Services/Account/Presentation/LRA.Account.Presentation.csproj"
COPY . .
WORKDIR "/src/LRA.Services/Account/Presentation"
RUN dotnet build "./LRA.Account.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LRA.Account.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LRA.Account.Presentation.dll"]