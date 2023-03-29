using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Amatsucozy.PMS.Email.Infrastructure;

public sealed class DbContextFactory : IDesignTimeDbContextFactory<EmailDbContext>
{
    public EmailDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<EmailDbContext>()
            .UseNpgsql(DbConstants.ConnectionString, sqlBuilder =>
            {
                sqlBuilder.MigrationsAssembly(typeof(InfrastructureMarker).Assembly.GetName().Name);
            }).Options;

        return new EmailDbContext(options);
    }
}