# Catalog CQRS Project

This project demonstrates the implementation of CQRS (Command Query Responsibility Segregation) pattern in a .NET Core application for managing a product catalog.

![Screenshot 2025-06-01 151222](https://github.com/user-attachments/assets/7d5d0aa6-35ee-4efd-b132-d1a27beb223c)


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

## Key Technologies & Packages

```
┌─────────────────────────────────────────────────────────────┐
│                      ASP.NET Core 8.0                       │
│              (Web API & Application Framework)              │
├─────────────────┬───────────────────────┬─────────────────┤
│     Carter      │       MediatR         │  FluentValidation│
│  Minimal APIs   │   CQRS & Mediator     │   Input Validation│
├─────────────────┴───────────────────────┴─────────────────┤
│                        Mapster                             │
│                    Object Mapping                          │
├─────────────────────────────────────────────────────────────┤
│                         Marten                             │
│           .NET Transactional Document DB                   │
├─────────────────────────────────────────────────────────────┤
│                      PostgreSQL                            │
│              Document Database Storage                      │
└─────────────────────────────────────────────────────────────┘
```

### Technologies Overview

- **.NET 8 (ASP.NET Core)**: The foundational framework for building the web API.
- **PostgreSQL**: The relational database used for data storage.
- **Marten**: A .NET Transactional Document DB for PostgreSQL, leveraging PostgreSQL's JSON capabilities for storing, querying, and managing documents. It provides a flexible document database approach while maintaining the reliability of PostgreSQL.
- **MediatR**: A library that simplifies the implementation of the CQRS and Mediator patterns, facilitating in-process messaging and reducing direct dependencies.
- **Carter**: A lightweight, convention-based routing library for ASP.NET Core that simplifies defining API endpoints with clean and concise code.
- **Mapster**: A fast and configurable object mapper that simplifies the task of mapping objects between different layers (e.g., DTOs to Domain Models).
- **FluentValidation**: A .NET library for building strongly-typed validation rules, ensuring inputs are correct before processing.
- **Docker**: Used for containerization, enabling consistent deployment across various environments.

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
   - Docker Desktop
   - Docker Compose

2. Clone the repository:
   ```powershell
   git clone https://github.com/cuunon9x/catalog-cqrs.git
   cd catalog-cqrs
   ```

3. Start the infrastructure services (PostgreSQL and pgAdmin):
   ```powershell
   docker-compose up -d postgres pgadmin
   ```
   This will start:
   - PostgreSQL on port 5432
   - pgAdmin on http://localhost:5050 (email: admin@admin.com, password: admin)

4. Run the API:
   ```powershell
   dotnet run --project CatalogCQRS.API
   ```
   
5. Access the services:
   - API and Swagger UI: https://localhost:5001/swagger
   - Health Check: https://localhost:5001/health
   - pgAdmin: http://localhost:5050

6. (Optional) To run everything in Docker including the API:
   ```powershell
   docker-compose up -d
   ```

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

This project follows CQRS pattern with Clean Architecture principles. Here's a visual representation:

```
┌──────────────────────────────────────────────────────────────┐
│                      CatalogCQRS.API                         │
│  ┌────────────────────────────────────────────────────────┐  │
│  │                   REST Endpoints                        │  │
│  │            (Carter Minimal API Routes)                  │  │
│  └────────────────────────────────────────────────────────┘  │
└─────────────────────────────┬────────────────────────────────┘
                              │
┌─────────────────────────────▼────────────────────────────────┐
│                   CatalogCQRS.Application                     │
│  ┌─────────────────┐                  ┌──────────────────┐   │
│  │  Command Stack  │                  │   Query Stack    │   │
│  │   ┌─────────┐  │                  │   ┌─────────┐   │   │
│  │   │Commands │  │                  │   │ Queries │   │   │
│  │   └────┬────┘  │                  │   └────┬────┘   │   │
│  │        │       │                  │        │        │   │
│  │   ┌────▼────┐  │                  │   ┌────▼────┐   │   │
│  │   │Handlers │  │                  │   │Handlers │   │   │
│  │   └────┬────┘  │                  │   └────┬────┘   │   │
│  └────────│───────┘                  └────────│────────┘   │
└───────────│────────────────────────────────────│────────────┘
            │                                    │
┌───────────▼────────────────────────────────────▼────────────┐
│                     CatalogCQRS.Domain                       │
│  ┌────────────────────────────────────────────────────────┐  │
│  │                   Domain Model                         │  │
│  │        (Entities, Value Objects, Interfaces)           │  │
│  └────────────────────────────────────────────────────────┘  │
└─────────────────────────────┬────────────────────────────────┘
                              │
┌─────────────────────────────▼────────────────────────────────┐
│                  CatalogCQRS.Infrastructure                   │
│  ┌────────────────────────────────────────────────────────┐  │
│  │              Marten Document Store                     │  │
│  │         (Repository Implementation)                    │  │
│  └────────────────────────────────────────────────────────┘  │
└─────────────────────────────┬────────────────────────────────┘
                              │
┌─────────────────────────────▼────────────────────────────────┐
│                        PostgreSQL                             │
│                     (Document Database)                       │
└──────────────────────────────────────────────────────────────┘
```

### Key Architectural Concepts:

- **Command Stack**: Handles create, update, and delete operations
  - Commands are immutable and represent intent to change state
  - Each command has one handler
  - Validates input using FluentValidation

- **Query Stack**: Handles read operations
  - Queries are immutable and represent intent to retrieve data
  - Each query has one handler
  - Can be optimized independently from commands

- **Domain Model**: Rich domain models with business rules
  - Encapsulates business logic
  - Enforces invariants
  - Uses value objects for immutability

- **Infrastructure**: 
  - Uses Marten as document store
  - PostgreSQL for persistence
  - CQRS-friendly document database architecture

### Data Flow:

1. **Commands**:
   ```
   HTTP Request → API → Command → Handler → Domain Model → Repository → Database
   ```

2. **Queries**:
   ```
   HTTP Request → API → Query → Handler → Repository → Database → DTO → Response
   ```
