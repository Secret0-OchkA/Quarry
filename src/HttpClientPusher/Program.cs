
using RabbitMQ.Client;
using Warehouse.Services.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



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

app.Run();
