using MediatR;
using User.Api.Data;

namespace User.Api.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Data.Entity.User>
{
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    private readonly UserDbContext _dbContext;

    public GetUserByIdQueryHandler(ILogger<GetUserByIdQueryHandler> logger, UserDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Data.Entity.User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetUserByIdQuery Handler started.");
        
        var user = await _dbContext.Users.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        _logger.LogInformation("GetUserByIdQuery Handler end.");
        
        return user;
    }
}