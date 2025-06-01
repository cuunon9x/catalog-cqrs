# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["CatalogCQRS.sln", "./"]
COPY ["CatalogCQRS.Domain/*.csproj", "CatalogCQRS.Domain/"]
COPY ["CatalogCQRS.Application/*.csproj", "CatalogCQRS.Application/"]
COPY ["CatalogCQRS.Infrastructure/*.csproj", "CatalogCQRS.Infrastructure/"]
COPY ["CatalogCQRS.API/*.csproj", "CatalogCQRS.API/"]

# Restore NuGet packages
RUN dotnet restore

# Copy the source code
COPY . .

# Build and publish
RUN dotnet build -c Release
RUN dotnet publish -c Release -o /app --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Expose the port
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CatalogCQRS.API.dll"]
