FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 443
EXPOSE 8080
EXPOSE 8081
EXPOSE 5044

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
ARG GITHUB_TOKEN
WORKDIR /src
COPY ["NotificationService.csproj", "./"]

RUN dotnet nuget add source https://nuget.pkg.github.com/androdenby/index.json -u androdenby -p ${GITHUB_TOKEN} --store-password-in-clear-text

RUN dotnet restore "NotificationService.csproj"
# IMPORTANT: Copy all project content (including the Templates folder) into the build stage
COPY . .
WORKDIR "/src/"
RUN dotnet build "NotificationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "NotificationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# -----------------------------------------------------------
# CRITICAL FIX: Explicitly copy the Templates folder
# This ensures the required .cshtml files exist at /app/Templates 
# where FluentEmail expects them. We copy them from the build stage (/src)
# -----------------------------------------------------------
COPY --from=build /src/Templates /app/Templates

ENTRYPOINT ["dotnet", "NotificationService.dll"]
