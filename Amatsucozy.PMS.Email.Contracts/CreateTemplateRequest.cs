namespace Amatsucozy.PMS.Email.Contracts;

public sealed class CreateTemplateRequest
{
    public required string Key { get; init; }
    
    public required string Subject { get; init; }
    
    public required string PlainBody { get; init; }

    public required string Body { get; init; }

    public bool EnableMultipleReceivers { get; init; }
}