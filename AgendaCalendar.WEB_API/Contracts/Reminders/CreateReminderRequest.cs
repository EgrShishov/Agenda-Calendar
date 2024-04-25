namespace AgendaCalendar.WEB_API.Contracts.Reminders
{
    public record CreateReminderRequest(
        string Description,
        string Email,
        DateTime ReminderTime,
        TimeSpan NotificationInterval);
}
