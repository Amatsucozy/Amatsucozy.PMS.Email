using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Amatsucozy.PMS.Email.Messaging.Services.EmailSenders;

public sealed class SendgridApiEmailSender : IEmailSender
{
    private readonly ILogger<SendgridApiEmailSender> _logger;
    private readonly IOptionsMonitor<SendgridOptions> _optionsMonitor;
    private readonly SendGridClient _sendGridClient;

    public SendgridApiEmailSender(
        SendGridClient sendGridClient,
        IOptionsMonitor<SendgridOptions> optionsMonitor,
        ILogger<SendgridApiEmailSender> logger)
    {
        _sendGridClient = sendGridClient;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    public async Task SendEmailAsync(
        string recipient,
        string subject,
        string plainTextMessage,
        string htmlMessage,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            _logger.LogWarning("Recipient is null or empty");
            return;
        }

        var email = MailHelper.CreateSingleEmail(
            new EmailAddress(_optionsMonitor.CurrentValue.From, _optionsMonitor.CurrentValue.FromName),
            new EmailAddress(recipient),
            subject,
            plainTextMessage,
            htmlMessage);

        await _sendGridClient.SendEmailAsync(email, cancellationToken);
    }

    public async Task SendEmailsAsync(
        IEnumerable<string> toRecipients,
        IEnumerable<string> ccRecipients,
        IEnumerable<string> bccRecipients,
        string subject,
        string plainTextMessage,
        string htmlMessage,
        CancellationToken cancellationToken = default)
    {
        var email = MailHelper.CreateSingleEmailToMultipleRecipients(
            new EmailAddress(_optionsMonitor.CurrentValue.From, _optionsMonitor.CurrentValue.FromName),
            toRecipients.Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => new EmailAddress(r)).ToList(),
            subject,
            plainTextMessage,
            htmlMessage);

        if (ccRecipients.Any())
        {
            email.AddCcs(ccRecipients.Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => new EmailAddress(r)).ToList());
        }

        if (bccRecipients.Any())
        {
            email.AddBccs(bccRecipients.Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => new EmailAddress(r)).ToList());
        }

        await _sendGridClient.SendEmailAsync(email, cancellationToken);
    }
}