
namespace AgendaCalendar.Application.Calendars.Queries
{
    public sealed record GetUserSubscriptionsQuery(int userId) : IRequest<ErrorOr<List<Calendar>>> { }

    public class GetUserSubscriptionsQueryHandelr(IUnitOfWork unitOfWork) : IRequestHandler<GetUserSubscriptionsQuery, ErrorOr<List<Calendar>>>
    {
        public async Task<ErrorOr<List<Calendar>>> Handle(GetUserSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Errors.User.NotFound;
            }

            var subscribed_calendars = await unitOfWork.CalendarRepository.ListAsync(x => x.SubscribersId.Contains(request.userId));
            if (subscribed_calendars == null)
            {
                return Errors.Calendar.NotFound;
            }
            return subscribed_calendars.ToList();
        }
    }
}
