
--ʹ��˵��,��������1,2,3,4��������ѡ��ִ������ִ��

--1.�л����ݿ�
use [HTJCSysDB]
go

--2.Զ�����ݿ�ע�ᵽ����
exec sp_addlinkedserver 'srv_lnk','','SQLOLEDB','192.168.140.105' --���ƣ�sqloledb��Զ�����ݿ��ַ
exec sp_addlinkedsrvlogin 'srv_lnk','false','sa','sa','123456' --���ƣ�false,�����û�����Զ���û�����Զ���û�����
go

--3.ִ�е�������,�������в��ò���
delete ProductInfo
insert ProductInfo  select [ProductType]
      ,[ProductCode]
      ,[ProductName]
      ,[FeatureIndex]
      ,[FeatureCode]
      ,[BarcodeCount]
      ,[PrintDate]
      ,[HaveSub]
      ,[Desc] from srv_lnk.HTJCSysDB.dbo.ProductInfo --�����
go

--4.ɾ������
exec sp_dropserver 'srv_lnk','droplogins'--������ʹ��ʱ��ɾ�����ӷ�����
go