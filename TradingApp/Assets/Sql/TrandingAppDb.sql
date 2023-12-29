CREATE TABLE [Stocks] (
  [Id] int PRIMARY KEY,
  [Symbol] nvarchar(max),
  [Name] nvarchar(max),
  [MarketCap] nvarchar(max)
)


CREATE TABLE [Users] (
  [Id] int PRIMARY KEY,
  [Email] nvarchar(max),
  [Name] nvarchar(max),
  [Surname] nvarchar(max),
  [Password] nvarchar(max)
)


CREATE TABLE [UsersStocks] (
  [UserId] int foreign key references Users(Id),
  [StockId] int foreign key references Stocks(Id),
  [StockCount] int
)
