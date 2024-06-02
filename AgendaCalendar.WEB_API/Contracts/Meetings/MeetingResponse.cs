using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.WEB_API.Contracts.Meetings
{
    public record MeetingResponse(
        int id,
        string title,
        string description,
        DateTime startTime,
        DateTime endTime,
        InvitationStatus InvitationStatus);
}