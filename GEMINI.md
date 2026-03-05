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
- **Front-end:** SortableJS (for dynamic ordering)

## Architecture

- **CareerCraft.Core:** Domain models (Entities), core service interfaces (`ISkillService`, `IUserService`, `ITemplateService`, `IPdfGenerator`).
- **CareerCraft.Shared:** Common models (External API contracts), ViewModels, and AutoMapper profiles shared between Web and Api projects.
- **CareerCraft.Application:** Application-specific logic.
- **CareerCraft.Infrastructure:** Concrete implementations.
    - `AppDbContext`: Entity Framework context for SQLite.
    - `SkillService` & `UserService`: CRUD operations.
    - `VacancySyncService`: Logic for synchronizing job offers from external sources.
- **CareerCraft.Web:** MVC Management interface for profile, skills, and users. Uses dynamic JS components (SortableJS) for user info management.
- **CareerCraft.Api:** The entry point. Hosts templates, configures the database, and provides REST API endpoints.
- **CareerCraft.Tests:** Integration and unit tests.

## API & Web Endpoints

### Skills (API & MVC)
- `GET /api/skills`: Retrieve all skills (API).
- `GET /Skills`: Manage skills via MVC interface.
- Full CRUD support for Skills in both API and Web projects.

### Users & Profil (API & MVC)
- `GET /api/users`: Retrieve users.
- `GET /Users`: Manage user profiles via MVC interface.
- **Dynamic UserInfos:** Integrated dynamic table in the User edit view with drag-and-drop ordering (SortableJS).

### Vacancy Synchronization (API)
- `POST /api/vacancies/sync`: Trigger a full synchronization from external sources (e.g., HierarchScraper).
- `POST /api/vacancies/sync/{id}`: Targeted synchronization for a specific vacancy.

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

- **SOLID:** Adhere to SOLID principles, especially Dependency Inversion and Single Responsibility.
- **Auto-Mapping:** Use AutoMapper for mapping between Entities and ViewModels. Profiles are centralized in `CareerCraft.Shared/Mapping`.
- **Shared Contracts:** Always place ViewModels and external API models in the `CareerCraft.Shared` project to ensure consistency between Web and API projects.
- **Migrations:** Use EF Core migrations for any schema changes.
- **External Integration:** External sources should be abstracted via `IVacancySourceService` to allow for easy extension.
- **Templates:** Templates are located in `CareerCraft.Api/Templates`. Resources are linked in `CareerCraft.Tests.csproj` for test consistency.
