namespace Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Options;

public sealed class SendgridOptions
{
    public required string ApiKey { get; init; }
    
    public required string From { get; init; }
    
    public required string FromName { get; init; }
}