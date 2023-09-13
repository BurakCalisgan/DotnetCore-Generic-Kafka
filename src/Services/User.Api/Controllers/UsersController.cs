using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Api.Application.Queries.GetUserById;
using User.Api.Application.Queries.GetUsers;

namespace User.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IMediator _mediator;

    public UsersController(ILogger<UsersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _mediator.Send(new GetUserByIdQuery() { Id = id }));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new GetUsersQuery()));
    }
}