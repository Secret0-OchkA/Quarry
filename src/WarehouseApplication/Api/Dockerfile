#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WarehouseApplication/Api/Warehouse.Api.csproj", "src/WarehouseApplication/Api/"]
COPY ["src/WarehouseApplication/Services/Warehouse.Services.csproj", "src/WarehouseApplication/Services/"]
COPY ["src/WarehouseApplication/Domain/Warehouse.Domain.csproj", "src/WarehouseApplication/Domain/"]
COPY ["src/WarehouseApplication/Infrastructura/Warehouse.Infrastructura.csproj", "src/WarehouseApplication/Infrastructura/"]
RUN dotnet restore "src/WarehouseApplication/Api/Warehouse.Api.csproj"
COPY . .
WORKDIR "/src/src/WarehouseApplication/Api"
RUN dotnet build "Warehouse.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Warehouse.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Warehouse.Api.dll"]