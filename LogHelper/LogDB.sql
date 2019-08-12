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

