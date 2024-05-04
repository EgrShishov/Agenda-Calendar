namespace AgendaCalendar.WEB_API.Contracts.Calendars
{
    public record CalendarResponse(
        int id,
        int AuthorId,
        string Title,
        string CalendarColor,
        string CalendarDescription);
}
