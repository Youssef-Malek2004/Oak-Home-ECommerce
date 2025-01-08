# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100
EXPOSE 5101

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files separately to leverage Docker caching
COPY ["Services/Users/Users.Api/Users.Api.csproj", "Services/Users/Users.Api/"]
COPY ["Services/Users/Users.Infrastructure/Users.Infrastructure.csproj", "Services/Users/Users.Infrastructure/"]
COPY ["Shared/Abstractions/Abstractions.csproj", "Shared/Abstractions/"]
COPY ["Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]

# Restore dependencies
RUN dotnet restore "Services/Users/Users.Api/Users.Api.csproj"

# Copy the rest of the files
COPY . .
WORKDIR "/src/Services/Users/Users.Api"

# Build the project
RUN dotnet build "Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Api.dll"]

