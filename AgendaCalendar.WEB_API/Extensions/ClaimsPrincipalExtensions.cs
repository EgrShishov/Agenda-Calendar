using System.Security.Claims;

namespace AgendaCalendar.WEB_API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return user.FindFirstValue(ClaimTypes.Name);
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return int.TryParse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out int id) ? id : 0;
        }
    }
}
