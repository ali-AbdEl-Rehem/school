# Students Affairs

A **Blazor Web App** (.NET 10, *Interactive Auto* render mode) for a single domain — **Students** — built on an
**n‑tier architecture** with the **Unit of Work** and **Repository** patterns over **EF Core** and **SQL Server**.

The same use cases are exposed twice: as a Blazor UI and as a documented REST API (`/api/students`, Swagger at `/swagger`).

---

## Solution layout

```
StudentsAffairs.slnx
└── src/
    ├── StudentsAffairs.Domain          # Entities & enums. No dependencies.
    ├── StudentsAffairs.Shared          # DTOs + IStudentApi contract. Shared by the API, the server and the WASM client.
    ├── StudentsAffairs.Application     # Service layer + repository / unit-of-work interfaces + mapping. Depends on Domain + Shared.
    ├── StudentsAffairs.Infrastructure  # EF Core DbContext, repositories, UnitOfWork, migrations, seeding. Depends on Application.
    ├── StudentsAffairs.Web             # Blazor host + API controllers + composition root. Depends on Infrastructure + Web.Client.
    └── StudentsAffairs.Web.Client      # Blazor WebAssembly UI (pages, forms, typed HttpClient). Depends on Shared.
```

Dependency direction always points **inward** (`Web → Infrastructure → Application → Domain`); the outer layers never leak into the inner ones.

### Layer responsibilities

| Layer | Key types |
|-------|-----------|
| **Domain** | `BaseEntity`, `Student`, `Gender`, `AcademicLevel` |
| **Application** | `IGenericRepository<T>`, `IStudentRepository`, `IUnitOfWork`, `IStudentService` / `StudentService`, `StudentMappings`, `NotFoundException`, `BusinessRuleException` |
| **Infrastructure** | `ApplicationDbContext` (audit-stamps `CreatedAtUtc` / `UpdatedAtUtc`), `GenericRepository<T>`, `StudentRepository`, `UnitOfWork` (lazy repos + transaction helpers), `StudentConfiguration`, `DbSeeder`, `ApplicationDbContextFactory` (design-time) |
| **Web** | `StudentsController`, `ApiExceptionFilter` (exceptions → RFC 7807 ProblemDetails), `ServerStudentApi` (implements `IStudentApi` by calling the service directly) |
| **Web.Client** | `HttpStudentApiClient` (implements `IStudentApi` over HTTP), `StudentFormModel`, `StudentEditor` component, `StudentList` / `StudentDetails` / `StudentCreate` / `StudentEdit` pages |

### Why `IStudentApi`

With *Interactive Auto*, a component first renders on the **server** (pre-render + SignalR circuit) and later in the
**browser** on WebAssembly. Components depend only on `IStudentApi`:

* on the server it resolves to **`ServerStudentApi`** → calls `IStudentService` in-process (no network hop);
* in the browser it resolves to **`HttpStudentApiClient`** → calls the `/api/students` REST endpoints.

### Unit of Work / Repository flow

```
StudentService  ──uses──▶  IUnitOfWork
                               │  .Students            → IStudentRepository  (lazy, shares the DbContext)
                               │  .Repository<T>()      → IGenericRepository<T>
                               └─ .SaveChangesAsync()   → commits the whole change set once
```

Repositories only stage changes (`Add` / `Update` / `Remove`); **only the Unit of Work calls `SaveChanges`**, so one
service call = one transaction.

---

## Prerequisites

* .NET SDK **10.0**
* SQL Server reachable at `Server=localhost` with Windows authentication
  (change `ConnectionStrings:DefaultConnection` in `src/StudentsAffairs.Web/appsettings.json` for another instance)
* `dotnet-ef` CLI: `dotnet tool install --global dotnet-ef --version 10.*`

## Run

```bash
dotnet run --project src/StudentsAffairs.Web
```

On first start the app **applies migrations and seeds five demo students** automatically
(`DbSeeder.MigrateAndSeedAsync` in `Program.cs`).

| URL | |
|-----|--|
| `/`            | Home |
| `/students`    | List — search, filter, sort, page, delete |
| `/students/create`, `/students/{id}`, `/students/{id}/edit` | CRUD screens |
| `/swagger`     | REST API explorer (Development only) |

## Database / migrations

```bash
# add a migration
dotnet ef migrations add <Name> \
  --project src/StudentsAffairs.Infrastructure \
  --startup-project src/StudentsAffairs.Web \
  --output-dir Persistence/Migrations

# apply migrations manually (otherwise done on startup)
dotnet ef database update \
  --project src/StudentsAffairs.Infrastructure \
  --startup-project src/StudentsAffairs.Web
```

Database name: **`StudentsAffairsDb`**.

## REST API

| Method | Route | Notes |
|--------|-------|-------|
| `GET`    | `/api/students` | `?search=&department=&isActive=&sortBy=&sortDescending=&page=&pageSize=` → `PagedResult<StudentDto>` |
| `GET`    | `/api/students/all` | unpaged |
| `GET`    | `/api/students/departments` | distinct department names |
| `GET`    | `/api/students/{id}` | `404` if missing |
| `POST`   | `/api/students` | `201 Created`; `400` validation; `409` duplicate e‑mail / number / national id |
| `PUT`    | `/api/students/{id}` | `200`; `404`; `409` |
| `DELETE` | `/api/students/{id}` | `204`; `404` |

Errors are returned as `application/problem+json`.
