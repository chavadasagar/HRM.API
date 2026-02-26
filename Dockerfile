FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /

# Copy csproj and restore as distinct layers
COPY ["HRM.API/Directory.Build.props", "/"]
COPY ["HRM.API/Directory.Build.targets", "/"]
COPY ["HRM.API/dotnet.ruleset", "/"]
COPY ["HRM.API/stylecop.json", "/"]
COPY ["HRM.API/src/Host/Host.csproj", "src/Host/"]
COPY ["HRM.API/src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["HRM.API/src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["HRM.API/src/Core/Shared/Shared.csproj", "src/Core/Shared/"]
COPY ["HRM.API/src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["HRM.API/src/Migrators/Migrators.MSSQL/Migrators.MSSQL.csproj", "src/Migrators/Migrators.MSSQL/"]
COPY ["HRM.API/src/Migrators/Migrators.MySQL/Migrators.MySQL.csproj", "src/Migrators/Migrators.MySQL/"]
COPY ["HRM.API/src/Migrators/Migrators.PostgreSQL/Migrators.PostgreSQL.csproj", "src/Migrators/Migrators.PostgreSQL/"]
COPY ["HRM.API/src/Migrators/Migrators.Oracle/Migrators.Oracle.csproj", "src/Migrators/Migrators.Oracle/"]

RUN dotnet restore "HRM.API/src/Host/Host.csproj" --disable-parallel

# Copy everything else and build
COPY . .
WORKDIR "HRM.API/src/Host"
RUN dotnet publish "Host.csproj" -c Release -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/publish .

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENV ASPNETCORE_URLS=https://+:5050;http://+:5060
EXPOSE 5050
EXPOSE 5060

ENTRYPOINT ["dotnet", "MasterPOS.API.Host.dll"]
