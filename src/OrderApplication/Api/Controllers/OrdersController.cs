using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Queries;
using System.Web.Http.OData;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet,EnableQuery]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll(CancellationToken token)
        {
            try
            {
                return Ok(await mediator.Send(new GetAllOrderQuery(), token));
            } catch(FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
        }
    }
}
