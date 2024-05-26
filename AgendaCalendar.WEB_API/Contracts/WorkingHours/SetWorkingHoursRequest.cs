
namespace AgendaCalendar.WEB_API.Contracts.WorkingHours
{
    public record SetWorkingHoursRequest(
        DayOfWeek Day,
        List<DailyHours> DailyHours
    );

    public record DailyHours(
        DayOfWeek Day,
        DateTime StartTime,
        DateTime EndTime);
}
