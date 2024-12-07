FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5500
EXPOSE 5501

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Notifications/Notifications.Api/Notifications.Api.csproj", "Services/Notifications/Notifications.Api/"]
RUN dotnet restore "Services/Notifications/Notifications.Api/Notifications.Api.csproj"
COPY . .
WORKDIR "/src/Services/Notifications/Notifications.Api"
RUN dotnet build "Notifications.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Notifications.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.Api.dll"]
