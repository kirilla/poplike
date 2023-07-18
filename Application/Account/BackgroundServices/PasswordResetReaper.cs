using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Poplike.Application.Account.BackgroundServices
{
    public class PasswordResetReaper : BackgroundService
    {
        private readonly IDateService _dateService;
        private readonly IServiceProvider _serviceProvider;

        public PasswordResetReaper(
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
                await ReapPasswordResets(stoppingToken);

                await Task
                    .Delay(TimeSpan.FromHours(1), stoppingToken)
                    .ConfigureAwait(false);

                // NOTE: Should we ConfigureAwait(false)?
                // I don't now. Read this?
                // https://devblogs.microsoft.com/dotnet/configureawait-faq/
            }
        }

        private async Task ReapPasswordResets(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            IDatabaseService database =
                scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            var anHourAgo = _dateService.GetDateTimeNow().AddHours(-1);

            var oldRequests = await database.PasswordResetRequests
                .Where(x => x.Created < anHourAgo)
                .ToListAsync(stoppingToken);

            database.PasswordResetRequests.RemoveRange(oldRequests);

            await database.SaveAsync(new NoUserToken());
        }
    }
}
