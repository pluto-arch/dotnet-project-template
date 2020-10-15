#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/PlutoNetCoreTemplate/PlutoNetCoreTemplate.csproj", "src/PlutoNetCoreTemplate/"]
COPY ["src/PlutoNetCoreTemplate.Infrastructure/PlutoNetCoreTemplate.Infrastructure.csproj", "src/PlutoNetCoreTemplate.Infrastructure/"]
COPY ["src/PlutoNetCoreTemplate.Domain/PlutoNetCoreTemplate.Domain.csproj", "src/PlutoNetCoreTemplate.Domain/"]
COPY ["src/PlutoNetCoreTemplate.Application/PlutoNetCoreTemplate.Application.csproj", "src/PlutoNetCoreTemplate.Application/"]
RUN dotnet restore "src/PlutoNetCoreTemplate/PlutoNetCoreTemplate.csproj"
COPY . .
WORKDIR "/src/src/PlutoNetCoreTemplate"
RUN dotnet build "PlutoNetCoreTemplate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlutoNetCoreTemplate.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlutoNetCoreTemplate.dll"]
