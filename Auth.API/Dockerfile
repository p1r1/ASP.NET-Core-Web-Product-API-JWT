# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# EF Tool
#RUN dotnet tool install --global dotnet-ef --version 9.0.8
#ENV PATH="$PATH:/root/.dotnet/tools"

# Copy project files
COPY ["Auth.API/Auth.API.csproj", "Auth.API/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Auth.API/Auth.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Auth.API"
RUN dotnet build "Auth.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Auth.API.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Just copy ef tool not entire SDKK
#COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
#ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=publish /app/publish .

# Environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 80
ENTRYPOINT ["dotnet", "Auth.API.dll"]