

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using System.Reflection;
using System.Text;
using Warehouse.Domain;
using Warehouse.Services.Commands;
using Warehouse.Services.RabbitMq;

namespace Warehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> logger;
        private readonly IMediator mediator;

        public EventController(ILogger<EventController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        /// <summary>
        /// push events here
        /// </summary>
        /// <param name="eventObj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        public async Task<ActionResult> ExecuteEvent([FromBody]Event eventObj)
        {
            logger.LogInformation("Get event: {object}", JsonConvert.SerializeObject(eventObj));

            this.Request.Headers.Add("Idempotency-Key", eventObj.Id.ToString());

            Type? eventType = Type.GetType($"Warehouse.Services.Commands.{eventObj.EventType}, {Assembly.GetAssembly(typeof(CreateProductCommand)).GetName()}");
            if (eventType == null)
                throw new ArgumentException("invalid event type");

            var command = eventObj.Data.ToObject(eventType);
            if (command == null)
                throw new ArgumentNullException("null data for event type");

            await mediator.Send(command);
            return Ok();
        }
    }
}
