
namespace AgendaCalendar.Application.WorkingHours.Commands
{
    public sealed record SetWorkingHoursCommand(
        int userId,
        DayOfWeek day,
        List<DailyWorkingHours> dailyHours) : IRequest<ErrorOr<AgendaCalendar.Domain.Entities.WorkingHours>> { }

    public class SetWorkingHoursCommandHandler(IUnitOfWork unitOfWork) 
        : IRequestHandler<SetWorkingHoursCommand, ErrorOr<AgendaCalendar.Domain.Entities.WorkingHours>>
    {
        public async Task<ErrorOr<Domain.Entities.WorkingHours>> Handle(SetWorkingHoursCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Errors.User.NotFound;
            }

            var newWorkingHours = new Domain.Entities.WorkingHours
            {
                UserId = request.userId,
                Day = request.day,
                DailyHours = request.dailyHours
            };

            newWorkingHours = await unitOfWork.WorkHoursRepository.AddAsync(newWorkingHours);
            await unitOfWork.SaveAllAsync();
            return newWorkingHours;
        }
    }
}
