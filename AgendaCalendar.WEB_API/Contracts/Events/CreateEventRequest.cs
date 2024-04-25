using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.WEB_API.Contracts.Events
{
    public record CreateEventRequest(
        string Title,
        string Description,
        string Location,
        DateTime StartTime,
        DateTime EndTiem,
        RecurrenceRule RecurrenceRule);
}
