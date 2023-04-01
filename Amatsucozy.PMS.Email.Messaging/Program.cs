using Amatsucozy.PMS.Email.Infrastructure;
using Amatsucozy.PMS.Email.Messaging;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Interfaces;
using Amatsucozy.PMS.Email.Messaging.Services.EmailSenders.Options;
using Amatsucozy.PMS.Shared.Helpers.Extensions;
using Amatsucozy.PMS.Shared.Helpers.MessageQueues;
using Amatsucozy.PMS.Shared.Helpers.MessageQueues.Configurations;
using Microsoft.EntityFrameworkCore;
using SendGrid.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureHostConfiguration(configurationBuilder => { configurationBuilder.AddEnvironmentVariables(); })
    .ConfigureAppConfiguration(configurationBuilder => { configurationBuilder.AddUserSecrets<MessagingMarker>(); })
    .ConfigureServices((context, serviceCollection) =>
    {
        if (context.HostingEnvironment.EnvironmentName is not "Development")
        {
            var arguments = Environment.GetEnvironmentVariables();

            context.Configuration.AddFlatConfigurations<QueueOptions>(arguments);
            context.Configuration.AddFlatConfigurations<ConnectionStrings>(arguments);
        }

        serviceCollection.AddMessageQueue(context.Configuration, typeof(MessagingMarker));
        var connectionString = context.Configuration.GetConnectionString("Default") ??
                               throw new InvalidOperationException("Connection string 'Default' not found.");
        serviceCollection.AddDbContext<EmailDbContext>(
            options => options.UseNpgsql(
                connectionString,
                sqlBuilder => { sqlBuilder.MigrationsAssembly(typeof(InfrastructureMarker).Assembly.GetName().Name); }
            ));

        serviceCollection.AddSendGrid(options =>
        {
            options.ApiKey = context.Configuration
                .GetSection(nameof(SendGridOptions))
                .GetValue<string>(nameof(SendGridOptions.ApiKey));
        });
        serviceCollection.AddScoped<IEmailSender, SendGridApiEmailSender>();
    });

var host = builder.Build();

host.DbStart();

host.Run();