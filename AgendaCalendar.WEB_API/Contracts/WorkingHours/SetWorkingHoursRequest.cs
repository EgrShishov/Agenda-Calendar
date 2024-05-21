namespace AgendaCalendar.WEB_API.Contracts.WorkingHours
{
    public record SetWorkingHoursRequest(
        DayOfWeek Day,
        Dictionary<DayOfWeek, TimeSpan> StartTimes,
        Dictionary<DayOfWeek, TimeSpan> EndTimes);
}
