namespace AgendaCalendar.WEB_API.Contracts.Reminders
{
    public record EditReminderRequest(
        string Description,
        string Email,
        DateTime ReminderTime,
        int NotificationInterval);
}
