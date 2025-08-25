FROM mcr.microsoft.com/dotnet/sdk:9.0
RUN dotnet tool install --global dotnet-ef --version 9.0.8
ENV PATH="$PATH:/root/.dotnet/tools"
WORKDIR /src
