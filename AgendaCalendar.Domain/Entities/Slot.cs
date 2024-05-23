
namespace AgendaCalendar.Domain.Entities
{
    public class Slot : Entity
    {
        public DateTime Date { get; set; }
        public List<string> Times { get; set; } = new();
        public int UserId {  get; set; }
        public bool IsBooked { get; set; } = false;
    }
}
