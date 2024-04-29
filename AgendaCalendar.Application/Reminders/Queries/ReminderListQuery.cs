
namespace AgendaCalendar.Application.Reminders.Queries
{
    public sealed record ReminderListQuery() : IRequest<ErrorOr<List<Reminder>>> { }

    public class RemidnerListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ReminderListQuery, ErrorOr<List<Reminder>>>
    {
        public async Task<ErrorOr<List<Reminder>>> Handle(ReminderListQuery request, CancellationToken cancellationToken)
        {
            var reminders = await unitOfWork.ReminderRepository.GetListAsync();
            if (reminders is null)
            {
                return Errors.Reminder.NotFound;
            }
            return reminders.ToList();
        }
    }
}
