using System.Text.Json;
using Amatsucozy.PMS.Email.Core.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amatsucozy.PMS.Email.Infrastructure.ModelConfigurations;

public sealed class EmailTemplateModelConfig : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
    }
}