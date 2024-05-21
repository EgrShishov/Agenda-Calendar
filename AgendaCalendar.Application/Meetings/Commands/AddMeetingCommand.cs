
namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record AddMeetingCommand(
        string title,
        string description,
        DateTime startTime,
        DateTime endTime,
        int userId) : IRequest<ErrorOr<Meeting>> { }

    public class AddMeetingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddMeetingCommand, ErrorOr<Meeting>>
    {
        public async Task<ErrorOr<Meeting>> Handle(AddMeetingCommand request, CancellationToken cancellationToken)
        {
            var userWorkingHours = await unitOfWork.WorkHoursRepository.ListAsync(w => w.UserId == request.userId);

            if(userWorkingHours == null)
            {
                return Errors.WorkingHours.Empty;
            }

            var workingHours = userWorkingHours.FirstOrDefault();

            foreach (var dailyHour in workingHours.DailyHours)
            {
                if (dailyHour.Day == request.startTime.DayOfWeek)
                {
                    if (dailyHour.StartTime == null || dailyHour.EndTime == null)
                    {
                        return Errors.Meetings.TimeIsOutsideOfWorkingHours;
                    }

                    if (request.startTime.TimeOfDay < dailyHour.StartTime.Value ||
                        request.endTime.TimeOfDay > dailyHour.EndTime.Value)
                    {
                        return Errors.Meetings.TimeIsOutsideOfWorkingHours;
                    }
                }
            }

            var meeting = new Meeting
            {
                Title = request.title,
                Description = request.description,
                StartTime = request.startTime,
                EndTime = request.endTime,
                UserId = request.userId
            };

            return meeting;
        }
    }
}
