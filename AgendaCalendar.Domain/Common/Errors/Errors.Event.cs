using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Event
        {
            public static Error NotFound => Error.NotFound(
               code: "Event.NotFound",
               description: "Cannot found event.");

            public static Error InvalidCalendarId => Error.Validation(
               code: "Event.InvalidCalendarId",
               description: "Event ID is invalid.");
        }
    }
}
