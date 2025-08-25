# 🚀 ASP.NET-Core-Web-Product-API-JWT

A modern .NET 9 Microservices API with JWT Authentication, Redis Caching, and CQRS Pattern implementation.

## 📋 Project Overview
This project demonstrates a complete backend system with two microservices:

Auth.API - JWT Authentication & User Management

Product.API - Product CRUD Operations with Redis Caching

## 🛠️ Technologies Used
- .NET 9.0 - Latest .NET Framework
- ASP.NET Core Web API - RESTful API Framework
- Entity Framework Core 9 - ORM with PostgreSQL
- JWT Bearer Authentication - Secure Token-based Auth
- Redis - In-memory Data Caching
- Docker & Docker Compose - Containerization
- PostgreSQL - Relational Database
- MediatR - CQRS Pattern Implementation

## 🚀 Quick Start
### Prerequisites
- .NET 9.0 SDK
- Docker Desktop
- PostgreSQL (optional, Docker version provided)
- Redis (optional, Docker version provided)

### Installation & Running
Postgres Default Credentials: Username=postgres;Password=postgres

- Clone the repository
```bash
git clone <repository-url>
cd ProductApiJWT
```

- Start Docker containers
```bash
docker-compose up -d
```
- Run database migrations
```bash 
docker compose run --rm `
  -e ConnectionStrings__DefaultConnection="Host=postgres;Database=ProductAppDB;Username=postgres;Password=postgres" `
  ef-tools dotnet ef database update `
  --project Auth.API/Auth.API.csproj `
  --startup-project Auth.API/Auth.API.csproj `
  --context AppDbContext
```

- Create migratiton if needed (inital migrations provided but just in case)
```bash
docker compose run --rm `
  -e ConnectionStrings__DefaultConnection="Host=postgres;Database=ProductAppDB;Username=postgres;Password=postgres" `
  ef-tools dotnet ef migrations add InitialCreate2 `
  --project Auth.API/Auth.API.csproj `
  --context AppDbContext `
  --output-dir Data/Migrations
```

-Verify services are running
```bash 
docker-compose ps
```

- Check the database on docker
```bash
# psql
docker compose exec postgres psql -U postgres

# (lists all dbs)
\l 

# (switch db for sql querries)
\c ProductAppDB 

# (selected db tables)
\dt 
```
```sql
/*relevant tables*/
select * from "Products";
select * from "AspNetUsers";
```

## Swagger Docs
-Auth API Swagger: http://localhost:7001/swagger
-Product API Swagger: http://localhost:7002/swagger

## 📊 API Endpoints
1. Authentication Endpoints
- POST	"/api/auth/register"	Register new user
- POST	"/api/auth/login"	    Login and get JWT token

2. Product Endpoints (Require JWT)
- GET	/api/products
- POST	/api/products

You can find example ".http" files inside docs/examples.

## 🔐 Authentication Flow
- Register a new user with email and password
- Login to receive JWT token
- Include token in Authorization header: Bearer {token}
- Access protected endpoints with valid token

## ⚙️ Configuration
Environment Variables
Auth.API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Database=ProductAppDB;Username=postgres;Password=postgres",
    "Redis": "redis:6379,abortConnect=false"
  },
  "Jwt": {
    "Secret": "YourSuperSecretKeyHereAtLeast32CharactersLong123!",
    "Issuer": "Auth.API",
    "Audience": "Product.API",
    "ExpiryInMinutes": 60
  }
}
```

Product.API: (Similar structure)
Similar settings inside "docker-compose.override.yml"

Docker Compose Services
- postgres: PostgreSQL 16 database
- redis: Redis 7 caching server
- auth-api: Authentication service (port 7001)
- product-api: Product service (port 7002)
- ef-tools: EF Core tools for migrations