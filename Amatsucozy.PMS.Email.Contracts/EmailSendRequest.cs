namespace Amatsucozy.PMS.Email.Contracts;

public sealed class EmailSendRequest
{
    public required string Key { get; init; }
    
    public required IDictionary<string, string> PlaceholderMappings { get; init; }
    
    public required IEnumerable<string> To { get; init; }
    
    public required IEnumerable<string> Cc { get; init; }
    
    public required IEnumerable<string> Bcc { get; init; }
}