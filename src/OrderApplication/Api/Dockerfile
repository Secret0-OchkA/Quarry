#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/OrderApplication/Api/Order.Api.csproj", "src/OrderApplication/Api/"]
COPY ["src/OrderApplication/Infrastructura/Order.Infrastructura.csproj", "src/OrderApplication/Infrastructura/"]
COPY ["src/OrderApplication/Domain/Order.Domain.csproj", "src/OrderApplication/Domain/"]
COPY ["src/OrderApplication/Services/Order.Services.csproj", "src/OrderApplication/Services/"]
RUN dotnet restore "src/OrderApplication/Api/Order.Api.csproj"
COPY . .
WORKDIR "/src/src/OrderApplication/Api"
RUN dotnet build "Order.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Api.dll"]