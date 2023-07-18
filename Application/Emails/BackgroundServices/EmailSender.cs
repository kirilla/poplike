using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poplike.Common.Settings;

namespace Poplike.Application.Emails.BackgroundServices;

public class EmailSender : BackgroundService
{
    private readonly IDateService _dateService;
    private readonly ILogger<EmailSender> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly EmailAccountConfiguration _config;

    public EmailSender(
        IDateService dateService,
        ILogger<EmailSender> logger,
        IServiceProvider serviceProvider,
        IOptions<EmailAccountConfiguration> options)
    {
        _dateService = dateService;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_config.Active)
                await SendBatch(stoppingToken);

            await Task
                .Delay(TimeSpan.FromHours(1), stoppingToken)
                .ConfigureAwait(false);

            // NOTE: Should we ConfigureAwait(false)?
            // I don't now. Read this?
            // https://devblogs.microsoft.com/dotnet/configureawait-faq/
        }
    }

    private async Task SendBatch(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        IDatabaseService database =
            scope.ServiceProvider.GetRequiredService<IDatabaseService>();

        ISmtpService smtpService =
            scope.ServiceProvider.GetRequiredService<ISmtpService>();

        var hourlyRateLimit = 60;

        var anHourAgo = _dateService.GetDateTimeNow().AddHours(-1);

        var sentLastHour = await database.Emails
            .CountAsync(x =>
                x.Status == EmailStatus.Sent &&
                x.Sent > anHourAgo,
                stoppingToken);

        var unsetEmails = await database.Emails
            .Where(x => x.Status == EmailStatus.NotSent)
            .Take(hourlyRateLimit - sentLastHour)
            .ToListAsync(stoppingToken);

        foreach (var email in unsetEmails)
        {
            try
            {
                smtpService.SendMessage(email);

                email.Status = EmailStatus.Sent;
                email.Sent = _dateService.GetDateTimeNow();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Fel i EmailSender: {exception}", e.Message);

                email.Status = EmailStatus.SendFailed;
            }

            if (stoppingToken.IsCancellationRequested)
                break;
        }

        await database.SaveAsync(new NoUserToken());
    }
}
