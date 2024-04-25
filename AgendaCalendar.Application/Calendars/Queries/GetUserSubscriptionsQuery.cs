
namespace AgendaCalendar.Application.Calendars.Queries
{
    public sealed record GetUserSubscriptionsQuery(int userId) : IRequest<IReadOnlyList<Calendar>> { }

    public class GetUserSubscriptionsQueryHandelr(IUnitOfWork unitOfWork) : IRequestHandler<GetUserSubscriptionsQuery, IReadOnlyList<Calendar>>
    {
        public async Task<IReadOnlyList<Calendar>> Handle(GetUserSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (user == null) return null;

            var subscribed_calendars = await unitOfWork.CalendarRepository.ListAsync(x => x.SubscribersId.Contains(request.userId));
            if (subscribed_calendars == null) return null;
            return subscribed_calendars;
        }
    }
}
