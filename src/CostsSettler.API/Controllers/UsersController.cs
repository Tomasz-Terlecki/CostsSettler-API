using CostsSettler.Domain.Queries.User;
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
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new GetUsersQuery());

        if (result is null)
            return BadRequest("Getting users failed");

        return new OkObjectResult(result);
    }
}
