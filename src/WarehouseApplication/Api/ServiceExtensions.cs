
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using Domain;
using FluentValidation;
using infrastructura;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Behaviour;
using Services.Commands;
using Services.Queries;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;

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

            services.ConfigureMediatorR(configuration);

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

        private static IServiceCollection ConfigureMediatorR(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblies(new List<Assembly>{ typeof(CreateProductCommand).Assembly, typeof(GetAllProductQuery).Assembly});
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaiour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

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
