using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.WEB_API.Contracts.WorkingHours
{
    public record WorkingHoursResponse(
        DayOfWeek Day,
        List<DailyWorkingHours> DailyHours
        );
}