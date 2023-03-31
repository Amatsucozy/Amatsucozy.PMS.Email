namespace Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;

public interface IEmailSender
{
    public Task SendEmailAsync(
        IEnumerable<string> to,
        IEnumerable<string> cc,
        IEnumerable<string> bcc,
        string subject,
        string plainTextMessage,
        string htmlMessage,
        CancellationToken cancellationToken = default);
}