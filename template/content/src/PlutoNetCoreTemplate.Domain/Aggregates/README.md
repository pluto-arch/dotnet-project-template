# 如果使用dapper，需要使用dapper的一些attribute

## 1、指定表名
```chsarp
[Dapper.Table("tablename")]
```

## 2.指定主键
```csharp
// 默认是Id，

// 进行隐藏父类的定义，然后再使用特性
[Dapper.Key]
public new string Id { get;set; }
```

## 2. 忽略属性
```csharp
 [Dapper.IgnoreInsert,Dapper.IgnoreSelect,Dapper.IgnoreUpdate]
```
