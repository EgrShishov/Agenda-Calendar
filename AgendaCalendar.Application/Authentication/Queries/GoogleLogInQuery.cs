using AgendaCalendar.Application.Authentication.Common;
using AgendaCalendar.Application.Common.Interfaces;

namespace AgendaCalendar.Application.Authentication.Queries
{
    public sealed record GoogleSigninQuery(string tokenId) : IRequest<ErrorOr<AuthenticationResult>> { }

    public class GoogleSigninQueryHandler(IUnitOfWork unitOfWork, IGoogleOAuthService googleOAuthService, IJwtTokenGenerator jwtTokenGenerator) 
        : IRequestHandler<GoogleSigninQuery, ErrorOr<AuthenticationResult>>
    {
        public async Task<ErrorOr<AuthenticationResult>> Handle(GoogleSigninQuery request, CancellationToken cancellationToken)
        {
            var user = await googleOAuthService.GoogleSignIn(request.tokenId);
            if (user == null)
            {
                return Errors.Authentication.GoogleAuthError;
            }

            var token = jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email);
            if (token == null)
            {
                return Errors.Authentication.GenerationFailed;
            }
            return new AuthenticationResult(
                user,
                token);
        }
    }
}
