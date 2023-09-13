using Auth.Api.Data;
using Auth.Api.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kafka.MessageBus;

namespace Auth.Api.Application.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IdentityDbContext _dbContext;
    private readonly IKafkaMessageBus<string, User> _messageBus;

    public RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger, IdentityDbContext dbContext,
        IKafkaMessageBus<string, User> messageBus)
    {
        _logger = logger;
        _dbContext = dbContext;
        _messageBus = messageBus;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RegisterUserCommand Handler started.");

        if (await _dbContext.Users.AsNoTracking()
                .AnyAsync(s => s.Email == request.Email, cancellationToken: cancellationToken))
            throw new ApplicationException("Email is already exist.");

        var user = new User
        {
            Id = request.Id,
            Password = request.Password,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(request.Email, user);
        return user.Id;
    }
}