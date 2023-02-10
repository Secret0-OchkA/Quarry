using Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .Build();

builder.Services.ConfigureServices(configuration);

var app = builder.Build();

app.UseMetricsEndpoint();
app.UseMetricsRequestTrackingMiddleware();
app.UseMetricsAllEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
