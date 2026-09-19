using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TeachersAffairs.Shared.Abstractions;
using TeachersAffairs.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<ITeacherApi, HttpTeacherApiClient>();

await builder.Build().RunAsync();