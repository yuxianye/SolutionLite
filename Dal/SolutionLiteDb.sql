-- USE [D:\SVN\Base\branches\单机版框架\03_项目开发\06_源代码\SolutionLite\DAL\SOLUTIONLITEDB.MDF]
-- 判断要创建的表名是否存在 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RoleModule]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[RoleModule] 
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UserRole]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[UserRole]
GO 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Module]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[Module]
GO 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[User]
GO 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Role]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[Role]
GO 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AppLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[AppLog]
GO 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeviceNode]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[DeviceNode]
GO 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Equipment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[Equipment]
GO 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OpcServer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[OpcServer]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Notice]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[Notice]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Station]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  
drop table [dbo].[Station]
GO

--创建表
CREATE TABLE [dbo].[Role] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]            NVARCHAR (16)    NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_Role_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色名称（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'Remark';

GO

CREATE TABLE [dbo].[User] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]             NVARCHAR (16)    NOT NULL,
    [NickName]         NVARCHAR (16)    NULL,
    [SecurityPassword] NVARCHAR (50)    NOT NULL,
    [CreatedTime]      DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]      NVARCHAR (16)    NULL,
    [LastUpdatedTime]  DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser]  NVARCHAR (16)    NULL,
    [Remark]           NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_User_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'昵称、中文名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'NickName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'安全密码', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'SecurityPassword';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'Remark';

GO

CREATE TABLE [dbo].[UserRole] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [RoleId]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC, [UserId] ASC),
    UNIQUE NONCLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'UserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'RoleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserRole', @level2type = N'COLUMN', @level2name = N'Remark';

GO
CREATE TABLE [dbo].[Module] (
    [Id]                 UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]               NVARCHAR (16)    NOT NULL,
    [ParentId]           UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [OrderCode]          REAL             DEFAULT ((0)) NULL,
    [ShowInNavigateMenu] BIT              DEFAULT ((1)) NULL,
    [Icon]               NVARCHAR (100)   NULL,
    [AssemblyName]       NVARCHAR (100)   DEFAULT ('Desktop.UserModule') NULL,
    [ViewName]           NVARCHAR (100)   DEFAULT ('Desktop.UserModule.Views.UserView') NULL,
    [CreatedTime]        DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]        NVARCHAR (16)    NULL,
    [LastUpdatedTime]    DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser]    NVARCHAR (16)    NULL,
    [Remark]             NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_Module_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模块名称（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父节点编号,默认 00000000-0000-0000-0000-000000000000', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'ParentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序码', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'OrderCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否在导航菜单显示，默认显示', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'ShowInNavigateMenu';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'图标', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'Icon';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'程序集名，例如：Desktop.UserModule', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'AssemblyName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'页面名，例如：Desktop.UserModule.Views.UserView', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'ViewName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Module', @level2type = N'COLUMN', @level2name = N'Remark';


GO

CREATE TABLE [dbo].[RoleModule] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [RoleId]          UNIQUEIDENTIFIER NOT NULL,
    [ModuleId]        UNIQUEIDENTIFIER NOT NULL,
    [IsEnable]        BIT              DEFAULT ((1)) NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC, [ModuleId] ASC),
    CONSTRAINT [FK_RoleModule_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [FK_RoleModule_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'RoleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模块GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'ModuleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否启用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'IsEnable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleModule', @level2type = N'COLUMN', @level2name = N'Remark';

GO
CREATE TABLE [dbo].[OpcServer] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]            NVARCHAR (50)    DEFAULT ('OPC Server') NOT NULL,
    [Uri]             NVARCHAR (50)    DEFAULT ('Kepware.KEPServerEX.V6') NOT NULL,
    [OpcType]        int              DEFAULT ((0)) NOT NULL,
    [IsEnable]        BIT              DEFAULT ((1)) NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_OpcServer_Uri] UNIQUE NONCLUSTERED ([Uri] ASC),
    CONSTRAINT [AK_OpcServer_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名称（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Uri地址（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'Uri';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'类型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'OpcType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否启用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'IsEnable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OpcServer', @level2type = N'COLUMN', @level2name = N'Remark';


GO
CREATE TABLE [dbo].[DeviceNode] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [OpcServerId]     UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR (50)    NOT NULL,
    [DataType]        INT              DEFAULT ((0)) NOT NULL,
    [UpdateRate]      INT              DEFAULT ((100)) NOT NULL,
    [IsEnable]        BIT              DEFAULT ((1)) NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_DeviceNode_OpcServerId+Name] UNIQUE NONCLUSTERED ([OpcServerId] ASC, [Name] ASC),
    CONSTRAINT [FK_DeviceNode_OpcServer] FOREIGN KEY ([OpcServerId]) REFERENCES [dbo].[OpcServer] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OPC服务器GUID主键（与Name组合唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'OpcServerId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'节点名称（与OPC ServerId组合唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'数据类型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'DataType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新频率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'UpdateRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否启用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'IsEnable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DeviceNode', @level2type = N'COLUMN', @level2name = N'Remark';

GO

CREATE TABLE [dbo].[Equipment] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Code]            NVARCHAR (50)    NOT NULL,
    [Name]            NVARCHAR (50)    NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_Equipment_Code] UNIQUE NONCLUSTERED ([Code] ASC),
    CONSTRAINT [AK_Equipment_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'设备编码（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'设备名称（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后修改人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Equipment', @level2type = N'COLUMN', @level2name = N'Remark';

GO


CREATE TABLE [dbo].[Notice] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Content]         NVARCHAR (MAX)   NOT NULL,
    [OrderCode]       FLOAT (53)       DEFAULT ((1)) NOT NULL,
    [IsEnable]        BIT              DEFAULT ((1)) NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'内容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序码，默认1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'OrderCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否启用，默认启用1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'IsEnable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Remark';

GO

CREATE TABLE [dbo].[AppLog] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [CreatedTime] DATETIME2 (7)  NOT NULL,
    [Level]       NVARCHAR (5)   NOT NULL,
    [ThreadId]    INT            DEFAULT ((0)) NOT NULL,
    [Message]     NVARCHAR (MAX) NOT NULL,
    [CallSite]    NVARCHAR (MAX) NULL,
    [Exception]   NVARCHAR (MAX) NULL,
    [StackTrace]  NVARCHAR (MAX) NULL
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'等级', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'Level';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'线程号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'ThreadId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'Message';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'来源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'CallSite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'异常', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'Exception';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'堆栈', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AppLog', @level2type = N'COLUMN', @level2name = N'StackTrace';

GO
CREATE TABLE [dbo].[Station] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Code]            NVARCHAR (50)    NOT NULL,
    [Name]            NVARCHAR (50)    NOT NULL,
    [CreatedTime]     DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [CreatorUser]     NVARCHAR (16)    NULL,
    [LastUpdatedTime] DATETIME2 (7)    DEFAULT (getdate()) NULL,
    [LastUpdatorUser] NVARCHAR (16)    NULL,
    [Remark]          NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_Station_Code] UNIQUE NONCLUSTERED ([Code] ASC),
    CONSTRAINT [AK_Station_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GUID主键', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工序编码（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工序名称（唯一）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'CreatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'LastUpdatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后修改人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'LastUpdatorUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Station', @level2type = N'COLUMN', @level2name = N'Remark';

GO