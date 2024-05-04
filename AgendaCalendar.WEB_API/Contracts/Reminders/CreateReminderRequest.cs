namespace AgendaCalendar.WEB_API.Contracts.Reminders
{
    public record CreateReminderRequest(
        int eventId,
        string Description,
        string Email,
        DateTime ReminderTime,
        TimeSpan NotificationInterval);
}
