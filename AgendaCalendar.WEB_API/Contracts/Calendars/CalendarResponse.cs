namespace AgendaCalendar.WEB_API.Contracts.Calendars
{
    public record CalendarResponse(
        int id,
        int authorId,
        string title,
        string calendarColor,
        string calendarDescription);
}
