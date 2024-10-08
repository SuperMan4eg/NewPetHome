using Microsoft.EntityFrameworkCore;

namespace NewPetHome.Web.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigration<T>(this WebApplication app) where T : DbContext
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();

        await dbContext.Database.MigrateAsync();
    }
}