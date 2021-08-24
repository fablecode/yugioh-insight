CREATE TABLE [dbo].[Card] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CardNumber]  BIGINT         NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [CardLevel]   INT            NULL,
    [CardRank]    INT            NULL,
    [Atk]         INT            NULL,
    [Def]         INT            NULL,
    [Created]     DATETIME2 (7)  NOT NULL,
    [Updated]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Card] UNIQUE NONCLUSTERED ([Name] ASC)
);

