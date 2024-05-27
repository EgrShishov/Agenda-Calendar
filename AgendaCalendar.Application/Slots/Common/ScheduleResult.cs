
namespace AgendaCalendar.Application.Slots.Common
{
    public record ScheduleResult(
        List<Slot> slots,
        string title,
        string description,
        string userName);
}
