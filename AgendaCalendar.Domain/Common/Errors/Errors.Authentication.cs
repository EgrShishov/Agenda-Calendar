using ErrorOr;

namespace AgendaCalendar.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Authentication
        {
            public static Error InvalidCredentials => Error.Validation(
                code: "Auth.InvalidCred",
                description: "Invalid credentials.");

            public static Error GenerationFailed => Error.Failure(
                code: "Auth.JwtGenerationFailed",
                description: "Cannot generate JWT token");

            public static Error GoogleAuthError => Error.Failure(
                code: "Auth.FailedToAuth",
                description: "Failed to authorize through third-party service. Google");
        }
    }
}
