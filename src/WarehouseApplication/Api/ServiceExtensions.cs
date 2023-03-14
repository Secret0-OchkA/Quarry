
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using Domain;
using FluentValidation;
using infrastructura;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using Warehouse.Services.Behaviour;
using Warehouse.Services.Commands;
using Warehouse.Services.Idempotency;
using Warehouse.Services.Queries;

namespace Api
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.ConfigureDbConnection(configuration);

            services.AddControllers();

            services.AddEfCore(configuration);

            services.AddMetrics(configuration);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.ConfigureMediatorR();

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

        private static IServiceCollection ConfigureDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDbConnection>(new SqlConnection(configuration.GetConnectionString("MsSql")?? Environment.GetEnvironmentVariable("MsSql")));
            return services;
        }

        private static IServiceCollection ConfigureMediatorR(this IServiceCollection services)
        {
            services.AddTransient<IIdempotencyKeyProvider, HttpContextIdempotencyKeyProvider>();
            services.AddTransient<IEventManager,EventManager>();
            services.AddHttpContextAccessor();

            services.AddValidatorsFromAssemblies(new List<Assembly>{ typeof(CreateProductCommand).Assembly, typeof(GetAllProductQuery).Assembly});
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaiour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            services.AddMediatR(typeof(CreateProductCommand).Assembly,typeof(GetAllProductQuery).Assembly);
            return services;
        }

        public static void AddEfCore(this IServiceCollection services, IConfiguration config)
        {
            const int PoolSize = 3000;
            services.AddDbContextPool<WarehouseDbContext>((dbContextConfig) =>
            {
                dbContextConfig
                .UseSqlServer(config.GetConnectionString("MsSql"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, PoolSize);
        }
    }
}
