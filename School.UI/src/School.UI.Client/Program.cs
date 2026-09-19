using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using School.UI.Client;
using School.UI.Client.Abstractions;
using School.UI.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Students API
builder.Services.AddHttpClient<IStudentApi, HttpStudentApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7224/");
});

// Teachers API
builder.Services.AddHttpClient<ITeacherApi, HttpTeacherApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7225/");
});

await builder.Build().RunAsync();