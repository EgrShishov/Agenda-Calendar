using System.ComponentModel.DataAnnotations;

namespace AgendaCalendar.Domain.Entities
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }// = Math.Abs(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
    }
}