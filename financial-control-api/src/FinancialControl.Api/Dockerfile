#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FinancialControl.Api/FinancialControl.Api.csproj", "src/FinancialControl.Api/"]
COPY ["src/FinancialControl.Application/FinancialControl.Application.csproj", "src/FinancialControl.Application/"]
COPY ["src/FinancialControl.Domain/FinancialControl.Domain.csproj", "src/FinancialControl.Domain/"]
COPY ["src/FinancialControl.Infra/FinancialControl.Infra.csproj", "src/FinancialControl.Infra/"]
COPY ["src/FinancialControl.IoC/FinancialControl.IoC.csproj", "src/FinancialControl.IoC/"]
RUN dotnet restore "./src/FinancialControl.Api/FinancialControl.Api.csproj"
COPY . .
WORKDIR "/src/src/FinancialControl.Api"
RUN dotnet build "./FinancialControl.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinancialControl.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialControl.Api.dll"]