using Shared.Kafka.Consumer;
using User.Api.Data;
using User.Api.Services.Kafka.Messages;

namespace User.Api.Services.Kafka.Handlers;

public class UserCreatedHandler : IKafkaHandler<string, UserMessage>
{
    private readonly ILogger<UserCreatedHandler> _logger;
    private readonly UserDbContext _dbContext;

    public UserCreatedHandler(ILogger<UserCreatedHandler> logger, UserDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task HandleAsync(string key, UserMessage value)
    {
        _logger.LogInformation($"UserCreatedHandler started. -- Key: {key}");

        await _dbContext.Users.AddAsync(new()
        {
            Id = value.Id,
            Email = value.Email,
            FirstName = value.FirstName
        });

        await _dbContext.SaveChangesAsync();


        _logger.LogInformation($"UserCreatedHandler end.");
    }
}