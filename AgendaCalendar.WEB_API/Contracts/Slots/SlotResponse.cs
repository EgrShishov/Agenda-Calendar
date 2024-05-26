namespace AgendaCalendar.WEB_API.Contracts.Slots
{
    public record SlotResponse(
        bool IsBooked,
        List<TimeOnly> Times,
        DateTime Date
        );
}
