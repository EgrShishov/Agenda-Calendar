namespace AgendaCalendar.Application.Slots.Queries
{
    public sealed record GetAvaibaleSlotsQuery(string email) : IRequest<ErrorOr<List<Slot>>> { }

    public class GetAvaibaleSlotsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAvaibaleSlotsQuery, ErrorOr<List<Slot>>>
    {
        public async Task<ErrorOr<List<Slot>>> Handle(GetAvaibaleSlotsQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetUserByEmailAsync(request.email);
            if(user == null)
            {
                return Errors.User.NotFound;
            }

            var slots = await unitOfWork.SlotRepository.ListAsync(s => s.UserId == user.Id && !s.IsBooked && s.Times.Any());
            if (!slots.Any())
            {
                return Errors.Slot.NotFound;
            }

            List<Slot> avaible_slots = slots.ToList();
            return avaible_slots;
        }
    }
}
