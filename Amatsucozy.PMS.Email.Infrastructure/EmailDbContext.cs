using Amatsucozy.PMS.Email.Core.Templates;
using Microsoft.EntityFrameworkCore;

namespace Amatsucozy.PMS.Email.Infrastructure;

public sealed class EmailDbContext : DbContext
{
    public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
    {
    }

    public DbSet<EmailTemplate> Templates { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("email");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureMarker).Assembly);
    }
}