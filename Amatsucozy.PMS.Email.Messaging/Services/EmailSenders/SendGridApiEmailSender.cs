using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Amatsucozy.PMS.Email.Messaging.Services.EmailSenders;

public sealed class SendGridApiEmailSender : IEmailSender
{
    private readonly ILogger<SendGridApiEmailSender> _logger;
    private readonly IOptionsMonitor<SendGridOptions> _optionsMonitor;
    private readonly ISendGridClient _sendGridClient;

    public SendGridApiEmailSender(
        ISendGridClient sendGridClient,
        IOptionsMonitor<SendGridOptions> optionsMonitor,
        ILogger<SendGridApiEmailSender> logger)
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

        var response = await _sendGridClient.SendEmailAsync(email, cancellationToken);
        
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email sent successfully");
        }
        else
        {
            _logger.LogError("Email sending failed");
            _logger.LogError(
                "Message from email provider: {responseBody}",
                await response.Body.ReadAsStringAsync(cancellationToken));
        }
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

        var response = await _sendGridClient.SendEmailAsync(email, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email sent successfully");
        }
        else
        {
            _logger.LogError("Email sending failed");
            _logger.LogError(
                "Message from email provider: {responseBody}",
                await response.Body.ReadAsStringAsync(cancellationToken));
        }
    }
}