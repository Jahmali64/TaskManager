# Task Manager

A full-stack task management application built with .NET 10 to demonstrate modern software engineering practices including **Clean Architecture**, **Domain-Driven Design**, **SOLID principles**, and a **Blazor WebAssembly** frontend consuming a **RESTful API**.

## Architecture

The solution follows Clean Architecture with strict dependency flow — inner layers have zero knowledge of outer layers.

```
TaskManager.Domain          Pure business logic, entities, value objects, enums
TaskManager.Contracts       Shared DTOs and request models (no EF Core dependency)
TaskManager.Application     Use case orchestration and service layer
TaskManager.Infrastructure  EF Core persistence, SQLite, Fluent API configurations
TaskManager.Api             ASP.NET Core Web API with versioned controllers
TaskManager.UI              Blazor Server host
TaskManager.UI.Client       Blazor WebAssembly client with MudBlazor UI
```

## What This Project Demonstrates

### Clean Architecture & DDD
- Layered project structure with enforced dependency rules
- Rich domain entities with factory methods, guard clauses, and state transitions
- Value Objects with validation and implicit conversion (`Color`)
- Custom domain exceptions (`NotFoundException`)
- Contracts layer to prevent EF Core from bleeding into the UI

### API Design
- RESTful endpoints with API versioning (`Asp.Versioning`)
- Global exception handling middleware producing `ProblemDetails` responses
- Thin controllers delegating immediately to application services
- Structured logging with Serilog (console + rolling file)
- CORS configuration for frontend communication
- OpenAPI documentation with Scalar UI

### Entity Framework Core
- Fluent API configurations (`IEntityTypeConfiguration<T>`)
- Value conversions for domain types (enums to strings, Value Objects)
- `AsNoTracking()` for read-only queries
- Code-first migrations
- SQLite for lightweight local persistence

### Blazor & Frontend
- Blazor Web App with `InteractiveAuto` render mode (Server + WebAssembly)
- MudBlazor component library for Material Design UI
- Refit for typed HTTP client generation
- Component composition: pages, tables, dialogs, action menus
- `EventCallback` pattern for parent-child communication
- Dialog service for modal CRUD workflows
- Snackbar notifications for user feedback

### General Practices
- SOLID principles throughout
- Immutable records for DTOs and requests
- Data annotation validation on request models
- `CancellationToken` support for graceful request handling
- Nullable reference types enabled across all projects
- `sealed` and `internal` by default

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Framework | .NET 10 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | SQLite |
| Frontend | Blazor WebAssembly |
| UI Components | MudBlazor |
| HTTP Client | Refit |
| Logging | Serilog |
| API Docs | OpenAPI / Scalar |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run the API

```bash
cd TaskManager.Api
dotnet run
```

The API starts at `https://localhost:7048`. Scalar API docs are available at `/scalar/v1`.

### Run the UI

In a separate terminal:

```bash
cd TaskManager.UI/TaskManager.UI
dotnet run
```

The UI starts at `https://localhost:7021`.

### Apply Migrations

Migrations are included. EF Core will create the SQLite database (`App.db`) automatically on first run. To apply manually:

```bash
cd TaskManager.Api
dotnet ef database update
```

## Project Structure

```
TaskManager/
├── TaskManager.Domain/
│   ├── Entities/           TaskItem, Category (aggregate roots)
│   ├── Enums/              TaskItemPriority, TaskItemStatus
│   ├── Exceptions/         NotFoundException
│   └── ValueObjects/       Color
├── TaskManager.Contracts/
│   ├── DTOs/               CategoryDto, TaskItemDto
│   └── Requests/           Create, Update, ChangePriority requests
├── TaskManager.Application/
│   └── Services/           CategoryService, TaskItemService
├── TaskManager.Infrastructure/
│   └── Persistence/        AppDbContext, Configurations, Migrations
├── TaskManager.Api/
│   ├── Controllers/        CategoriesController, TaskItemsController
│   └── Middleware/         GlobalExceptionHandler
├── TaskManager.UI/
│   └── Components/         App.razor, Error.razor, ReconnectModal
└── TaskManager.UI.Client/
    ├── Clients/            Refit API client interfaces
    ├── Layout/             MainLayout, NavMenu
    └── Pages/              Home, Categories, TaskItems (with components)
```
