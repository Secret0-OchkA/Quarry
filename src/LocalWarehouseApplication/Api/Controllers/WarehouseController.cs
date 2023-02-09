using Api.Features.Commands;
using Api.Features.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Security.Cryptography.X509Certificates;

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
            Product entity = await mediator.Send(command, token);
            return CreatedAtAction(nameof(CreateProduct), new { id = entity.Id }, entity);
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteProduct(Guid Id, CancellationToken token)
    {
        try
        {
            int count = await mediator.Send(new DeleteProductCommand() { id = Id }, token);
            return Ok(count);
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProduct(CancellationToken token)
    {
        try
        {
            return Ok(await mediator.Send(new GetAllProductQuery(), token));
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
        if (Id != command.Id) return BadRequest("not equal id in route and command");

        try
        {
            return Ok(await mediator.Send(command));
        } catch (FluentValidation.ValidationException ex) { return BadRequest(ex.Message); }
    }
}
