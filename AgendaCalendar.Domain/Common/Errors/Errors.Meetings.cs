using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Meetings
        {
            public static Error TimeIsOutsideOfWorkingHours => Error.Failure(
                code: "Meeting.WrongTime",
                description: "Meeting time is outside of working hours.");

            public static Error NotFound => Error.NotFound(
                code: "Meeting.NotFound",
                description: "Meeting not found");

            public static Error InvitationNotFound => Error.NotFound(
                code: "Meeting.InvitationNotFound",
                description: "Meeting's Invitation Not Found");

            public static Error AlreadyInvited => Error.Conflict(
                code: "Meeting.AlreadyInvited",
                description: "User already invited");
        }
    }
}
