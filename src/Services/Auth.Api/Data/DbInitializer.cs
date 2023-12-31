using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Data;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        context.Database.Migrate();
    }
}