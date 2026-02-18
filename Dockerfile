# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files for caching
COPY *.sln ./
COPY src/Pharmacy.Api/*.csproj ./src/Pharmacy.Api/
COPY src/Pharmacy.Application/*.csproj ./src/Pharmacy.Application/
COPY src/Pharmacy.Domain/*.csproj ./src/Pharmacy.Domain/
COPY src/Pharmacy.Infrastructure/*.csproj ./src/Pharmacy.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build and publish API
WORKDIR /src/src/Pharmacy.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos "" appuser

# Copy published output
COPY --from=build /app/publish .

# Set environment
ENV ASPNETCORE_URLS=http://+:4000
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 4000

# Switch to non-root user
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:4000/api/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "Pharmacy.Api.dll"]
