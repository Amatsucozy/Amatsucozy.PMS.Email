using Amatsucozy.PMS.Email.Contracts;
using Amatsucozy.PMS.Email.Infrastructure;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;
using MassTransit;

namespace Amatsucozy.PMS.Email.Messaging.Consumers;

public sealed class SendEmailConsumer : IConsumer<EmailSendRequest>
{
    private readonly EmailDbContext _context;
    private readonly ILogger<SendEmailConsumer> _logger;
    private readonly IEmailSender _emailSender;

    public SendEmailConsumer(EmailDbContext context, ILogger<SendEmailConsumer> logger, IEmailSender emailSender)
    {
        _context = context;
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task Consume(ConsumeContext<EmailSendRequest> context)
    {
        var template = _context.Templates.FirstOrDefault(x => x.Key == context.Message.Key);

        if (template is null)
        {
            return;
        }

        if (!template.EnableMultipleReceivers)
        {
            await _emailSender.SendEmailAsync(
                context.Message.To.First(),
                template.Subject,
                template.PlainBody,
                template.Body);
            return;
        }

        await _emailSender.SendEmailsAsync(
            context.Message.To,
            context.Message.Cc,
            context.Message.Bcc,
            template.Subject,
            template.PlainBody,
            template.Body);
    }
}