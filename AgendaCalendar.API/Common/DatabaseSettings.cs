namespace AgendaCalendar.API.Common
{
    public class DatabaseSettings
    {
        public const string SectionName = "AgendaCalendar";
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
