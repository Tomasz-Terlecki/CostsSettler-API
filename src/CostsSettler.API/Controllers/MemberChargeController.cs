using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class MemberChargeController : ControllerBase
{
    private readonly IMediator _mediator;

    public MemberChargeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetChargesByParamsQuery query)
    {
        var charges = await _mediator.Send(query);

        if (charges is null)
        {
            return BadRequest("Could not get any charges from database");
        }

        return Ok(charges);
    }
}
