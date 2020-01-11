## dotnet-project-template
asp.net core api 项目模板

template 文件夹中 content中内容不能乱移动，默认保持就行，模板其实就是一个简单的项目架子，可以添加自己的东西。


1. 打包
请确保电脑中又nuget.exe
```
# 打包成nuget
nuget pack Pluto.netcoreTemplate.nuspec 
```
会生成：Pluto.netcoreTemplate.0.0.4.nupkg  版本号可在nuspec中自定义
2. 安装
在刚刚生成的文件同目录执行下边命令
```
dotnet new -i Pluto.netcoreTemplate.0.0.4.nupkg  
```

3. 查看模板是否安装成功
```
dotnet new 
```
应该能看到名为pluto的模板


尝试示例：
```
dotnet new pluto -n demo -na demo -s demo -d demo

# 参数：-n dotnet命令自带，-na 命名空间名称  -s解决方案名称  -d efcore dbcontext 名称

```
