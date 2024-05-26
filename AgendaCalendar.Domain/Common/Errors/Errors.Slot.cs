using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Slot
        {
            public static Error NotFound => Error.NotFound(
                code: "Slot.NotFound",
                description: "Slot not found");

            public static Error AlreadyBooked => Error.Failure(
                code: "Slot.AlreadyBooked",
                description: "Slot not available or already booked");
        }
    }
}
