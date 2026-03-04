# CareerCraft

CareerCraft is a .NET 8 application designed to generate professional resumes (CVs) by rendering Razor templates into HTML and then converting them into PDF documents using PuppeteerSharp.

## Project Overview

The project follows a layered architecture (Clean Architecture principles) to decouple domain logic, application services, and infrastructure concerns.

### Key Technologies
- **Framework:** .NET 8
- **Web API:** ASP.NET Core
- **Web MVC:** ASP.NET Core MVC (Management Interface)
- **Database:** SQLite
- **ORM:** Entity Framework Core
- **Auto-Mapping:** [AutoMapper](https://automapper.org/)
- **Template Engine:** [RazorLight](https://github.com/toddams/RazorLight)
- **PDF Generation:** [PuppeteerSharp](https://www.puppeteersharp.com/)
- **Testing:** xUnit

## Architecture

- **CareerCraft.Core:** Domain models (Entities), service interfaces (`ISkillService`, `ITemplateService`, `IPdfGenerator`), and ViewModels.
- **CareerCraft.Application:** Application-specific logic.
- **CareerCraft.Infrastructure:** Concrete implementations.
    - `AppDbContext`: Entity Framework context for SQLite.
    - `SkillService`: CRUD operations for skills.
- **CareerCraft.Web:** MVC Management interface for profile and skills. Uses AutoMapper for Entity/ViewModel conversions.
- **CareerCraft.Api:** The entry point. Hosts templates, configures the database, and provides API endpoints.
- **CareerCraft.Tests:** Integration and unit tests.

## API & Web Endpoints

### Skills (API & MVC)
- `GET /api/skills`: Retrieve all skills (API).
- `GET /Skills`: Manage skills via MVC interface.
- Full CRUD support for Skills in both API and Web projects.

## Building and Running

### Prerequisites
- .NET 8 SDK
- Google Chrome/Chromium installed (configured in `appsettings.json`)

### Commands
- **Build the solution:** `dotnet build`
- **Run the Web interface:** `dotnet run --project CareerCraft.Web`
- **Run the API:** `dotnet run --project CareerCraft.Api`
- **Run tests:** `dotnet test`
- **Update database:** `dotnet ef database update --project CareerCraft.Infrastructure --startup-project CareerCraft.Api`

## Development Conventions

- **SOLID:** Adhere to SOLID principles, especially Dependency Inversion.
- **Auto-Mapping:** Use AutoMapper for mapping between Entities and ViewModels in the Web project. Profiles are located in `CareerCraft.Web/Mapping`.
- **Migrations:** Use EF Core migrations for any schema changes.
- **Templates:** Templates are located in `CareerCraft.Api/Templates`. Resources are linked in `CareerCraft.Tests.csproj` for test consistency.
