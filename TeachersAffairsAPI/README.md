# Teachers Affairs

A **Blazor Web App** (.NET 10, *Interactive Auto* render mode) for a single domain — **Teachers** — built on an
**n‑tier architecture** with the **Unit of Work** and **Repository** patterns over **EF Core** and **SQL Server**.

The same use cases are exposed twice: as a Blazor UI and as a documented REST API (`/api/teachers`, Swagger at `/swagger`).

---

## Solution layout

```
TeachersAffairs.slnx
└── src/
    ├── TeachersAffairs.Domain          # Entities & enums. No dependencies.
    ├── TeachersAffairs.Shared          # DTOs + ITeacherApi contract. Shared by the API, the server and the WASM client.
    ├── TeachersAffairs.Application     # Service layer + repository / unit-of-work interfaces + mapping. Depends on Domain + Shared.
    ├── TeachersAffairs.Infrastructure  # EF Core DbContext, repositories, UnitOfWork, migrations, seeding. Depends on Application.
    ├── TeachersAffairs.Web             # Blazor host + API controllers + composition root. Depends on Infrastructure + Web.Client.
    └── TeachersAffairs.Web.Client      # Blazor WebAssembly UI (pages, forms, typed HttpClient). Depends on Shared.
```

Dependency direction always points **inward** (`Web → Infrastructure → Application → Domain`); the outer layers never leak into the inner ones.

### Layer responsibilities

| Layer | Key types |
|-------|-----------|
| **Domain** | `BaseEntity`, `Teacher`, `Gender`, `TeacherLevel` |
| **Application** | `IGenericRepository<T>`, `ITeacherRepository`, `IUnitOfWork`, `ITeacherService` / `TeacherService`, `TeacherMappings`, `NotFoundException`, `BusinessRuleException` |
| **Infrastructure** | `ApplicationDbContext` (audit-stamps `CreatedAtUtc` / `UpdatedAtUtc`), `GenericRepository<T>`, `TeacherRepository`, `UnitOfWork` (lazy repos + transaction helpers), `TeacherConfiguration`, `DbSeeder`, `ApplicationDbContextFactory` (design-time) |
| **Web** | `TeachersController`, `ApiExceptionFilter` (exceptions → RFC 7807 ProblemDetails), `ServerTeacherApi` (implements `ITeacherApi` by calling the service directly) |
| **Web.Client** | `HttpTeacherApiClient` (implements `ITeacherApi` over HTTP), `TeacherFormModel`, `TeacherEditor` component, `TeacherList` / `TeacherDetails` / `TeacherCreate` / `TeacherEdit` pages |

### Why `ITeacherApi`

With *Interactive Auto*, a component first renders on the **server** (pre-render + SignalR circuit) and later in the
**browser** on WebAssembly. Components depend only on `ITeacherApi`:

* on the server it resolves to **`ServerTeacherApi`** → calls `ITeacherService` in-process (no network hop);
* in the browser it resolves to **`HttpTeacherApiClient`** → calls the `/api/teachers` REST endpoints.

### Unit of Work / Repository flow

```
TeacherService  ──uses──▶  IUnitOfWork
                               │  .Teachers            → ITeacherRepository  (lazy, shares the DbContext)
                               │  .Repository<T>()      → IGenericRepository<T>
                               └─ .SaveChangesAsync()   → commits the whole change set once
```

Repositories only stage changes (`Add` / `Update` / `Remove`); **only the Unit of Work calls `SaveChanges`**, so one
service call = one transaction.

---

## Prerequisites

* .NET SDK **10.0**
* SQL Server reachable at `Server=localhost` with Windows authentication
  (change `ConnectionStrings:TeachersConnection` in `src/TeachersAffairs.Web/appsettings.json` for another instance)
* `dotnet-ef` CLI: `dotnet tool install --global dotnet-ef --version 10.*`

## Run

```bash
dotnet run --project src/TeachersAffairs.Web
```

On first start the app **applies migrations and seeds five demo teachers** automatically
(`DbSeeder.MigrateAndSeedAsync` in `Program.cs`).

| URL | |
|-----|--|
| `/`            | Home |
| `/teachers`    | List — search, filter, sort, page, delete |
| `/teachers/create`, `/teachers/{id}`, `/teachers/{id}/edit` | CRUD screens |
| `/swagger`     | REST API explorer (Development only) |

## Database / migrations

```bash
# add a migration
dotnet ef migrations add <Name> \
  --project src/TeachersAffairs.Infrastructure \
  --startup-project src/TeachersAffairs.Web \
  --output-dir Persistence/Migrations

# apply migrations manually (otherwise done on startup)
dotnet ef database update \
  --project src/TeachersAffairs.Infrastructure \
  --startup-project src/TeachersAffairs.Web
```

Database name: **`TeachersAffairsDb`**.

## REST API

| Method | Route | Notes |
|--------|-------|-------|
| `GET`    | `/api/teachers` | `?search=&department=&isActive=&sortBy=&sortDescending=&page=&pageSize=` → `PagedResult<TeacherDto>` |
| `GET`    | `/api/teachers/all` | unpaged |
| `GET`    | `/api/teachers/departments` | distinct department names |
| `GET`    | `/api/teachers/{id}` | `404` if missing |
| `POST`   | `/api/teachers` | `201 Created`; `400` validation; `409` duplicate e‑mail / employee number / national id |
| `PUT`    | `/api/teachers/{id}` | `200`; `404`; `409` |
| `DELETE` | `/api/teachers/{id}` | `204`; `404` |

Errors are returned as `application/problem+json`.