#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.azk8s.cn/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.azk8s.cn/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Demo.API/Demo.API.csproj", "src/Demo.API/"]
COPY ["src/Demo.Infrastructure/Demo.Infrastructure.csproj", "src/Demo.Infrastructure/"]
COPY ["src/Demo.Domain/Demo.Domain.csproj", "src/Demo.Domain/"]
COPY ["src/Demo.Application/Demo.Application.csproj", "src/Demo.Application/"]
RUN dotnet restore "src/Demo.API/Demo.API.csproj"
COPY . .
WORKDIR "/src/src/Demo.API"
RUN dotnet build "Demo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.API.dll"]
