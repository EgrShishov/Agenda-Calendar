
namespace AgendaCalendar.Infrastructure.Authorization
{
    public class GoogleOAuthSettings
    {
        public const string SectionName = "GoogleOAuthSettings";
        public string Secret { get; init; } = null!;
        public string ClientId { get; set; } = null!;
    }
}
