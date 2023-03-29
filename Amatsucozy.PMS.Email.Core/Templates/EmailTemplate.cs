using System.ComponentModel.DataAnnotations;

namespace Amatsucozy.PMS.Email.Core.Templates;

public sealed class EmailTemplate
{
    [Key]
    public required string Key { get; init; }
    
    public required string Body { get; init; }
    
    public required IEnumerable<string> Placeholders { get; init; }
    
    public bool EnableMultipleReceivers { get; init; }
}