using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByParams([FromQuery] GetUsersQuery query)
    {
        var result = await _mediator.Send(query);

        if (result is null)
            return BadRequest("Getting users failed");

        return new OkObjectResult(result);
    }
}
