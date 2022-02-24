To create the initial migration:
> Add-Migration InitialCreate -OutputDir _Migrations

Don't run the Update-Database to create it. Instead run the app with "DbFill mode". It will create the db and fill it.