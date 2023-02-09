#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MigrationApplay/MigrationApplay.csproj", "MigrationApplay/"]
COPY ["src/LocalWarehouseApplication/ForMigrations/ForMigrations.csproj", "src/LocalWarehouseApplication/ForMigrations/"]
COPY ["src/LocalWarehouseApplication/Domain/Domain.csproj", "src/LocalWarehouseApplication/Domain/"]
RUN dotnet restore "MigrationApplay/MigrationApplay.csproj"
COPY . .
WORKDIR "/src/MigrationApplay"
RUN dotnet build "MigrationApplay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MigrationApplay.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MigrationApplay.dll"]