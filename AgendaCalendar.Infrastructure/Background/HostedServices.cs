using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AgendaCalendar.Infrastructure.Background
{
    public class RecurringJobHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public RecurringJobHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                var reminderService = scope.ServiceProvider.GetRequiredService<BackgroundJobSettings>();

                recurringJobManager.AddOrUpdate(
                    "CheckReminders",
                    () => reminderService.CheckRemindersForSending(),
                    Cron.Minutely
                );
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
