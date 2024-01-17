CREATE TABLE [Stocks] (
    [Id] int PRIMARY KEY identity ,
    [Symbol] nvarchar(max),
    [Name] nvarchar(max),
    [MarketCap] nvarchar(max)
)

CREATE TABLE [Users] (
    [Id] int PRIMARY KEY identity,
    [Email] nvarchar(max),
    [Name] nvarchar(max),
    [Surname] nvarchar(max),
    [Password] nvarchar(max)
)
 

CREATE TABLE [UsersStocks] (
    [UserId] int,
    [StockId] int,
    [StockCount] int
)
