using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor.Services;
using Refit;
using TaskManager.UI.Client.Clients;
using TaskManager.UI.Components;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents();
builder.Services.AddMudServices();

RefitSettings refitSettings = new(new SystemTextJsonContentSerializer(
    new JsonSerializerOptions(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } }));

Uri apiBaseAddress = new(builder.Configuration["ApiBaseAddress"]!);
builder.Services.AddRefitClient<ITaskItemApiClient>(refitSettings)
    .ConfigureHttpClient(client => client.BaseAddress = apiBaseAddress);
builder.Services.AddRefitClient<ICategoryApiClient>(refitSettings)
    .ConfigureHttpClient(client => client.BaseAddress = apiBaseAddress);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(TaskManager.UI.Client._Imports).Assembly);

app.Run();
