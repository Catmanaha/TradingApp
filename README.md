# TradingApp

TradingApp is a learning ASP.NET Core MVC application for managing illustrative
stock reference records in SQL Server. It demonstrates a small backend with
Dapper repositories, local cookie authentication, metadata-only request
logging, automated tests and dependency scanning.

It is not a trading platform, market-data service, low-latency system or
deployed financial product. The stock rows are illustrative fixtures, not live
market data.

## What it demonstrates

- ASP.NET Core MVC controllers and Razor views.
- Repository abstractions over parameterised Dapper queries.
- SQL Server schema and stock reference-record CRUD.
- ASP.NET Core cookie authentication with `PasswordHasher<User>` for local use.
- ASP.NET Core Data Protection for the authentication cookie.
- Request metadata logging without query strings, request bodies or response
  bodies.
- xUnit tests for authentication, route boundaries, stock validation and logging.
- GitHub Actions Release builds and NuGet vulnerability scans.

## Architecture

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

The application has separate stock, user and logging repositories. User input
is passed to Dapper as parameters; SQL Server remains an explicit local
dependency rather than being hidden behind an in-memory substitute.

## Local setup

Requirements: .NET 10 SDK and a local SQL Server instance.

1. Create a fresh local database using:

   - `TradingApp/Assets/Sql/TrandingAppDb.sql`
   - `TradingApp/Assets/Sql/TradingAppDbCreates.sql`

2. Supply the connection string through an environment variable or user-secrets:

   ```powershell
   $env:ConnectionStrings__DefaultConnectionString = "Server=localhost;Database=TradingAppDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
   ```

   Never commit a populated `appsettings.Development.json`, connection string,
   password, publish profile or `.env` file. The checked-in example contains
   placeholders only.

3. For a disposable local account, set `ASPNETCORE_ENVIRONMENT=Development`,
   run the app, and use `/User/RegisterDemo`. See
   [`docs/local-demo-user.md`](docs/local-demo-user.md). The route is disabled
   outside Development and stores only a password hash.

## Build, test and scan

```powershell
dotnet restore TradingApp/TradingApp.csproj
dotnet build TradingApp/TradingApp.csproj --configuration Release --no-restore
dotnet test tests/TradingApp.Tests/TradingApp.Tests.csproj --configuration Release
dotnet list TradingApp/TradingApp.csproj package --vulnerable --include-transitive
```

GitHub Actions runs the application and test Release builds, the test suite and
the application/test dependency vulnerability scans for pushes and pull
requests. No personal database or Azure credential is required in CI.

## Security scope

This is proportionate local/demo authentication, not a production identity
platform. Stock routes require an authenticated cookie session; login and the
Development-only demo-user route remain anonymous. Application credentials are
provided through runtime configuration and are not committed.

The repository has multiple historical contributors. Commit history, rather
than the README, should be used to attribute individual implementation work.

## Naming recommendation

`TradingApp` is broader than the current scope. `StockReferenceApp` would be a
more precise recruiter-facing name, but renaming is optional and should only be
done after checking links, pull requests and any coursework references.
