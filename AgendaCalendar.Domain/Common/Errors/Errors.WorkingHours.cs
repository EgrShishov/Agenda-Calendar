
using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class WorkingHours
        {
            public static Error Empty => Error.Unexpected(
                code: "WorkingHours.Empty",
                description: "Empty working hours");
        }
    }
}
