namespace AgendaCalendar.WEB_API.Contracts.Meetings
{
    public record EditMeetingRequest(
        string title,
        string description,
        DateTime startTime,
        DateTime endTime);
}
