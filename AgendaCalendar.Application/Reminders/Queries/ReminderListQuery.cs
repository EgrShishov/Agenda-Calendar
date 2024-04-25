
namespace AgendaCalendar.Application.Reminders.Queries
{
    public sealed record ReminderListQuery() : IRequest<IReadOnlyList<Reminder>> { }

    public class RemidnerListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ReminderListQuery, IReadOnlyList<Reminder>>
    {
        public async Task<IReadOnlyList<Reminder>> Handle(ReminderListQuery request, CancellationToken cancellationToken)
        {
            var reminders = await unitOfWork.ReminderRepository.GetListAsync();

            return reminders;
        }
    }
}
