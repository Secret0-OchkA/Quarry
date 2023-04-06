using Api;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Order.Api;

var builder = WebApplication.CreateBuilder(args);

Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .Build();

builder.Services.ConfigureServices(configuration);


var app = builder.Build();
app.InitializeDatabase();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseMetricsEndpoint();
app.UseMetricsRequestTrackingMiddleware();
app.UseMetricsAllEndpoints();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.Run();