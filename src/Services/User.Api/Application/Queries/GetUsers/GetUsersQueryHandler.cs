using MediatR;
using Microsoft.EntityFrameworkCore;
using User.Api.Data;

namespace User.Api.Application.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<Data.Entity.User>>
{
    private readonly ILogger<GetUsersQueryHandler> _logger;
    private readonly UserDbContext _dbContext;

    public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, UserDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<List<Data.Entity.User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetUsersQuery Handler started.");

        var users = await _dbContext.Users.ToListAsync(cancellationToken: cancellationToken);
        if (users == null)
        {
            throw new Exception("Users not found.");
        }

        _logger.LogInformation("GetUsersQuery Handler end.");
        
        return users;
    }
}