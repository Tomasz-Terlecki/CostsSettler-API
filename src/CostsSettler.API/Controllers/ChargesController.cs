using CostsSettler.Domain.Commands;
using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

/// <summary>
/// Controller with endpoints to manage charges data.
/// </summary>
[ApiController]
[Route("/api/[Controller]")]
[Authorize]
public class ChargesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates new ChargesController class instance.
    /// </summary>
    /// <param name="mediator">Mediator with implementation of queries and commands that manages charges data.</param>
    public ChargesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets charges filtered by 'query' params.
    /// </summary>
    /// <param name="query">Filters to apply.</param>
    /// <returns>Returns HTTP status 200 with all charges that match given query filters on success or 
    /// HTTP status 400 with error description on fail.</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetChargesByParamsQuery query)
    {
        var charges = await _mediator.Send(query);

        if (charges is null)
        {
            return BadRequest("Could not get any charges from database");
        }

        return Ok(charges);
    }

    /// <summary>
    /// Votes for charge.
    /// </summary>
    /// <param name="command">Command to apply.</param>
    /// <returns>Returns HTTP status 200 with an information (true or false) if voting was successful or 
    /// HTTP status 400 with error description on fail.</returns>
    [HttpPut("vote")]
    public async Task<IActionResult> Vote([FromBody] VoteForChargeCommand command)
    {
        var isSuccess = await _mediator.Send(command);

        if (!isSuccess)
        {
            return BadRequest($"Could not accept charge of Id {command.ChargeId}");
        }

        return Ok(isSuccess);
    }

    /// <summary>
    /// Settles charge.
    /// </summary>
    /// <param name="command">Command to apply.</param>
    /// <returns>Returns HTTP status 200 with an information (true or false) if settling was successful or 
    /// HTTP status 400 with error description on fail.</returns>
    [HttpPut("settle")]
    public async Task<IActionResult> Settle([FromBody] SettleChargeCommand command)
    {
        var isSuccess = await _mediator.Send(command);

        if (!isSuccess)
        {
            return BadRequest($"Could not settle charge of Id {command.ChargeId}");
        }

        return Ok(isSuccess);
    }
}
