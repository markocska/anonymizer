Create database Serilog;
go
use Serilog;
go
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Logs] (
    [Id] int NOT NULL IDENTITY,
    [Msg] nvarchar(max) NULL,
    [Template] nvarchar(max) NULL,
    [Severity] nvarchar(max) NULL,
    [Timestamp] datetime NULL,
    [Ex] nvarchar(max) NULL,
    [LogEvent] nvarchar(max) NULL,
    [JobKey] nvarchar(500) NULL,
    [GroupKey] nvarchar(500) NULL,
    [JobDescription] nvarchar(500) NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200328145406_test', N'2.2.6-servicing-10079');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'Severity');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Logs] ALTER COLUMN [Severity] nvarchar(450) NULL;

GO

CREATE INDEX [IX_Logs_Severity] ON [Logs] ([Severity]);

GO

CREATE INDEX [IX_Logs_Timestamp] ON [Logs] ([Timestamp]);

GO

CREATE INDEX [IX_Logs_GroupKey_JobKey] ON [Logs] ([GroupKey], [JobKey]);

GO

CREATE INDEX [IX_Logs_GroupKey_JobKey_Severity] ON [Logs] ([GroupKey], [JobKey], [Severity]);

GO

CREATE INDEX [IX_Logs_GroupKey_JobKey_Timestamp] ON [Logs] ([GroupKey], [JobKey], [Timestamp]);

GO

CREATE INDEX [IX_Logs_GroupKey_JobKey_Severity_Timestamp] ON [Logs] ([GroupKey], [JobKey], [Severity], [Timestamp]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200328160220_indexesAdded', N'2.2.6-servicing-10079');

GO

DROP INDEX [IX_Logs_Severity] ON [Logs];

GO

DROP INDEX [IX_Logs_Timestamp] ON [Logs];

GO

DROP INDEX [IX_Logs_GroupKey_JobKey] ON [Logs];

GO

DROP INDEX [IX_Logs_GroupKey_JobKey_Severity] ON [Logs];

GO

CREATE INDEX [IX_Logs_JobDescription_Timestamp] ON [Logs] ([JobDescription], [Timestamp]);

GO

CREATE INDEX [IX_Logs_Severity_Timestamp] ON [Logs] ([Severity], [Timestamp]);

GO

CREATE INDEX [IX_Logs_Timestamp_Severity] ON [Logs] ([Timestamp], [Severity]);

GO

CREATE INDEX [IX_Logs_JobDescription_GroupKey_Timestamp] ON [Logs] ([JobDescription], [GroupKey], [Timestamp]);

GO

CREATE INDEX [IX_Logs_JobDescription_Severity_Timestamp] ON [Logs] ([JobDescription], [Severity], [Timestamp]);

GO

CREATE INDEX [IX_Logs_JobDescription_GroupKey_Severity_Timestamp] ON [Logs] ([JobDescription], [GroupKey], [Severity], [Timestamp]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200426131910_AddIndexes', N'2.2.6-servicing-10079');

GO

