
namespace AgendaCalendar.Application.Slots.Queries
{
    public sealed record UserSlotsQuery(int userId) : IRequest<ErrorOr<List<Slot>>> { }

    public class UserSlotsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserSlotsQuery, ErrorOr<List<Slot>>>
    {
        public async Task<ErrorOr<List<Slot>>> Handle(UserSlotsQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Errors.User.NotFound;
            }

            var slots = await unitOfWork.SlotRepository.ListAsync(s => s.UserId == request.userId);
            if (!slots.Any())
            {
                return Errors.Slot.NotFound;
            }

            List<Slot> user_slots = slots.ToList();
            return user_slots;
        }
    }
}
