using Amatsucozy.PMS.Shared.Core.Modelling;

namespace Amatsucozy.PMS.Email.Core.Templates;

public sealed class EmailTemplate : Entity
{
    public required string Key { get; init; }
    
    public required string Subject { get; init; }
    
    public required string PlainBody { get; init; }

    public required string Body { get; init; }

    public required IEnumerable<string> Placeholders { get; init; }

    public bool EnableMultipleReceivers { get; init; }
}