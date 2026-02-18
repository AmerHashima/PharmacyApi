# Use the official .NET 9 runtime as the base image for final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official .NET 9 SDK for build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
# Copy all project files to ensure proper dependency resolution
COPY ["src/Pharmacy.Api/Pharmacy.Api.csproj", "src/Pharmacy.Api/"]
COPY ["src/Pharmacy.Application/Pharmacy.Application.csproj", "src/Pharmacy.Application/"]
COPY ["src/Pharmacy.Infrastructure/Pharmacy.Infrastructure.csproj", "src/Pharmacy.Infrastructure/"]
COPY ["src/Pharmacy.Domain/Pharmacy.Domain.csproj", "src/Pharmacy.Domain/"]

# Restore dependencies for the main API project
RUN dotnet restore "src/Pharmacy.Api/Pharmacy.Api.csproj"

# Copy the entire source code
COPY . .

# Build the application
WORKDIR "/src/src/Pharmacy.Api"
RUN dotnet build "Pharmacy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pharmacy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Create a non-root user for security
RUN adduser --disabled-password --home /app --gecos '' appuser && chown -R appuser /app
USER appuser

# Add metadata labels
LABEL org.opencontainers.image.title="Pharmacy API"
LABEL org.opencontainers.image.description="Pharmacy Information System API built with .NET 9"
LABEL org.opencontainers.image.version="1.0.0"
LABEL org.opencontainers.image.source="https://github.com/AmerHashima/HisMed.git"
LABEL org.opencontainers.image.licenses="MIT"

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/api/health || exit 1

ENTRYPOINT ["dotnet", "Pharmacy.Api.dll"]