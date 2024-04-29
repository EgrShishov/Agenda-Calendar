using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Calendar
        {
            public static Error NotFound => Error.NotFound(
               code: "Calendar.NotFound",
               description: "Cannot found calendar.");

            public static Error InvalidCalendarId => Error.Validation(
               code: "Calendar.InvalidCalendarId",
               description: "Calendar ID is invalid.");

            public static Error SerializationError => Error.Unexpected(
               code: "Calendar.SerializationError",
               description: "Some problems with calendar serailization");
        }
    }
}
