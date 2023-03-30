using System.Linq.Expressions;
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
        builder.HasKey(et => et.Id);

        builder.Property(et => et.Placeholders)
            .HasConversion(
                enumerable => JsonSerializer.Serialize(
                    enumerable, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<string>>(
                    json, JsonSerializerOptions.Default) ?? new List<string>(),
                StringCollectionComparer);
    }

    private static readonly ValueComparer<IEnumerable<string>> StringCollectionComparer = new(
        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList());
}

public sealed class PlaceholderComparer : ValueComparer<IEnumerable<string>>
{
    public PlaceholderComparer(bool favorStructuralComparisons) : base(favorStructuralComparisons)
    {
    }

    public PlaceholderComparer(Expression<Func<IEnumerable<string>?, IEnumerable<string>?, bool>> equalsExpression, Expression<Func<IEnumerable<string>, int>> hashCodeExpression) : base(equalsExpression, hashCodeExpression)
    {
    }

    public PlaceholderComparer(Expression<Func<IEnumerable<string>?, IEnumerable<string>?, bool>> equalsExpression, Expression<Func<IEnumerable<string>, int>> hashCodeExpression, Expression<Func<IEnumerable<string>, IEnumerable<string>>> snapshotExpression) : base(equalsExpression, hashCodeExpression, snapshotExpression)
    {
    }
}