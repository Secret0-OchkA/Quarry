using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api
{
    public static partial class ServiceExtensions
    {
        public class IdempotencyKeyHeaderSwaggerAttribute : IOperationFilter
        {

            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (context.ApiDescription.HttpMethod == "GET") return;

                operation.Parameters ??= new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Idempotency-Key",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "uuid"
                    }
                });
            }


        }
    }
}
