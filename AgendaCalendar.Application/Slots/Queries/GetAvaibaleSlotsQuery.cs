using AgendaCalendar.Application.Slots.Common;

namespace AgendaCalendar.Application.Slots.Queries
{
    public sealed record GetAvaibaleSlotsQuery(string email) : IRequest<ErrorOr<ScheduleResult>> { }

    public class GetAvaibaleSlotsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAvaibaleSlotsQuery, ErrorOr<ScheduleResult>>
    {
        public async Task<ErrorOr<ScheduleResult>> Handle(GetAvaibaleSlotsQuery request, CancellationToken cancellationToken)
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

            //var schedule = await unit
            string title = "meetings schedule";
            string description = "description";

            List<Slot> avaible_slots = slots.ToList();
            return new ScheduleResult(
                avaible_slots,
                title,
                description,
                user.UserName);
        }
    }
}
