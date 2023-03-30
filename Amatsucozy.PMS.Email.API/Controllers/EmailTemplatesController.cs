using Amatsucozy.PMS.Email.Contracts;
using Amatsucozy.PMS.Shared.API.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Amatsucozy.PMS.Email.API.Controllers;

public class EmailTemplatesController : PublicController
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EmailTemplatesController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate(CreateTemplateRequest createTemplateRequest, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(createTemplateRequest, cancellationToken);

        return Ok();
    }
}