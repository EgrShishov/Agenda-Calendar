
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaCalendar.Domain.Entities
{
    public class Event : Entity
    {
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        public int AuthorId { get; set; }

        public List<EventParticipant> EventParticipants { get; set; } = new();

        public string Location { get; set; }

        public RecurrenceRule ReccurenceRules { get; set; } = new();

        public override string ToString()
        {
            return $"Event : {Title}, starts : {StartTime}, Desc: {Description}";
        }

        public void AddParticipant(EventParticipant participant)
        {
            if (participant != null) EventParticipants.Add(participant);
        }

        public void RemoveParticipant(EventParticipant participant)
        {
            if (participant != null) EventParticipants.Remove(participant);
        }

    }
}