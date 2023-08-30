using CostsSettler.Domain.Commands;
using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

/// <summary>
/// Controller with endpoints to manage circumstances data.
/// </summary>
[ApiController]
[Route("/api/[Controller]")]
[Authorize]
public class CircumstancesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates new CircumstanceController class instance.
    /// </summary>
    /// <param name="mediator">Mediator with implementation of queries and commands that manages circumstances data.</param>
    public CircumstancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets circumstances filtered by 'query' params.
    /// </summary>
    /// <param name="query">Filters to apply.</param>
    /// <returns>Returns HTTP status 200 with all circumstances that match given query filters on success or 
    /// HTTP status 400 with error description on fail.</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetCircumstancesByParamsQuery query)
    {
        var circumstances = await _mediator.Send(query);

        if (circumstances is null)
            return BadRequest("Could not get any circumstances from database");

        return Ok(circumstances);
    }

    /// <summary>
    /// Gets circumstance with given id.
    /// </summary>
    /// <param name="id">Identifier of circumstance.</param>
    /// <returns>Returns HTTP status 200 with circumstance with given identifier on success or 
    /// HTTP status 400 with error description on fail.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var circumstance = await _mediator.Send(new GetCircumstanceByIdQuery(id));

        if (circumstance is null)
            return BadRequest($"Could not get circumstance of Id {id}");

        return Ok(circumstance);
    }

    /// <summary>
    /// Adds new circumstance.
    /// </summary>
    /// <param name="command">Command to apply.</param>
    /// <returns>Returns HTTP status 200 with an information if circumstance is added successfully.</returns>
    [HttpPost]
    public async Task<IActionResult> Add(AddCircumstanceCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}