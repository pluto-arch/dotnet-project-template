#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Pluto.netcoreTemplate.API/Pluto.netcoreTemplate.API.csproj", "src/Pluto.netcoreTemplate.API/"]
COPY ["src/Pluto.netcoreTemplate.Infrastructure/Pluto.netcoreTemplate.Infrastructure.csproj", "src/Pluto.netcoreTemplate.Infrastructure/"]
COPY ["src/Pluto.netcoreTemplate.Domain/Pluto.netcoreTemplate.Domain.csproj", "src/Pluto.netcoreTemplate.Domain/"]
COPY ["src/Pluto.netcoreTemplate.Application/Pluto.netcoreTemplate.Application.csproj", "src/Pluto.netcoreTemplate.Application/"]
RUN dotnet restore "src/Pluto.netcoreTemplate.API/Pluto.netcoreTemplate.API.csproj"
COPY . .
WORKDIR "/src/src/Pluto.netcoreTemplate.API"
RUN dotnet build "Pluto.netcoreTemplate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pluto.netcoreTemplate.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pluto.netcoreTemplate.API.dll"]
