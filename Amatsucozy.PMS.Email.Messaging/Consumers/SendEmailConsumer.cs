using Amatsucozy.PMS.Email.Contracts;
using Amatsucozy.PMS.Email.Infrastructure;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;
using MassTransit;

namespace Amatsucozy.PMS.Email.Messaging.Consumers;

public sealed class SendEmailConsumer : IConsumer<EmailSendRequest>
{
    private readonly EmailDbContext _context;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<SendEmailConsumer> _logger;

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

        var plainBodyBuildResult = template.BuildPlainTextTemplate(context.Message.PlaceholderMappings);

        if (!plainBodyBuildResult.IsSuccess)
        {
            throw plainBodyBuildResult.Exception!;
        }
        
        var htmlBodyBuildResult = template.BuildHtmlTemplate(context.Message.PlaceholderMappings);

        if (!htmlBodyBuildResult.IsSuccess)
        {
            throw htmlBodyBuildResult.Exception!;
        }

        if (!template.EnableMultipleReceivers)
        {
            await _emailSender.SendEmailAsync(
                context.Message.To.First(),
                template.Subject,
                plainBodyBuildResult.Value!,
                htmlBodyBuildResult.Value!);
            return;
        }

        await _emailSender.SendEmailsAsync(
            context.Message.To,
            context.Message.Cc,
            context.Message.Bcc,
            template.Subject,
            plainBodyBuildResult.Value!,
            htmlBodyBuildResult.Value!);
    }
}