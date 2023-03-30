namespace Amatsucozy.PMS.Email.Contracts;

public sealed class CreateTemplateRequest
{
    public required string Key { get; init; }
    
    public required string Body { get; init; }
    
    public required IEnumerable<string> Placeholders { get; init; }
    
    public bool EnableMultipleReceivers { get; init; }
}