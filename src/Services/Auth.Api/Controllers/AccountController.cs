using Auth.Api.Application.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IMediator _mediator;

    public AccountController(ILogger<AccountController> logger, IMediator mediator)
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