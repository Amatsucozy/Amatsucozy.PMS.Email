using Amatsucozy.PMS.Email.Core.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amatsucozy.PMS.Email.Infrastructure.ModelConfigurations;

public sealed class EmailTemplateModelConfig : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
    }
}