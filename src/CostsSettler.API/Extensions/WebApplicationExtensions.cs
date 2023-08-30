using CostsSettler.Repo;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.API.Extensions;

/// <summary>
/// 'WebApplication' class extension methods.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Applies all migrations for CostsSettlerDbContext to database.
    /// </summary>
    /// <param name="app">Web application that extension method will be applied to.</param>
    /// <returns>Given 'app' object.</returns>
    public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CostsSettlerDbContext>();
        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch { }

        return app;
    }
}
