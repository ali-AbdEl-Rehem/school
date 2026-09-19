using StudentsAffairs.Application;
using StudentsAffairs.Infrastructure;
using StudentsAffairs.Infrastructure.Persistence;
using StudentsAffairs.Shared.Abstractions;
using StudentsAffairs.Web.Components;
using StudentsAffairs.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS for School.UI dashboard
builder.Services.AddCors(options =>
{
    options.AddPolicy("SchoolUI", policy =>
        policy.WithOrigins("https://localhost:7227", "http://localhost:5138")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// ---- Presentation ----
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Students Affairs API", Version = "v1" }));

// ---- Application + Infrastructure (n-tier composition) ----
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IStudentApi, ServerStudentApi>();

// ---- Cross-cutting ----
builder.Services.AddProblemDetails();

var app = builder.Build();

// ---- Pipeline ----
app.UseCors("SchoolUI");

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

await DbSeeder.MigrateAndSeedAsync(app.Services);

app.Run();