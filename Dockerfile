FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy props/targets
COPY ["HRM.API/Directory.Build.props", "."]
COPY ["HRM.API/Directory.Build.targets", "."]
COPY ["HRM.API/dotnet.ruleset", "."]
COPY ["HRM.API/stylecop.json", "."]

# Copy project files
COPY ["HRM.API/src/Host/Host.csproj", "Host/"]
COPY ["HRM.API/src/Core/Application/Application.csproj", "Core/Application/"]
COPY ["HRM.API/src/Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["HRM.API/src/Core/Shared/Shared.csproj", "Core/Shared/"]
COPY ["HRM.API/src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["HRM.API/src/Migrators/Migrators.MSSQL/Migrators.MSSQL.csproj", "Migrators/Migrators.MSSQL/"]
COPY ["HRM.API/src/Migrators/Migrators.MySQL/Migrators.MySQL.csproj", "Migrators/Migrators.MySQL/"]
COPY ["HRM.API/src/Migrators/Migrators.PostgreSQL/Migrators.PostgreSQL.csproj", "Migrators/Migrators.PostgreSQL/"]
COPY ["HRM.API/src/Migrators/Migrators.Oracle/Migrators.Oracle.csproj", "Migrators/Migrators.Oracle/"]

# Restore (correct path)
RUN dotnet restore "Host/Host.csproj" --disable-parallel

# Copy everything else
COPY HRM.API/src/. .

# Publish
WORKDIR /src/Host
RUN dotnet publish "Host.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/publish .

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENV ASPNETCORE_URLS=https://+:5050;http://+:5060
EXPOSE 5050
EXPOSE 5060

ENTRYPOINT ["dotnet", "MasterPOS.API.Host.dll"]
