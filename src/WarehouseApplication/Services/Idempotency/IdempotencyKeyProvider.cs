using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Services.Idempotency
{
    public interface IIdempotencyKeyProvider
    {
        Task<string?> Get();
    }

    public class HttpContextIdempotencyKeyProvider : IIdempotencyKeyProvider
    {
        private readonly IHttpContextAccessor contextAccessor;

        public HttpContextIdempotencyKeyProvider(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public Task<string?> Get()
        {
            string? value = null;

            if (contextAccessor.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var values))
            {
                value = values.ToString();
            }

            return Task.FromResult(value);
        }
    }
}
