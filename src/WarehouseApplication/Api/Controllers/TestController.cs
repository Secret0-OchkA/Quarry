

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using Warehouse.Domain;
using Warehouse.Services.Commands;
using Warehouse.Services.RabbitMq;

namespace Warehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IRabbitMQProducer rabbitMqService;

        public TestController(IRabbitMQProducer rabbitMqService)
        {
            this.rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<ActionResult> IndexAsync(CreateProductCommand command)
        {
            Event eventObj = new Event(Guid.Empty,command.GetType().Name,command);
            rabbitMqService.SendMessage(eventObj);
            return Ok();
        }
    }
}
