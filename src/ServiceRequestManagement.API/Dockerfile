#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/ServiceRequestManagement.API/ServiceRequestManagement.API.csproj", "src/ServiceRequestManagement.API/"]
COPY ["src/ServiceRequestManagement.Infrastructure/ServiceRequestManagement.Infrastructure.csproj", "src/ServiceRequestManagement.Infrastructure/"]
COPY ["src/ServiceRequestManagement.Domain/ServiceRequestManagement.Domain.csproj", "src/ServiceRequestManagement.Domain/"]
RUN dotnet restore "src/ServiceRequestManagement.API/ServiceRequestManagement.API.csproj"
COPY . .
WORKDIR "/src/src/ServiceRequestManagement.API"
RUN dotnet build "ServiceRequestManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceRequestManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceRequestManagement.API.dll"]