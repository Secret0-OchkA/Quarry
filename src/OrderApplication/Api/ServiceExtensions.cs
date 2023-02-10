
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using FluentValidation;
using Infrastructura;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Behaviour;
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
            services.AddControllers();

            services.AddMetrics(configuration);
            services.AddEfCore(configuration);

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

        private static IServiceCollection ConfigureMediatorR(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(typeof(GetByIdOrderQuery).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaiour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            services.AddMediatR(typeof(GetByIdOrderQuery).Assembly, typeof(OrderAppDbContext).Assembly);
            return services;
        }

        public static void AddEfCore(this IServiceCollection services, IConfiguration config)
        {
            const int PoolSize = 3000;
            services.AddDbContextPool<OrderAppDbContext>((dbContextConfig) =>
            {
                dbContextConfig
                .UseSqlServer(config.GetConnectionString("MsSql"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, PoolSize);
        }
    }
}
