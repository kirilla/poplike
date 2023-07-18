using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Poplike.Application.Sessions.BackgroundServices
{
    public class SessionReaper : BackgroundService
    {
        private readonly IDateService _dateService;
        private readonly IServiceProvider _serviceProvider;

        public SessionReaper(
            IDateService dateService,
            IServiceProvider serviceProvider)
        {
            _dateService = dateService;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ReapSessions(stoppingToken);
                }
                catch (Exception ex)
                {
                    // TODO: Use the plain logger
                }
                
                await Task
                    .Delay(TimeSpan.FromMinutes(5), stoppingToken)
                    .ConfigureAwait(false);

                // NOTE: Should we ConfigureAwait(false)?
                // I don't now. Read this?
                // https://devblogs.microsoft.com/dotnet/configureawait-faq/
            }
        }

        private async Task ReapSessions(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            IDatabaseService database =
                scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            var yesterday = _dateService.GetDateTimeNow().AddDays(-1);

            var staleSessions = await database.Sessions
                .Where(x => 
                    x.Created < yesterday &&
                    !x.SessionActivities.Any(y => y.Created > yesterday))
                .ToListAsync(stoppingToken);

            database.Sessions.RemoveRange(staleSessions);

            await database.SaveAsync(new NoUserToken());

            // Prune 
            var activitiesToRemove = new List<SessionActivity>();

            var activities = await database.SessionActivities.ToListAsync();

            var groups = activities
                .GroupBy(x => x.SessionId)
                .Where(x => x.Count() > 1)
                .ToList();

            foreach (var group in groups)
            {
                var redundant = group
                    .OrderBy(x => x.Created)
                    .SkipLast(1)
                    .ToList();

                activitiesToRemove.AddRange(redundant);
            }

            database.SessionActivities.RemoveRange(activitiesToRemove);

            await database.SaveAsync(new NoUserToken());
        }
    }
}
