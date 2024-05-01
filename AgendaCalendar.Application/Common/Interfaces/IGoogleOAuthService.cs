
namespace AgendaCalendar.Application.Common.Interfaces
{
    public interface IGoogleOAuthService
    {
        Task<User> GoogleSignIn(string idToken);
    }
}
