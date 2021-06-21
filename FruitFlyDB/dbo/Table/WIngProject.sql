CREATE TABLE [dbo].[WIngProject]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [name] NVARCHAR(50) NOT NULL, 
    [createdAt] DATETIME NOT NULL DEFAULT getdate(),
	

)
