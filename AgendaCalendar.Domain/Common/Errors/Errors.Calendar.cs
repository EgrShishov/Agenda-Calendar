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

            public static Error InvalidUrl => Error.Validation(
                code: "Calendar.InvalidUrl",
                description: "Seems to be the provided url is invalid :(");

            public static Error AlreadySubscribed => Error.Failure(
                code: "Calendar.SubscribtionExist",
                description: "You are already subscribed to this calendar");

            public static Error SubscriberIsAuthor => Error.Failure(
                code: "Calendar.SubscribeError",
                description: "You cannot subscribe to your own calendar");
        }
    }
}
