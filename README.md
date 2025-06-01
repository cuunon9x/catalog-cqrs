# Catalog CQRS Project

This project demonstrates the implementation of CQRS (Command Query Responsibility Segregation) pattern in a .NET Core application for managing a product catalog.

## Project Structure

The solution follows Clean Architecture principles and consists of the following projects:

- **CatalogCQRS.Domain**: Contains domain models and business rules
  - Product and Category entities
  - Value objects and domain events
  
- **CatalogCQRS.Application**: Contains application business rules and CQRS implementations
  - Commands and Command Handlers
  - Queries and Query Handlers
  - Interfaces for infrastructure services
  - Validation rules using FluentValidation

- **CatalogCQRS.Infrastructure**: Contains external concerns and implementations
  - Database context and configurations using Marten
  - Repository implementations
  - External service integrations

- **CatalogCQRS.API**: Contains API endpoints and configurations
  - REST API controllers
  - Dependency injection configurations
  - Middleware configurations
  - API documentation

## Technology Stack

- ASP.NET Core 8.0
- PostgreSQL (via Marten for Document DB)
- MediatR (CQRS implementation)
- FluentValidation
- Carter (for API endpoints)
- Mapster (object mapping)

## Features

- Product management (CRUD operations)
- Category management
- Product search and filtering
- Input validation
- Health checks
- API documentation

## Getting Started

1. Install Prerequisites:
   - .NET 8.0 SDK
   - PostgreSQL

2. Clone the repository

3. Update the connection string in `appsettings.json`

4. Run the migrations:
   ```powershell
   dotnet run --project CatalogCQRS.API
   ```

5. The API will be available at `https://localhost:5001`

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /products | List all products |
| GET | /products/{id} | Get product by ID |
| GET | /products/category | Get products by category |
| POST | /products | Create a new product |
| PUT | /products/{id} | Update a product |
| DELETE | /products/{id} | Delete a product |

## Architecture

This project follows CQRS pattern with the following key concepts:

- **Command Stack**: Handles create, update, and delete operations
- **Query Stack**: Handles read operations
- **Domain Model**: Rich domain models with business rules
- **Validation**: Request validation using FluentValidation
- **Persistence**: Document DB using Marten and PostgreSQL