using Microsoft.EntityFrameworkCore;

namespace Amatsucozy.PMS.Email.Infrastructure;

public sealed class EmailDbContext : DbContext
{
    public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Email");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureMarker).Assembly);
    }
}