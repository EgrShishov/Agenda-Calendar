namespace AgendaCalendar.WEB_API.Contracts.Reminders
{
    public record EditReminderRequest(
        int EventId,
        string Description,
        string Email,
        DateTime ReminderTime,
        TimeSpan NotificationInterval);
}
