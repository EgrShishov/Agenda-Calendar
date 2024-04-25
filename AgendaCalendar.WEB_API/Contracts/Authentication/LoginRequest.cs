namespace AgendaCalendar.WEB_API.Contracts.Authentication
{
    public record LoginRequest(
        string Email,
        string Password);
}
