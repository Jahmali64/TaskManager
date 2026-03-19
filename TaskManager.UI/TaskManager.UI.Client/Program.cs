using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;
using TaskManager.UI.Client.Clients;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

RefitSettings refitSettings = new(new SystemTextJsonContentSerializer(
    new JsonSerializerOptions(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } }));

Uri apiBaseAddress = new(builder.Configuration["ApiBaseAddress"]!);
builder.Services
    .AddRefitClient<ITaskItemApiClient>(refitSettings)
    .ConfigureHttpClient(client => client.BaseAddress = apiBaseAddress);

builder.Services
    .AddRefitClient<ICategoryApiClient>(refitSettings)
    .ConfigureHttpClient(client => client.BaseAddress = apiBaseAddress);

await builder.Build().RunAsync();
