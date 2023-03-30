using Amatsucozy.PMS.Email.Contracts;
using Amatsucozy.PMS.Email.Core.Templates;
using Amatsucozy.PMS.Email.Infrastructure;
using MassTransit;

namespace Amatsucozy.PMS.Email.API.Consumers;

public sealed class CreateTemplateConsumer : IConsumer<CreateTemplateRequest>
{
    private readonly EmailDbContext _context;

    public CreateTemplateConsumer(EmailDbContext context)
    {
        _context = context;
    }

    public Task Consume(ConsumeContext<CreateTemplateRequest> context)
    {
        var template = new EmailTemplate
        {
            Key = context.Message.Key,
            Body = context.Message.Body,
            Placeholders = context.Message.Placeholders,
            EnableMultipleReceivers = context.Message.EnableMultipleReceivers
        };

        _context.Templates.Add(template);
        _context.SaveChanges();

        return Task.CompletedTask;
    }
}