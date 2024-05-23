namespace AgendaCalendar.WEB_API.Contracts.Meetings
{
    public record MeetingResponse(
        string title,
        string description,
        DateTime startTime,
        DateTime endTime);
}