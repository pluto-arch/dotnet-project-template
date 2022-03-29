#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/PlutoNetCoreTemplate.Mvc/PlutoNetCoreTemplate.Mvc.csproj", "src/PlutoNetCoreTemplate.Mvc/"]
RUN dotnet restore "src/PlutoNetCoreTemplate.Mvc/PlutoNetCoreTemplate.Mvc.csproj"
COPY . .
WORKDIR "/src/src/PlutoNetCoreTemplate.Mvc"
RUN dotnet build "PlutoNetCoreTemplate.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlutoNetCoreTemplate.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlutoNetCoreTemplate.Mvc.dll"]