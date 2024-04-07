using AgendaCalendar.Domain.Abstractions;
using AgendaCalendar.Domain.Entities;
using MediatR;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record DeleteCalendarCommand(int calendarId) : IRequest<Calendar> { }

    public class DeleteCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCalendarCommand, Calendar>
    {
        public async Task<Calendar> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null) return null;
            await unitOfWork.CalendarRepository.DeleteAsync(calendar.Id);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }

}
