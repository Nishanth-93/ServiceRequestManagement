#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ServiceRequestManagement.API/ServiceRequestManagement.API.csproj", "ServiceRequestManagement.API/"]
RUN dotnet restore "ServiceRequestManagement.API/ServiceRequestManagement.API.csproj"
COPY . .
WORKDIR "/src/ServiceRequestManagement.API"
RUN dotnet build "ServiceRequestManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceRequestManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceRequestManagement.API.dll"]