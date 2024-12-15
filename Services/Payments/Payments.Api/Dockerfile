FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5600
EXPOSE 5600

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Payments/Payments.Api/Payments.Api.csproj", "Services/Payments/Payments.Api/"]
RUN dotnet restore "Services/Payments/Payments.Api/Payments.Api.csproj"
COPY . .
WORKDIR "/src/Services/Payments/Payments.Api"
RUN dotnet build "Payments.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Payments.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.Api.dll"]
