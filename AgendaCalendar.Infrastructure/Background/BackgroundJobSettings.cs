using AgendaCalendar.Application.Emails.Commands;
using Hangfire;
using MediatR;

namespace AgendaCalendar.Infrastructure.Background
{
    public class BackgroundJobSettings
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public BackgroundJobSettings(IUnitOfWork unitOfWork, IMediator mediator)
        {
            Console.WriteLine("Mama");
            _unitOfWork = unitOfWork;
            _mediator = mediator;

            //RecurringJob.AddOrUpdate("CheckingReminder", () => CheckRemindersForSending(), Cron.MinuteInterval(1));
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
