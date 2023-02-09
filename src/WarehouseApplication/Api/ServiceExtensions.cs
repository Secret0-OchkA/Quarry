﻿using Api.Features.Behaviour;
using Api.Features.Commands;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using Domain;
using FluentValidation;
using Infrastructura;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Behaviour;
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

            services.AddTransient<IRepository<Product>,ProductRepository>();

            services.AddControllers();

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
            services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaiour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            services.AddMediatR(Assembly.GetExecutingAssembly(),
                typeof(CreateProductCommand).Assembly);
            return services;
        }
    }
}
