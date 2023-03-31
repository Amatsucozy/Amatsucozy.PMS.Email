namespace Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;

public interface IEmailSender
{
    public Task SendEmailAsync(
        string recipient,
        string subject,
        string plainTextMessage,
        string htmlMessage,
        CancellationToken cancellationToken = default);

    public Task SendEmailsAsync(
        IEnumerable<string> toRecipients,
        IEnumerable<string> ccRecipients,
        IEnumerable<string> bccRecipients,
        string subject,
        string plainTextMessage,
        string htmlMessage,
        CancellationToken cancellationToken = default);
}