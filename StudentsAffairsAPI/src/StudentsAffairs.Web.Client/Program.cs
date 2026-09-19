using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudentsAffairs.Shared.Abstractions;
using StudentsAffairs.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// The WASM client reaches the API on the same origin it was served from.
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<IStudentApi, HttpStudentApiClient>();

await builder.Build().RunAsync();
