using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Invitations
        {
            public static Error NotFound => Error.NotFound(
                code: "Invintation.NotFound",
                description:"Invintation not found");
        }
    }
}
