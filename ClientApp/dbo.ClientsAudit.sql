CREATE TABLE [dbo].[ClientsAudit] (
    [Id]         INT        IDENTITY (1, 1) NOT NULL,
    [AuditData]  NCHAR (60) NULL,
    [RecordedAt] DATETIME   DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

