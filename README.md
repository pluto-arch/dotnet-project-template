## dotnet-project-template
asp.net core api 项目模板

template 文件夹中 content中内容不能乱移动，默认保持就行，模板其实就是一个简单的项目架子，可以添加自己的东西。

> github 时而推送不上去，新的更新会先放在gitee上。地址：https://gitee.com/zyllbx/dotnet-project-template


1. 打包
请确保电脑中有nuget.exe
```
# 打包成nuget
nuget pack Pluto.netcoreTemplate.nuspec 
```
会生成：PlutoNetCoreTemplate.{version}.nupkg  版本号可在nuspec中自定义
2. 安装
在刚刚生成的文件同目录执行下边命令
```
dotnet new -i PlutoNetCoreTemplate.1.3.2.nupkg  
```

3. 查看模板是否安装成功
```
dotnet new plutoapi -h
```
应该能看到名为pluto的模板


尝试示例：
```
dotnet new plutoapi -n Demo  -d Demo

# 参数：-n 名称（有值会新建一个文件夹输出项目），  -d efcore dbcontext 名称[可选]

```
