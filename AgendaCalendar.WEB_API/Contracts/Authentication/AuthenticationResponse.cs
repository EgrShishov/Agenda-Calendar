namespace AgendaCalendar.WEB_API.Contracts.Authentication
{
    public record AuthenticationResponse(
        int Id,
        string Username,
        string Email,
        string Token);
}
