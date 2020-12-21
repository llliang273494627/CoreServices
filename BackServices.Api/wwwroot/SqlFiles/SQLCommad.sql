
--使用说明,按照下面1,2,3,4步骤依次选中执行命令执行

--1.切换数据库
use [HTJCSysDB]
go

--2.远程数据库注册到本地
exec sp_addlinkedserver 'srv_lnk','','SQLOLEDB','192.168.140.105' --名称，sqloledb，远程数据库地址
exec sp_addlinkedsrvlogin 'srv_lnk','false','sa','sa','123456' --名称，false,本地用户名，远程用户名，远程用户密码
go

--3.执行导入表操作,自增长列不用插入
delete ProductInfo
insert ProductInfo  select [ProductType]
      ,[ProductCode]
      ,[ProductName]
      ,[FeatureIndex]
      ,[FeatureCode]
      ,[BarcodeCount]
      ,[PrintDate]
      ,[HaveSub]
      ,[Desc] from srv_lnk.HTJCSysDB.dbo.ProductInfo --导入表
go

--4.删除连接
exec sp_dropserver 'srv_lnk','droplogins'--若不再使用时，删除链接服务器
go