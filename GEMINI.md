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
- **AI Engine:** [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel)
- **AI Provider:** Google Gemini (2.5 Flash)
- **Auto-Mapping:** [AutoMapper](https://automapper.org/)
- **Template Engine:** [RazorLight](https://github.com/toddams/RazorLight)
- **PDF Generation:** [PuppeteerSharp](https://www.puppeteersharp.com/)
- **Testing:** xUnit & Moq
- **Front-end:** SortableJS (for dynamic ordering)

## Architecture

- **CareerCraft.Core:** Domain models, core service interfaces (`IAiService`, `ISkillService`, `IUserService`, `ITemplateService`, `IPdfGenerator`).
- **CareerCraft.Shared:** Common models, ViewModels, and AutoMapper profiles.
- **CareerCraft.Application:** Application-specific logic.
- **CareerCraft.Infrastructure:** Concrete implementations.
    - `AppDbContext`: Entity Framework context for SQLite.
    - `SemanticKernelAiService`: AI logic with Polly for resilience.
    - `SkillService` & `UserService`: CRUD operations.
    - `VacancySyncService`: Logic for synchronizing job offers.
- **CareerCraft.Web:** MVC Management interface.
- **CareerCraft.Api:** REST API endpoints and template hosting.
- **CareerCraft.Tests:** Integration and unit tests (including AI validation).

## AI Integration (Semantic Kernel)

The application uses **Semantic Kernel** to interact with LLMs. The implementation is decoupled through the `IAiService` interface.

### Features
- **Prompt Externalization:** Prompts are stored in `CareerCraft.Infrastructure/AiPrompts/*.yaml` (e.g., `MatchSkills.yaml`).
- **Resilience:** Integrated **Polly** with retry policies for transient AI errors (429, 500, 503).
- **Multi-Provider:** Support for Google Gemini and OpenAI (configured via `appsettings.json`).
- **Performance:** Optimized for low-resource hardware (extended timeouts, controlled retries).

### AI Configuration (User Secrets)
To use AI features locally, configure your API keys using .NET User Secrets:
```bash
dotnet user-secrets set "Ai:Gemini:ApiKey" "YOUR_KEY" --project CareerCraft.Tests
dotnet user-secrets set "Ai:Gemini:ApiKey" "YOUR_KEY" --project CareerCraft.Api
```

## API & Web Endpoints

### Skills (API & MVC)
- `GET /api/skills`: Retrieve all skills (API).
- `GET /Skills`: Manage skills via MVC interface.

### Users & Profil (API & MVC)
- `GET /api/users`: Retrieve users.
- `GET /Users`: Manage user profiles via MVC interface.

### Vacancy Synchronization (API)
- `POST /api/vacancies/sync`: Full synchronization from external sources.

## Building and Running

### Prerequisites
- .NET 8 SDK
- Google Chrome/Chromium installed (configured in `appsettings.json`)
- Google AI Studio API Key (for Gemini)

### Commands
- **Build the solution:** `dotnet build`
- **Run tests:** `dotnet test`
- **Run AI Integration Test:** `dotnet test --filter MatchSkills_IntegrationTest`
- **Update database:** `dotnet ef database update --project CareerCraft.Infrastructure --startup-project CareerCraft.Api`

## Development Conventions

- **SOLID:** Adhere to SOLID principles, especially Dependency Inversion.
- **AI Prompts:** Keep prompts as external files to avoid hardcoded logic. Use `AiContext` for model parameterization.
- **Resilience:** Always wrap external API calls (AI, Sync) with Polly policies.
- **Shared Contracts:** ViewModels must reside in `CareerCraft.Shared`.
- **Hardware Awareness:** Keep timeouts and resource usage in mind for low-spec hardware compatibility.
