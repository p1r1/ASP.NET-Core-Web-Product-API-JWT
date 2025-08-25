# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# EF Tool
RUN dotnet tool install --global dotnet-ef --version 9.0.8
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy project files
COPY ["Product.API/Product.API.csproj", "Product.API/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Product.API/Product.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Product.API"
RUN dotnet build "Product.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Product.API.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 80
ENTRYPOINT ["dotnet", "Product.API.dll"]