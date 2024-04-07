
namespace AgendaCalendar.Domain.Entities
{
    public class RecurrenceRule //analog of reccurency pattern in ical.net
    {
        public RecurrenceFrequency Frequency { get; set; } = RecurrenceFrequency.None;

        /// <summary>
        /// Specifies how often the recurrence should repeat.
        /// 1-based index (e.g., 1st, 2nd, 3rd, 4th, -1 for last)
        /// </summary>
        public int Interval { get; set; } = 0;
        public List<RecurrenceDayOfWeek> DaysOfWeek { get; set; } = new();
        public List<int> DaysOfMonth { get; set; } = new();
        public List<int> WeeksOfMonth { get; set; } = new();  // For example, first, second, third, fourth, or last week of the month
        public List<int> MonthsOfYear { get; set; } = new(); // For example, January, February, March, etc.
        public int Year { get; set; } = 0; // Yearly recurrence
        public List<TimePeriod> RecurrenceDates { get; set; } = new();
    }
}
