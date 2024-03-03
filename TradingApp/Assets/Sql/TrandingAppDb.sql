CREATE TABLE [Stocks] (
    [Id] int PRIMARY KEY identity ,
    [Symbol] nvarchar(max),
    [Name] nvarchar(max),
    [MarketCap] bigint
)

CREATE TABLE [Users] (
    [Id] int PRIMARY KEY identity,
    [Email] nvarchar(max),
    [Name] nvarchar(max),
    [Surname] nvarchar(max),
    [Password] nvarchar(max)
)

CREATE TABLE [Logs] (
    [Id] int PRIMARY KEY identity ,
    [UserId] int,
    [Url] nvarchar(max),
    [MethodType] nvarchar(max),
    [StatusCode] int,
    [RequestBody] nvarchar(max),
    [ResponseBody] nvarchar(max)
)
