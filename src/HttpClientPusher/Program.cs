
using RabbitMQ.Client;
using Warehouse.Services.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IConnectionFactory>(provider => new ConnectionFactory { HostName = "rabbitmq" });
builder.Services.AddScoped(provider => provider.GetRequiredService<IConnectionFactory>().CreateConnection());
builder.Services.AddScoped(provider => provider.GetRequiredService<IConnection>().CreateModel());
builder.Services.AddScoped(provider => provider.GetRequiredService<IModel>().QueueDeclare(
    queue: "MyQueue",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null));
builder.Services.AddHostedService<RabbitMqConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
