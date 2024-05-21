
namespace AgendaCalendar.Domain.Entities
{
    public class Invitation : Entity
    {
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    }

}
