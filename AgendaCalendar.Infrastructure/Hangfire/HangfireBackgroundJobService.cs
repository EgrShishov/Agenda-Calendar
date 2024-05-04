using AgendaCalendar.Application.Emails.Commands;
using Hangfire;
using MediatR;

namespace AgendaCalendar.Infrastructure.Hangfire
{
    public class HangfireBackgroundJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public HangfireBackgroundJobService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            Console.WriteLine("Hangfire background job service started...");
        }

        public void ScheduleReminderJob(Reminder reminder)
        {
            RecurringJob.AddOrUpdate("my-recurring-job",() => CheckRemindersForSending(), Cron.MinuteInterval(1));
            Console.WriteLine("sended");
        }

        public async Task<IReadOnlyList<Reminder>> CheckRemindersForSending()
        {
            DateTime currentTime = DateTime.Now;
            List<Reminder> remindersToSend = new List<Reminder>();

            foreach (var reminder in await _unitOfWork.ReminderRepository.GetListAsync())
            {
                if (reminder.ReminderTime - currentTime < TimeSpan.FromHours(1))
                {
                    remindersToSend.Add(reminder);
                }
            }
            Console.WriteLine(remindersToSend.Count);
            foreach (var reminder in remindersToSend)
            {
               await _mediator.Send(new SendReminderCommand(reminder));
                await _unitOfWork.ReminderRepository.DeleteAsync(reminder.Id);
            }

            await _unitOfWork.SaveAllAsync();
            return remindersToSend;
        }
    }
}
