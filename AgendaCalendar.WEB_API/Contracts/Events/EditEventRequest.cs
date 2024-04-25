using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.WEB_API.Contracts.Events
{
    public record EditEventRequest(
        string Title,
        string Description,
        string Location,
        DateTime StartTime,
        DateTime EndTime,
        RecurrenceRule RecurrenceRule,
        int CalendarId);
}
