#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.azk8s.cn/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.azk8s.cn/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src  # build 工作目录
COPY ["src/Pluto.netcoreTemplate.API/Pluto.netcoreTemplate.API.csproj", "src/Pluto.netcoreTemplate.API/"]
COPY ["src/Pluto.netcoreTemplate.Infrastructure/Pluto.netcoreTemplate.Infrastructure.csproj", "src/Pluto.netcoreTemplate.Infrastructure/"]
COPY ["src/Pluto.netcoreTemplate.Domain/Pluto.netcoreTemplate.Domain.csproj", "src/Pluto.netcoreTemplate.Domain/"]
COPY ["src/Pluto.netcoreTemplate.Application/Pluto.netcoreTemplate.Application.csproj", "src/Pluto.netcoreTemplate.Application/"]
RUN dotnet restore "src/Pluto.netcoreTemplate.API/Pluto.netcoreTemplate.API.csproj"
COPY . .  # 还原包以后将当前代码目录下的所有文件复制到 build 工作目录下
WORKDIR "/src/src/Pluto.netcoreTemplate.API"  # 切换工作目录到build 下的启动项目文件夹Pluto.netcoreTemplate.API
RUN dotnet build "Pluto.netcoreTemplate.API.csproj" -c Release -o /app/build   # 构建项目

FROM build AS publish 
RUN dotnet publish "Pluto.netcoreTemplate.API.csproj" -c Release -o /app/publish

FROM base AS final  # base的别名
WORKDIR /app  # 切换到最终目录下也就是base的工作目录
COPY --from=publish /app/publish .  # 从publish复制，复制publish 发布后的/app/publish 文件夹下的所有文件到当前final的工作目录
ENTRYPOINT ["dotnet", "Pluto.netcoreTemplate.API.dll"] # 执行启动

# 时区
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone