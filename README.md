# TradingApp

TradingApp is an ASP.NET Core MVC learning project for managing sample stock records in SQL Server. It is a CRUD application built around Dapper repositories, Razor views, cookie authentication, and request logging. Despite the repository name, it does not place trades or use live market data.

This repository has multiple contributors. The commit history is the best source for individual attribution.

## Main functionality

- Create, read, update, and delete stock reference records.
- Parameterised Dapper queries behind repository interfaces.
- Local cookie authentication with passwords hashed by `PasswordHasher<User>`.
- ASP.NET Core Data Protection for authentication cookies.
- Request logging limited to metadata; query strings and request/response bodies are not stored.
- Tests for authentication, route access, stock validation, and logging.

```text
MVC controllers and Razor views
              |
              v
repository interfaces
              |
              v
Dapper SQL repositories ----> SQL Server
              ^
              |
metadata-only logging middleware
```

## Stack

- .NET 10 and ASP.NET Core MVC
- Dapper
- SQL Server
- xUnit
- GitHub Actions

## Run locally

1. Create a fresh local database with:

   - `TradingApp/Assets/Sql/TrandingAppDb.sql`
   - `TradingApp/Assets/Sql/TradingAppDbCreates.sql`

2. Set the connection string through an environment variable or user-secrets:

   ```powershell
   $env:ConnectionStrings__DefaultConnectionString = "Server=localhost;Database=TradingAppDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
   ```

3. Run the application:

   ```powershell
   dotnet run --project TradingApp/TradingApp.csproj
   ```

For a disposable local account, set `ASPNETCORE_ENVIRONMENT=Development` and use `/User/RegisterDemo`. The route is disabled outside Development and stores a password hash rather than the submitted password. See [`docs/local-demo-user.md`](docs/local-demo-user.md).

Do not commit a populated `appsettings.Development.json`, connection string, password, publish profile, or `.env` file.

## Build and test

```powershell
dotnet restore TradingApp/TradingApp.csproj
dotnet build TradingApp/TradingApp.csproj --configuration Release --no-restore
dotnet test tests/TradingApp.Tests/TradingApp.Tests.csproj --configuration Release
dotnet list TradingApp/TradingApp.csproj package --vulnerable --include-transitive
```

GitHub Actions builds the application and test project, runs the tests, and scans both dependency sets. CI does not need a personal database or Azure credentials.

## Still to do

- Add SQL Server integration tests; CI currently tests without provisioning a database.
- Replace the SQL setup scripts with migrations.
- Use HTTPS for any deployment outside local development.
