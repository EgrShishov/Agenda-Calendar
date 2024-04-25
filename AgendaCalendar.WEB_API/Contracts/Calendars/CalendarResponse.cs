namespace AgendaCalendar.WEB_API.Contracts.Calendars
{
    public record CalendarResponse(
        int AuthorId,
        string Title,
        string CalendarDescription);
}
