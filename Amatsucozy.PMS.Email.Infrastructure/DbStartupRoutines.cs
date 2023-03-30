using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Amatsucozy.PMS.Email.Infrastructure;

public static class DbStartupRoutines
{
    public static void DbStart(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        using var securityDbContext = scope.ServiceProvider.GetRequiredService<EmailDbContext>();

        if (securityDbContext.Database.GetPendingMigrations().Any())
        {
            securityDbContext.Database.Migrate();
        }
    }
}