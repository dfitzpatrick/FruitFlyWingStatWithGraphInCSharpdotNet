CREATE TABLE [dbo].[WingCoordinate]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [WingId] INT NOT NULL, 
    [x] INT NOT NULL, 
    [y] INT NOT NULL, 
    CONSTRAINT [FK_WingCoordinate_Wing] FOREIGN KEY (WingId) REFERENCES [Wing]([Id])
)
