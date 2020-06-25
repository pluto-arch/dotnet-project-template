#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.azk8s.cn/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.azk8s.cn/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src  # build 工作目录
COPY ["src/Demo.API/Demo.API.csproj", "src/Demo.API/"]
COPY ["src/Demo.Infrastructure/Demo.Infrastructure.csproj", "src/Demo.Infrastructure/"]
COPY ["src/Demo.Domain/Demo.Domain.csproj", "src/Demo.Domain/"]
COPY ["src/Demo.Application/Demo.Application.csproj", "src/Demo.Application/"]
RUN dotnet restore "src/Demo.API/Demo.API.csproj"
COPY . .  # 还原包以后将当前代码目录下的所有文件复制到 build 工作目录下
WORKDIR "/src/src/Demo.API"  # 切换工作目录到build 下的启动项目文件夹Demo.API
RUN dotnet build "Demo.API.csproj" -c Release -o /app/build   # 构建项目

FROM build AS publish 
RUN dotnet publish "Demo.API.csproj" -c Release -o /app/publish

FROM base AS final  # base的别名
WORKDIR /app  # 切换到最终目录下也就是base的工作目录
COPY --from=publish /app/publish .  # 从publish复制，复制publish 发布后的/app/publish 文件夹下的所有文件到当前final的工作目录
ENTRYPOINT ["dotnet", "Demo.API.dll"] # 执行启动

# 时区
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone