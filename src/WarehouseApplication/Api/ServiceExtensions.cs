﻿
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using Domain;
using FluentValidation;
using infrastructura;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using Warehouse.Services.Behaviour;
using Warehouse.Services.Commands;
using Warehouse.Services.Idempotency;
using Warehouse.Services.Queries;
using Warehouse.Services.RabbitMq;

namespace Api
{
    public static partial class ServiceExtensions
    {
        internal static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddEfCore(configuration);

            services.AddMetrics(configuration);

            services.SetupHealthCheack();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<IdempotencyKeyHeaderSwaggerAttribute>();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "warehouse api",
                    Contact = new OpenApiContact
                    {
                        Name = "telegram",
                        Url = new Uri("https://t.me/bingolz")
                    },
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors();

            services.ConfigureMediatorR();

            services.ConfigureRabbit(configuration);

            services.AddGraphQLServer()
                .AddQueryType<Query>();

            return services;
        }

        public static void AddMetrics(this IServiceCollection services, IConfiguration configuration)
        {
            var metricBuilder = new MetricsBuilder()
                .Configuration.ReadFrom(configuration)
                .OutputMetrics.AsPrometheusPlainText();
            
            services.AddMetrics(metricBuilder);
            services.AddMetricsEndpoints(configuration);
            services.AddMvcCore().AddMetricsCore();
            services.AddMetricsTrackingMiddleware(configuration);
        }

        private static IServiceCollection ConfigureMediatorR(this IServiceCollection services)
        {
            services.AddTransient<IIdempotencyKeyProvider, HttpContextIdempotencyKeyProvider>();
            services.AddTransient<IEventManager, EventManager>();
            services.AddHttpContextAccessor();

            services.AddValidatorsFromAssemblies(new List<Assembly> { typeof(CreateProductCommand).Assembly });

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehaiour<,>));
                config.AddOpenBehavior(typeof(IdempotencyBehaviour<,>));
                config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });

                
            return services;
        }

        public static void AddEfCore(this IServiceCollection services, IConfiguration config)
        {
            const int PoolSize = 3000;
            services.AddDbContextPool<WarehouseDbContext>((dbContextConfig) =>
            {
                dbContextConfig
                .UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"))//config.GetConnectionString("MsSql")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, PoolSize);
        }

        public static void SetupHealthCheack(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddNpgSql(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new NullReferenceException("not define connection string for Postgres"));
        }

        public static void ConfigureRabbit(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionFactory>(provider => new ConnectionFactory { HostName = "rabbitmq"});
            services.AddScoped(provider => provider.GetRequiredService<IConnectionFactory>().CreateConnection());
            services.AddScoped(provider => provider.GetRequiredService<IConnection>().CreateModel());
            services.AddScoped(provider => provider.GetRequiredService<IModel>().QueueDeclare(
                queue: "MyQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null));

            services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
        }
    }
}
