using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class CircumstanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public CircumstanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetCircumstancesQuery query)
    {
        var circumstances = await _mediator.Send(query);
        
        if (circumstances is null)
        {
            return BadRequest("Could not get any users from database");
        }

        return Ok(circumstances);
    }
}