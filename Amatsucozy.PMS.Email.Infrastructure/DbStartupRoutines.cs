using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Amatsucozy.PMS.Email.Infrastructure;

public static class DbStartupRoutines
{
    public static void DbStart(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var securityDbContext = scope.ServiceProvider.GetRequiredService<EmailDbContext>();

        if (securityDbContext.Database.GetPendingMigrations().Any()) securityDbContext.Database.Migrate();
    }
}