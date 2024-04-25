namespace AgendaCalendar.WEB_API.Contracts.Authentication
{
    public record RegisterRequest(
        string Username,
        string Email,
        string Password,
        DateTime BirthdayDate);
}
