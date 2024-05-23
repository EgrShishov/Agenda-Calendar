
namespace AgendaCalendar.Application.Slots.Commands
{
    public sealed record GenerateSlotsCommand(int userId) : IRequest<ErrorOr<List<Slot>>> { }

    public class GenerateSlotsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<GenerateSlotsCommand, ErrorOr<List<Slot>>>
    {
        public async Task<ErrorOr<List<Slot>>> Handle(GenerateSlotsCommand request, CancellationToken cancellationToken)
        {
            var workingHours = unitOfWork.WorkHoursRepository.ListAsync(wh => wh.UserId == request.userId, cancellationToken).Result.First();

            var slots = new List<Slot>();

            foreach (var dailyHours in workingHours.DailyHours)
            {
                if (dailyHours.StartTime.HasValue && dailyHours.EndTime.HasValue)
                {
                    var date = DateTime.Today;
                    while (date.DayOfWeek != dailyHours.Day)
                    {
                        date = date.AddDays(1);
                    }

                    var currentDate = date;
                    while (currentDate <= date.AddMonths(1)) // Generate slots for a month, but in future change to user request
                    {
                        var startTime = currentDate.Date + dailyHours.StartTime.Value;
                        var endTime = currentDate.Date + dailyHours.EndTime.Value;

                        for (var time = startTime; time < endTime; time = time.AddMinutes(60)) // 60-minute slots, but in future change to user request
                        {
                            slots.Add(new Slot
                            {
                                Date = currentDate,
                                Times = new List<string> { time.ToString("HH:mm") },
                                UserId = request.userId
                            });
                        }

                        currentDate = currentDate.AddDays(7);
                    }
                }
            }

            foreach(var slot in slots)
            {
                await unitOfWork.SlotRepository.AddAsync(slot);
            }
            await unitOfWork.SaveAllAsync();

            return slots;
        }
    }
}
