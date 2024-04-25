
namespace AgendaCalendar.Application.Authentication.Common
{
    public record AuthenticationResult(
        User user,
        string Token);
}
