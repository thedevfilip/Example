# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["Example.Migrations/Directory.Build.props", "Example.Migrations/"]
COPY ["Example.Api/Example.Api.csproj", "Example.Api/"]
COPY ["Example.Infrastructure/Example.Infrastructure.csproj", "Example.Infrastructure/"]
COPY ["Example.Domain/Example.Domain.csproj", "Example.Domain/"]
COPY ["Example.Migrations/Example.Migrations.csproj", "Example.Migrations/"]
RUN dotnet restore "./Example.Api/Example.Api.csproj"
COPY . .
WORKDIR "/src/Example.Api"
RUN dotnet build "./Example.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Example.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Example.Api.dll"]