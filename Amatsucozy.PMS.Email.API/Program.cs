using Amatsucozy.PMS.Email.API;
using Amatsucozy.PMS.Email.Infrastructure;
using Amatsucozy.PMS.Shared.Helpers.Extensions;
using Amatsucozy.PMS.Shared.Helpers.MessageQueues;
using Amatsucozy.PMS.Shared.Helpers.MessageQueues.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.EnvironmentName is not "Development")
{
    var arguments = Environment.GetEnvironmentVariables();

    builder.Configuration.AddFlatConfigurations<QueueOptions>(arguments);
    builder.Configuration.AddFlatConfigurations<ConnectionStrings>(arguments);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMessageQueue(builder.Configuration, typeof(ApiMarker));
var connectionString = builder.Configuration.GetConnectionString("Default") ??
                       throw new InvalidOperationException("Connection string 'Default' not found.");
builder.Services.AddDbContext<EmailDbContext>(
    options => options.UseNpgsql(
        connectionString,
        sqlBuilder => { sqlBuilder.MigrationsAssembly(typeof(InfrastructureMarker).Assembly.GetName().Name); }
    ));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.DbStart();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();