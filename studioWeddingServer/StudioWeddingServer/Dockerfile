# ==========================================
# Multi-stage Dockerfile for .NET 8 Web API
# Studio Wedding Backend
# ==========================================

# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies (layer caching)
COPY ["StudioWeddingServer.csproj", "./"]
RUN dotnet restore "./StudioWeddingServer.csproj"

# Copy entire source code and build
COPY . .
WORKDIR "/src"
RUN dotnet build "StudioWeddingServer.csproj" -c Release -o /app/build

# Stage 2: Publish stage
FROM build AS publish
RUN dotnet publish "StudioWeddingServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render exposes PORT 8080 by default
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "StudioWeddingServer.dll"]
