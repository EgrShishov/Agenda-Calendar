namespace AgendaCalendar.WEB_API.Contracts.Slots
{
    public record ScheduleResponse(
        List<SlotResponse> slots,
        string title,
        string description,
        string userName);
}
