using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poplike.Application.Sessions.Queues;

namespace Poplike.Application.Sessions.BackgroundServices;

public class SessionActivityLogger : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SessionActivityLogger(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SaveEvents(stoppingToken);
            }
            catch (Exception ex)
            {
                // TODO: Log event?
            }
            
            await Task
                .Delay(TimeSpan.FromMinutes(1), stoppingToken)
                .ConfigureAwait(false);

            // NOTE: Should we ConfigureAwait(false)?
            // I don't now. Read this?
            // https://devblogs.microsoft.com/dotnet/configureawait-faq/
        }
    }

    private async Task SaveEvents(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        ISessionActivityList activityMap =
            scope.ServiceProvider.GetRequiredService<ISessionActivityList>();

        var activeSessionIds = activityMap.RemoveSessionIds();

        if (activeSessionIds.Count == 0)
            return;

        IDatabaseService database =
            scope.ServiceProvider.GetRequiredService<IDatabaseService>();

        var storedSessionId = database.Sessions
            .Select(x => x.Id)
            .Distinct()
            .ToList();

        var idsToLog = storedSessionId
            .Intersect(activeSessionIds)
            .Distinct()
            .ToList();

        var sessionActivities = idsToLog
            .Select(x => new SessionActivity()
            {
                SessionId = x,
            })
            .ToList();

        database.SessionActivities.AddRange(sessionActivities);

        await database.SaveAsync(new NoUserToken());
    }
}
