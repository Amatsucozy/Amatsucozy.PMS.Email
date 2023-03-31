using System.Text.Json;
using Amatsucozy.PMS.Email.Core.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amatsucozy.PMS.Email.Infrastructure.ModelConfigurations;

public sealed class EmailTemplateModelConfig : IEntityTypeConfiguration<EmailTemplate>
{
    private static readonly ValueComparer<IEnumerable<string>> StringCollectionComparer = new(
        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList());

    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasKey(et => et.Id);

        builder.Property(et => et.Placeholders)
            .HasConversion(
                enumerable => JsonSerializer.Serialize(enumerable, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<string>>(json, JsonSerializerOptions.Default)
                        ?? new List<string>(),
                StringCollectionComparer);
    }
}
