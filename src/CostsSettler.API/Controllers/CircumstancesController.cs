using CostsSettler.Domain.Commands;
using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class CircumstancesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CircumstancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetCircumstancesQuery query)
    {
        var circumstances = await _mediator.Send(query);

        if (circumstances is null)
            return BadRequest("Could not get any circumstances from database");

        return Ok(circumstances);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var circumstance = await _mediator.Send(new GetCircumstanceByIdQuery(id));

        if (circumstance is null)
            return BadRequest($"Could not get circumstance of Id {id}");

        return Ok(circumstance);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddCircumstanceCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}