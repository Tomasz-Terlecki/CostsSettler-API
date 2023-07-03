using CostsSettler.Domain.Commands;
using CostsSettler.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostsSettler.API.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class ChargesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChargesController(IMediator mediator)
    {
        _mediator = mediator;
    }

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

    [HttpPut]
    public async Task<IActionResult> SettleCharge([FromBody] SettleChargeCommand command)
    {
        var isSuccess = await _mediator.Send(command);

        if (!isSuccess)
        {
            return BadRequest($"Could not settle charge of Id {command.MemberChargeId}");
        }

        return Ok(isSuccess);
    }
}
