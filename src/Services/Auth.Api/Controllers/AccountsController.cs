using Auth.Api.Application.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;
    private readonly IMediator _mediator;

    public AccountsController(ILogger<AccountsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("register-user")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        _logger.LogInformation($"RegisterUser api called : {command}");
        return Ok(await _mediator.Send(command));
    }
}