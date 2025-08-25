# ASP.NET-Core-Web-Product-API-JWT

//code
-build
docker-compose build --no-cache
-run
docker-compose up -d


-database update
docker compose run --rm `
  -e ConnectionStrings__DefaultConnection="Host=postgres;Database=ProductAppDB;Username=postgres;Password=postgres" `
  ef-tools dotnet ef database update `
  --project Auth.API/Auth.API.csproj `
  --startup-project Auth.API/Auth.API.csproj `
  --context AppDbContext

-create migtration (if needed)
docker compose run --rm `
  -e ConnectionStrings__DefaultConnection="Host=postgres;Database=ProductAppDB;Username=postgres;Password=postgres" `
  ef-tools dotnet ef migrations add InitialCreate2 `
  --project Auth.API/Auth.API.csproj `
  --context AppDbContext `
  --output-dir Data/Migrations

run database update after create migration

- useful commands
- check DB's on docker
docker compose exec postgres psql -U postgres
\l (lists all dbs)
\c (switch db for sql querries)
\dt (selected db tables)
//code


