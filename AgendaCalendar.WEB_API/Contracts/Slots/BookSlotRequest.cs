
namespace AgendaCalendar.WEB_API.Contracts.Slots
{
    public record BookSlotRequest(
        string firstName,
        string lastName,
        string email,
        string description);
}
