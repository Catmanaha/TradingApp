# TradingApp

TradingApp is a small ASP.NET Core MVC backend demonstration for storing and
listing stock reference records in SQL Server. It uses Dapper repositories,
cookie-based session identification, ASP.NET Core Data Protection, and optional
request-metadata logging.

This is a learning project, not a trading platform, market-data service, or
production authentication system. The committed SQL records are illustrative
fixtures rather than live market data.

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

The application contains stock, user, and logging repositories. SQL statements
use Dapper parameters for user-supplied values. The logging middleware records
the request path, method, response status, and protected user identifier; it
does not persist query strings or request/response bodies because those values
can contain credentials or other sensitive data.

## Requirements

- .NET 10 SDK
- SQL Server reachable from the application

## Local setup

Create the database objects with:

- `TradingApp/Assets/Sql/TrandingAppDb.sql`
- `TradingApp/Assets/Sql/TradingAppDbCreates.sql`

Set the connection string outside source control. In PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnectionString = "Server=localhost;Database=TradingAppDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
dotnet run --project TradingApp/TradingApp.csproj
```

`TradingApp/appsettings.Development.example.json` documents the expected local
configuration shape. Do not commit a populated
`appsettings.Development.json`.

## Build and dependency checks

```powershell
dotnet restore TradingApp/TradingApp.csproj
dotnet build TradingApp/TradingApp.csproj --configuration Release --no-restore
dotnet list TradingApp/TradingApp.csproj package --vulnerable --include-transitive
```

GitHub Actions runs the same build and vulnerability check for pushes and pull
requests.

## Security limitations

- The current user table and login query use plaintext passwords. This must be
  replaced with a slow password-hashing scheme before the authentication flow
  is used outside local demonstration.
- The protected user-ID cookie identifies a local demo session but is not a
  complete authentication and authorization design.
- No default user credential is seeded. Create local test data deliberately and
  never reuse a personal password.
- A previously committed development connection string remains in Git history.
  Any reused credential must be rotated; deleting the current file does not
  remove historical commits.

## Current limitations

- There is no automated test project yet.
- Database schema changes are maintained as SQL scripts rather than migrations.
- Error handling and validation are intentionally limited to the learning scope.
- This repository includes contributions from multiple authors. Commit history
  should be used to distinguish individual contributions.
