namespace AgendaCalendar.WEB_API.Contracts.Slots
{
    public record SlotResponse(
        int Id,
        bool IsBooked,
        List<TimeOnly> Times,
        DateTime Date
        );
}
