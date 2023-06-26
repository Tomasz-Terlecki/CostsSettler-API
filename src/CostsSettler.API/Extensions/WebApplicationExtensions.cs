using CostsSettler.Repo;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CostsSettlerDbContext>();
        await dbContext.Database.MigrateAsync();

        return app;
    }
}
