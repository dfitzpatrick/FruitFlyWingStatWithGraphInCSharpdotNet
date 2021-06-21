CREATE TABLE [dbo].[Wing]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProjectId] INT NOT NULL, 
    [FileDate] DATETIME NOT NULL, 
    [ImportDate] DATETIME NOT NULL, 
    [Gender] NCHAR(10) NOT NULL, 
    [Perimeter] DECIMAL NULL, 
    [Area] DECIMAL NULL, 
    [Length] NCHAR(10) NULL, 
    CONSTRAINT [FK_Wing_WingProject] FOREIGN KEY ([ProjectId]) REFERENCES [WingProject]([Id])
)
