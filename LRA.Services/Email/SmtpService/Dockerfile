FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LRA.Services/Email/SmtpService/LRA.Email.SmtpService.csproj", "LRA.Services/Email/SmtpService/"]
RUN dotnet restore "./LRA.Services/Email/SmtpService/LRA.Email.SmtpService.csproj"
COPY . .
WORKDIR "/src/LRA.Services/Email/SmtpService"
RUN dotnet build "./LRA.Email.SmtpService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LRA.Email.SmtpService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LRA.Email.SmtpService.dll"]
