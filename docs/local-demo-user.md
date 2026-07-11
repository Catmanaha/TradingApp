# Local demonstration user

TradingApp does not seed a default account. To create a disposable local user:

1. Create the database with `TradingApp/Assets/Sql/TrandingAppDb.sql` and the
   stock fixtures in `TradingApp/Assets/Sql/TradingAppDbCreates.sql`.
2. Supply `ConnectionStrings__DefaultConnectionString` through the environment
   or user-secrets. Never commit a populated `appsettings.Development.json`.
3. Set `ASPNETCORE_ENVIRONMENT=Development` and run the application.
4. Open `/User/RegisterDemo` and enter a local email and a password of at least
   12 characters.

The registration route is disabled outside Development. Passwords are hashed
with ASP.NET Core's `PasswordHasher<User>` before they are written to SQL
Server; the database stores only `PasswordHash`, never the submitted password.
