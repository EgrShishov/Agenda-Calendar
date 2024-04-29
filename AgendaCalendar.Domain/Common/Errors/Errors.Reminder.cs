using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Reminder
        {
            public static Error NotFound => Error.NotFound(
               code: "Reminder.NotFound",
               description: "Cannot found Reminder.");

            public static Error InvalidCalendarId => Error.Validation(
               code: "Reminder.InvalidCalendarId",
               description: "Reminder ID is invalid.");
        }
    }
}
