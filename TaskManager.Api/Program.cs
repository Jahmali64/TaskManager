using Asp.Versioning;
using Scalar.AspNetCore;
using Serilog;
using TaskManager.Api.Middleware;
using TaskManager.Application;
using TaskManager.Infrastructure;

namespace TaskManager.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting up");
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
            builder.Host.UseSerilog((hostBuilderContext, serviceProvider, configuration) => configuration
                .ReadFrom.Configuration(hostBuilderContext.Configuration)
                .ReadFrom.Services(serviceProvider)
                .Enrich.FromLogContext()
                .WriteTo.Console());

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            string[] corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>() ?? [];
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("BlazorClient", policy =>
                {
                    policy.WithOrigins(corsOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
            builder.Services.AddOpenApi();

            WebApplication app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseExceptionHandler();
            app.UseSerilogRequestLogging();
            app.UseCors("BlazorClient");
            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Unhandled exception");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}