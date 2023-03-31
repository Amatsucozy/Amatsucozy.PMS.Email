using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Amatsucozy.PMS.Email.Messaging.Services.EmailSenders;

public sealed class SendgridApiEmailSender : IEmailSender
{
    private readonly SendGridClient _sendGridClient;
    private readonly IOptionsMonitor<SendgridOptions> _optionsMonitor;

    public SendgridApiEmailSender(SendGridClient sendGridClient, IOptionsMonitor<SendgridOptions> optionsMonitor)
    {
        _sendGridClient = sendGridClient;
        _optionsMonitor = optionsMonitor;
    }

    public async Task SendEmailAsync(
        IEnumerable<string> to,
        IEnumerable<string> cc,
        IEnumerable<string> bcc,
        string subject,
        string plainTextMessage,
        string htmlMessage,
        CancellationToken cancellationToken = default)
    {
        var email = MailHelper.CreateSingleEmailToMultipleRecipients(
            new EmailAddress(_optionsMonitor.CurrentValue.From, _optionsMonitor.CurrentValue.FromName),
            to.Select(emailString => new EmailAddress(emailString)).ToList(),
            subject,
            plainTextMessage,
            htmlMessage);

        await _sendGridClient.SendEmailAsync(email, cancellationToken);
    }
}