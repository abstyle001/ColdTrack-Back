# ============================================
# ColdTrack Dockerfile
# .NET 8 ASP.NET Core Web API
# ============================================

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore dependencies (cache layer)
COPY ["ColdTrack-Back.csproj", "./"]
RUN dotnet restore "ColdTrack-Back.csproj"

# Copy everything and publish
COPY . .
RUN dotnet publish "ColdTrack-Back.csproj" -c Release -o /app/publish --no-restore

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create Statics directory for avatar uploads
RUN mkdir -p /app/Statics

# Copy published output
COPY --from=build /app/publish .

# Expose port
EXPOSE 5194
ENV ASPNETCORE_URLS=http://+:5194
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DISABLE_HTTPS_REDIRECT=true

ENTRYPOINT ["dotnet", "ColdTrack-Back.dll"]