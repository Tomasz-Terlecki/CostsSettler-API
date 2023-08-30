using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

/// <summary>
/// Controller with endpoints to manage users data.
/// </summary>
[ApiController]
[Route("/api/[Controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates new UsersController class instance.
    /// </summary>
    /// <param name="mediator">Mediator with implementation of queries and commands that manages users data.</param>
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets users filtered by 'query' params.
    /// </summary>
    /// <param name="query">Filters to apply.</param>
    /// <returns>Returns HTTP status 200 with all users that match given query filters on success or 
    /// HTTP status 400 with error description on fail.</returns>
    [HttpGet]
    public async Task<IActionResult> GetByParams([FromQuery] GetUsersByParamsQuery query)
    {
        var result = await _mediator.Send(query);

        if (result is null)
            return BadRequest("Getting users failed");

        return Ok(result);
    }
}
