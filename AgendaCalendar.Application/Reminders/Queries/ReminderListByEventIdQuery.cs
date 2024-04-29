
namespace AgendaCalendar.Application.Reminders.Queries
{
    public sealed record ReminderListByEventIdQuery(int eventId) : IRequest<ErrorOr<List<Reminder>>> { }

    public class ReminderListByEventIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ReminderListByEventIdQuery, ErrorOr<List<Reminder>>>
    {
        public async Task<ErrorOr<List<Reminder>>> Handle(ReminderListByEventIdQuery request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null)
            {
                return Errors.Event.NotFound;
            }
            var reminders = await unitOfWork.ReminderRepository.ListAsync(x => x.Id.Equals(request.eventId));
            if (reminders == null)
            {
                return Errors.Reminder.NotFound;
            }
            return reminders.ToList();
        }
    }
}

