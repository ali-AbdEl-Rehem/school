using StudentsAffairs.Application;
using StudentsAffairs.Infrastructure;
using StudentsAffairs.Infrastructure.Persistence;
using StudentsAffairs.Shared.Abstractions;
using StudentsAffairs.Web.Components;
using StudentsAffairs.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ---- Presentation ----
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers(options =>
{
    // API actions: map Application exceptions -> ProblemDetails.
    options.Filters.Add<ApiExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Students Affairs API", Version = "v1" }));

// ---- Application + Infrastructure (n-tier composition) ----
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// When a component renders on the server, talk to the Application layer directly (no HTTP hop).
// The WebAssembly client project registers its own HttpClient-based IStudentApi.
builder.Services.AddScoped<IStudentApi, ServerStudentApi>();

// ---- Cross-cutting ----
builder.Services.AddProblemDetails();

var app = builder.Build();

// ---- Pipeline ----
// Non-API (Blazor) unhandled errors -> /Error page. API errors are handled by ApiExceptionFilter.
app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandlingPath = "/Error" });

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Students Affairs API v1"));
}
else
{
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(StudentsAffairs.Web.Client._Imports).Assembly);

// Apply migrations and seed demo data on startup so the app is usable on first run.
await DbSeeder.MigrateAndSeedAsync(app.Services);

app.Run();
