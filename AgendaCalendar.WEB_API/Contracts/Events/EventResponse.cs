using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.WEB_API.Contracts.Events
{
    public record EventResponse(
        string Title,
        string Description,
        string Location,
        DateTime StartTime,
        DateTime EndTiem,
        RecurrenceRule RecurrenceRule,
        int CalendarId,
        int AuthorId);
}
