namespace AgendaCalendar.WEB_API.Contracts.Reminders
{
    public record ReminderResponse(
        int ReminderId,
        int EventId,
        string Title,
        string Description,
        string Email,
        DateTime ReminderTime,
        TimeSpan NotificationInterval);
}
