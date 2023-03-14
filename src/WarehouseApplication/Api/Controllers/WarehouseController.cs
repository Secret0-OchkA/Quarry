
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.Security.Cryptography.X509Certificates;
using Warehouse.Services.Commands;
using Warehouse.Services.Queries;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IMediator mediator;

    public WarehouseController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductCommand command, CancellationToken token)
    {
        try
        {
            await mediator.Send(command, token);
            return Ok();
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteProduct(Guid Id, CancellationToken token)
    {
        try
        {
            int count = await mediator.Send(new DeleteProductCommand() { Id = Id }, token);
            return Ok(count);
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProduct(CancellationToken token, [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
    {
        try
        {
            return Ok(await mediator.Send(new GetAllProductQuery() { Page = page, PageSize = pageSize}, token));
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<Product>> GetByIdProduct(Guid Id, CancellationToken token)
    {
        try
        {
            return Ok(await mediator.Send(new GetByIdProductQuery() { Id = Id }, token));
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<int>> UpdateProduct(Guid Id, [FromBody] UpdateProductCommand command)
    {
        try
        {
            command.Id = Id;
            return Ok(await mediator.Send(command));
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }


    [HttpPost("{id}/[action]")]
    public async Task<ActionResult<Product>> Import(Guid Id, double delta)
    {
        var command = new UpdateProductCountCommand() { ProductId = Id, Delta = delta };

        await mediator.Send(command);

        return Ok();
    }
}
