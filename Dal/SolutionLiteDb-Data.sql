USE [D:\SVN\Base\branches\��������\03_��Ŀ����\06_Դ����\SolutionLite\DAL\SOLUTIONLITEDB.MDF]
--alter database [D:\SVN\BASE\BRANCHES\��������\03_��Ŀ����\06_Դ����\SOLUTIONLITE\DAL\SOLUTIONLITEDB.MDF]  set multi_user; 
--USE SOLUTIONLITEDB
GO
 --DUMP TRANSACTION [SOLUTIONLITEDB] WITH NO_LOG 
 --BACKUP LOG [SOLUTIONLITEDB] WITH NO_LOG 
 --DBCC SHRINKDATABASE([SOLUTIONLITEDB])
 --ɾ������
delete from[dbo].[RoleModule]
delete from[dbo].[Module]
delete from[dbo].[UserRole]
delete from[dbo].[User]
delete from[dbo].[Role]
--��ɫ
INSERT INTO [dbo].[Role]([Id],[Name],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01000000-0000-0000-0000-000000000000',N'����Ա��ɫ',getdate(),'admin',getdate(),'admin',null)
GO

--�û�                                           
INSERT INTO [dbo].[User]([Id],[Name],[NickName],[SecurityPassword],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01000000-0000-0000-0000-000000000000',N'admin',N'����Ա',N'RQPQJfkUoSYpwG7HCKXBN5o5aidPbtjGJd/JNDhJqYM=',getdate(),'admin',getdate(),'admin',null)
GO

--�û���ɫ                                           
INSERT INTO [dbo].[UserRole]([Id],[UserId],[RoleId],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01000000-0000-0000-0000-000000000000','01000000-0000-0000-0000-000000000000','01000000-0000-0000-0000-000000000000',getdate(),'admin',getdate(),'admin',null)
GO

--ϵͳ����                                                                                                                                                                           GUIDÿ��λ�ָһ����ϵͳ����01���������ݹ���02��������ϵͳ��������Ľ�ɫ����0101��ϵͳ����������û��������0102��
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01000000-0000-0000-0000-000000000000',N'ϵͳ����',1,N'00000000-0000-0000-0000-000000000000',1,'pack://application:,,,/Desktop.Resource;component/Images/SystemManagement_32.png',NULL,NULL,GETDATE(),'admin',getdate(),'admin',null)
--ģ�����                                                                                                                                                                           GUIDÿ��λ�ָһ����ϵͳ����01���������ݹ���02��������ϵͳ��������Ľ�ɫ����0101��ϵͳ����������û��������0102��
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01030000-0000-0000-0000-000000000000',N'ģ�����',1,N'01000000-0000-0000-0000-000000000000',1.1,'pack://application:,,,/Desktop.Resource;component/Images/Module_32.png','Desktop.ModuleModule','Desktop.ModuleModule.Views.ModuleView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01030100-0000-0000-0000-000000000000',N'�½�ģ��',0,N'01030000-0000-0000-0000-000000000000',1.11,NULL,'Desktop.ModuleModule','Desktop.ModuleModule.Views.ModuleAddView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01030200-0000-0000-0000-000000000000',N'�༭ģ��',0,N'01030000-0000-0000-0000-000000000000',1.12,NULL,'Desktop.ModuleModule','Desktop.ModuleModule.Views.ModuleEditView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01030300-0000-0000-0000-000000000000',N'ɾ��ģ��',0,N'01030000-0000-0000-0000-000000000000',1.13,NULL,'Desktop.ModuleModule','Desktop.ModuleModule.Views.ModuleDeleteView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01030400-0000-0000-0000-000000000000',N'����ģ��',0,N'01030000-0000-0000-0000-000000000000',1.14,NULL,'Desktop.ModuleModule','Desktop.ModuleModule.Views.ModuleImportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01030500-0000-0000-0000-000000000000',N'����ģ��',0,N'01030000-0000-0000-0000-000000000000',1.15,NULL,'Desktop.ModuleModule','Desktop.ModuleModule.Views.ModuleExportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01030600-0000-0000-0000-000000000000',N'��ӡģ��',0,N'01030000-0000-0000-0000-000000000000',1.16,NULL,'Desktop.ModuleModule','Desktop.ModuleModule.Views.ModulePrintView',getdate(),'admin',getdate(),'admin',null)


--��ɫ����                                                                                                                                                                           GUIDÿ��λ�ָһ����ϵͳ����01���������ݹ���02��������ϵͳ��������Ľ�ɫ����0101��ϵͳ����������û��������0102��
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01010000-0000-0000-0000-000000000000',N'��ɫ����',1,N'01000000-0000-0000-0000-000000000000',1.2,'pack://application:,,,/Desktop.Resource;component/Images/Role_32.png','Desktop.RoleModule','Desktop.RoleModule.Views.RoleView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01010100-0000-0000-0000-000000000000',N'�½���ɫ',0,N'01010000-0000-0000-0000-000000000000',1.21,NULL,'Desktop.RoleModule','Desktop.RoleModule.Views.RoleAddView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01010200-0000-0000-0000-000000000000',N'�༭��ɫ',0,N'01010000-0000-0000-0000-000000000000',1.22,NULL,'Desktop.RoleModule','Desktop.RoleModule.Views.RoleEditView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01010300-0000-0000-0000-000000000000',N'ɾ����ɫ',0,N'01010000-0000-0000-0000-000000000000',1.23,NULL,'Desktop.RoleModule','Desktop.RoleModule.Views.RoleDeleteView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01010400-0000-0000-0000-000000000000',N'�����ɫ',0,N'01010000-0000-0000-0000-000000000000',1.24,NULL,'Desktop.RoleModule','Desktop.RoleModule.Views.RoleImportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01010500-0000-0000-0000-000000000000',N'������ɫ',0,N'01010000-0000-0000-0000-000000000000',1.25,NULL,'Desktop.RoleModule','Desktop.RoleModule.Views.RoleExportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01010600-0000-0000-0000-000000000000',N'��ӡ��ɫ',0,N'01010000-0000-0000-0000-000000000000',1.26,NULL,'Desktop.RoleModule','Desktop.RoleModule.Views.RolePrintView',getdate(),'admin',getdate(),'admin',null)
--�û�����                                                                                                                                                                           GUIDÿ��λ�ָһ����ϵͳ����01���������ݹ���02��������ϵͳ��������Ľ�ɫ����0101��ϵͳ����������û��������0102��
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01020000-0000-0000-0000-000000000000',N'�û�����',1,N'01000000-0000-0000-0000-000000000000',1.3,'pack://application:,,,/Desktop.Resource;component/Images/User_32.png','Desktop.UserModule','Desktop.UserModule.Views.UserView',GETDATE(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01020100-0000-0000-0000-000000000000',N'�½��û�',0,N'01020000-0000-0000-0000-000000000000',1.31,NULL,'Desktop.UserModule','Desktop.UserModule.Views.UserAddView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01020200-0000-0000-0000-000000000000',N'�༭�û�',0,N'01020000-0000-0000-0000-000000000000',1.32,NULL,'Desktop.UserModule','Desktop.UserModule.Views.UserEditView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01020300-0000-0000-0000-000000000000',N'ɾ���û�',0,N'01020000-0000-0000-0000-000000000000',1.33,NULL,'Desktop.UserModule','Desktop.UserModule.Views.UserDeleteView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01020400-0000-0000-0000-000000000000',N'�����û�',0,N'01020000-0000-0000-0000-000000000000',1.34,NULL,'Desktop.UserModule','Desktop.UserModule.Views.UserImportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01020500-0000-0000-0000-000000000000',N'�����û�',0,N'01020000-0000-0000-0000-000000000000',1.35,NULL,'Desktop.UserModule','Desktop.UserModule.Views.UserExportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01020600-0000-0000-0000-000000000000',N'��ӡ�û�',0,N'01020000-0000-0000-0000-000000000000',1.36,NULL,'Desktop.UserModule','Desktop.UserModule.Views.UserPrintView',getdate(),'admin',getdate(),'admin',null)


--OPC����������
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01040000-0000-0000-0000-000000000000',N'OPC����������',1,N'01000000-0000-0000-0000-000000000000',1.4,'pack://application:,,,/Desktop.Resource;component/Images/UA_32.png','Desktop.OpcModule','Desktop.OpcModule.Views.OpcServerView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01040100-0000-0000-0000-000000000000',N'�½�OPC������',0,N'01040000-0000-0000-0000-000000000000',1.41,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.OpcServerAddView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01040200-0000-0000-0000-000000000000',N'�༭OPC������',0,N'01040000-0000-0000-0000-000000000000',1.42,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.OpcServerEditView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01040300-0000-0000-0000-000000000000',N'ɾ��OPC������',0,N'01040000-0000-0000-0000-000000000000',1.43,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.OpcServerDeleteView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01040400-0000-0000-0000-000000000000',N'����OPC������',0,N'01040000-0000-0000-0000-000000000000',1.44,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.OpcServerImportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01040500-0000-0000-0000-000000000000',N'����OPC������',0,N'01040000-0000-0000-0000-000000000000',1.45,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.OpcServerExportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01040600-0000-0000-0000-000000000000',N'��ӡOPC������',0,N'01040000-0000-0000-0000-000000000000',1.46,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.OpcServerPrintView',getdate(),'admin',getdate(),'admin',null)

--�ڵ����
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01050000-0000-0000-0000-000000000000',N'�ڵ����',1,N'01000000-0000-0000-0000-000000000000',1.5,'pack://application:,,,/Desktop.Resource;component/Images/DeviceNode_32.png','Desktop.OpcModule','Desktop.OpcModule.Views.DeviceNodeView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01050100-0000-0000-0000-000000000000',N'�½��ڵ�',0,N'01050000-0000-0000-0000-000000000000',1.51,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.DeviceNodeAddView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01050200-0000-0000-0000-000000000000',N'�༭�ڵ�',0,N'01050000-0000-0000-0000-000000000000',1.52,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.DeviceNodeEditView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01050300-0000-0000-0000-000000000000',N'ɾ���ڵ�',0,N'01050000-0000-0000-0000-000000000000',1.53,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.DeviceNodeDeleteView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01050400-0000-0000-0000-000000000000',N'����ڵ�',0,N'01050000-0000-0000-0000-000000000000',1.54,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.DeviceNodeImportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01050500-0000-0000-0000-000000000000',N'�����ڵ�',0,N'01050000-0000-0000-0000-000000000000',1.55,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.DeviceNodeExportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01050600-0000-0000-0000-000000000000',N'��ӡ�ڵ�',0,N'01050000-0000-0000-0000-000000000000',1.56,NULL,'Desktop.OpcModule','Desktop.OpcModule.Views.DeviceNodePrintView',getdate(),'admin',getdate(),'admin',null)


--��־����
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01060000-0000-0000-0000-000000000000',N'ϵͳ��־',1,N'01000000-0000-0000-0000-000000000000',1.6,'pack://application:,,,/Desktop.Resource;component/Images/Log_32.png','Desktop.AppLogModule','Desktop.AppLogModule.Views.AppLogView',getdate(),'admin',getdate(),'admin',null)

--�������                                                                                                                                                                           GUIDÿ��λ�ָһ����ϵͳ����01���������ݹ���02��������ϵͳ��������Ľ�ɫ����0101��ϵͳ����������û��������0102��
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01070000-0000-0000-0000-000000000000',N'�������',1,N'01000000-0000-0000-0000-000000000000',1.7,'pack://application:,,,/Desktop.Resource;component/Images/Module_32.png','Desktop.NoticeModule','Desktop.NoticeModule.Views.NoticeView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01070100-0000-0000-0000-000000000000',N'�½�����',0,N'01070000-0000-0000-0000-000000000000',1.71,NULL,'Desktop.NoticeModule','Desktop.NoticeModule.Views.NoticeAddView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01070200-0000-0000-0000-000000000000',N'�༭����',0,N'01070000-0000-0000-0000-000000000000',1.72,NULL,'Desktop.NoticeModule','Desktop.NoticeModule.Views.NoticeEditView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01070300-0000-0000-0000-000000000000',N'ɾ������',0,N'01070000-0000-0000-0000-000000000000',1.73,NULL,'Desktop.NoticeModule','Desktop.NoticeModule.Views.NoticeDeleteView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01070400-0000-0000-0000-000000000000',N'���빫��',0,N'01070000-0000-0000-0000-000000000000',1.74,NULL,'Desktop.NoticeModule','Desktop.NoticeModule.Views.NoticeImportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01070500-0000-0000-0000-000000000000',N'��������',0,N'01070000-0000-0000-0000-000000000000',1.75,NULL,'Desktop.NoticeModule','Desktop.NoticeModule.Views.NoticeExportView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01070600-0000-0000-0000-000000000000',N'��ӡ����',0,N'01070000-0000-0000-0000-000000000000',1.76,NULL,'Desktop.NoticeModule','Desktop.NoticeModule.Views.NoticePrintView',getdate(),'admin',getdate(),'admin',null)

--�������ݹ���															 
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('02000000-0000-0000-0000-000000000000',N'�������ݹ���',1,N'00000000-0000-0000-0000-000000000000',2,'pack://application:,,,/Desktop.Resource;component/Images/BaseData_32.png',NULL,NULL,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('02010000-0000-0000-0000-000000000000',N'�豸����',1,N'02000000-0000-0000-0000-000000000000',2.1,'pack://application:,,,/Desktop.Resource;component/Images/Equipment_32.png','Desktop.EquipmentModule','Desktop.EquipmentModule.Views.EquipmentView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('02020000-0000-0000-0000-000000000000',N'�������',1,N'02000000-0000-0000-0000-000000000000',2.2,'pack://application:,,,/Desktop.Resource;component/Images/Logo_16.ico','Desktop.WorkCellModule','Desktop.WorkCellModule.Views.WorkCellView',getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[Module]([Id],[Name],[ShowInNavigateMenu],[ParentId],[OrderCode],[Icon],[AssemblyName],[ViewName],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('02030000-0000-0000-0000-000000000000',N'��Ʒ�ͺŹ���',1,N'02000000-0000-0000-0000-000000000000',2.3,'pack://application:,,,/Desktop.Resource;component/Images/ProductModel.png','Desktop.ProductModelModule','Desktop.ProductModelModule.Views.ProductModelView',getdate(),'admin',getdate(),'admin',null)

--��ɫģ��
INSERT INTO [dbo].[RoleModule]([Id],[RoleId],[ModuleId],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])
 select newid() as id ,'01000000-0000-0000-0000-000000000000' as RoleId, id as ModuleId,1 as IsEnable ,getdate() as CreatedTime,'admin'as CreatorUser,getdate()as LastUpdatedTime,'admin'as LastUpdatorUser,null as Remark from  module 

--��־�������� 
GO

delete from [dbo].[AppLog]
DECLARE @ct  int
set @ct=1
WHILE @ct < 300
BEGIN

INSERT INTO [dbo].[AppLog]([CreatedTime],[Level],[Message],[CallSite],[Exception],[StackTrace])
     VALUES ( GETDATE(),'DEBUG',N'����'+ CAST(@ct as nvarchar),'CallSite'+ CAST(@ct as nvarchar),'Exception'+ CAST(@ct as nvarchar),'StackTrace'+ CAST(@ct as nvarchar))	   
set @ct=@ct+1
END 
GO 

--����������� 
delete from [notice]
DECLARE @ct  int
set @ct=1
WHILE @ct < 3
BEGIN

INSERT INTO [dbo].[Notice]([Id],[Content],[OrderCode],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])
     VALUES ('00000000-0000-0000-0000-00000000000'+CAST(@ct as nvarchar), N'ϵͳ������'+ CAST(@ct as nvarchar)+N'�������������ڹ������ģ��¼�롣����ж���������Ҫ��ʾʱ������������Ӵ�С��˳��������ʾ���������ݳ���ҳ��ɼ�����ʱ���������ݻ��Զ��������������ʾĳ�����棬�ɽ�������������Ϊ�����á�û�п��ù���ʱ������ʾ������'
	 ,@ct,1,getdate(),'admin',getdate(),'admin',null)	   
set @ct=@ct+1
END 

GO 
delete from [dbo].[DeviceNode]
DELETE FROM [dbo].[OpcServer]
GO

INSERT INTO [dbo].[OpcServer]([Id],[Name],[Uri],[OpcType],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('01000000-0000-0000-0000-000000000000',N'Opc����','Kepware.KEPServerEX.V6',1,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[OpcServer]([Id],[Name],[Uri],[OpcType],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('02000000-0000-0000-0000-000000000000',N'OpcUa_KEPServerEX_Local','opc.tcp://192.168.1.198:49320',2,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[OpcServer]([Id],[Name],[Uri],[OpcType],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES('03000000-0000-0000-0000-000000000000',N'OpcUa_KEPServerEX','opc.tcp://192.168.1.234:49320',2,1,getdate(),'admin',getdate(),'admin',null)
GO

--���������� 
--delete from [dbo].[DeviceNode]
DECLARE @ct  int
set @ct=1
WHILE @ct < 2
BEGIN

INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'01000000-0000-0000-0000-000000000000','S7 [S7_connection_'+CAST(@ct as nvarchar)+']DB800,X0.2',8,100,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'01000000-0000-0000-0000-000000000000','Channel_1.Device_1.Bool_000'+CAST(@ct as nvarchar),8,100,1,getdate(),'admin',getdate(),'admin',null)
--INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'02000000-0000-0000-0000-000000000000','S7:[S7_connection_'+CAST(@ct as nvarchar)+']DB800,X0.2',8,100,1,getdate(),'admin',getdate(),'admin',null)
--INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'02000000-0000-0000-0000-000000000000','Channel_1.Device_1.Bool_000'+CAST(@ct as nvarchar),8,100,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'02000000-0000-0000-0000-000000000000','ns=2;s=TestChannel.TestDevice.Agv_SettingSpeed'+CAST(@ct as nvarchar),8,100,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'02000000-0000-0000-0000-000000000000','TestChannel.TestDevice.Agv_TaskStatus'+CAST(@ct as nvarchar),8,100,1,getdate(),'admin',getdate(),'admin',null)

--INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'03000000-0000-0000-0000-000000000000','S7:[S7_connection_'+CAST(@ct as nvarchar)+']DB800,X0.2',8,100,1,getdate(),'admin',getdate(),'admin',null)
--INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'03000000-0000-0000-0000-000000000000','Channel_1.Device_1.Bool_000'+CAST(@ct as nvarchar),8,100,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'03000000-0000-0000-0000-000000000000','ns=2;s=TestChannel.TestDevice.Agv_SettingSpeed'+CAST(@ct as nvarchar),8,100,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'03000000-0000-0000-0000-000000000000','TestChannel.TestDevice.Agv_TaskStatus'+CAST(@ct as nvarchar),8,100,1,getdate(),'admin',getdate(),'admin',null)


SET @ct=@ct+1
END 

INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'03000000-0000-0000-0000-000000000000','S7:[S7_connection_1]DB800,X0.2',8,100,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'03000000-0000-0000-0000-000000000000','Channel_1.Device_1.Bool_0001',8,100,1,getdate(),'admin',getdate(),'admin',null)

INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'02000000-0000-0000-0000-000000000000','S7:[S7_connection_2]DB800,X0.2',8,100,1,getdate(),'admin',getdate(),'admin',null)
INSERT INTO [dbo].[DeviceNode]([Id],[OpcServerId],[Name],[DataType],[UpdateRate],[IsEnable],[CreatedTime],[CreatorUser],[LastUpdatedTime],[LastUpdatorUser],[Remark])VALUES(NEWID(),'02000000-0000-0000-0000-000000000000','Channel_1.Device_1.Bool_0002',8,100,1,getdate(),'admin',getdate(),'admin',null)

GO 

