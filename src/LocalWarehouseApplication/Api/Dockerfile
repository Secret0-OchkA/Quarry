#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/LocalWarehouseApplication/Api/Api.csproj", "src/LocalWarehouseApplication/Api/"]
COPY ["src/LocalWarehouseApplication/Services/Services.csproj", "src/LocalWarehouseApplication/Services/"]
COPY ["src/LocalWarehouseApplication/Domain/Domain.csproj", "src/LocalWarehouseApplication/Domain/"]
COPY ["src/LocalWarehouseApplication/Infrastructura/Infrastructura.csproj", "src/LocalWarehouseApplication/Infrastructura/"]
RUN dotnet restore "src/LocalWarehouseApplication/Api/Api.csproj"
COPY . .
WORKDIR "/src/src/LocalWarehouseApplication/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]