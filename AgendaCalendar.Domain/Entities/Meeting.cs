
namespace AgendaCalendar.Domain.Entities
{
    public class Meeting : Entity
    { 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<User> Participants { get; set; } = new();
        public bool IsCancelled { get; set; } = false;
        public List<Invitation> Invitations { get; set; } = new();
    }
}
