namespace AgendaCalendar.WEB_API.Contracts.Calendars
{
    public record CreateCalendarRequest(
        string Title,
        string CalendarDescription);
}
