namespace AgendaCalendar.WEB_API.Contracts.Meetings
{
    public record CreateMeetingRequest(
        string title,
        string description,
        DateTime startTime,
        DateTime endTime
        );
}
