using AgendaCalendar.Application.Authentication.Common;
using AgendaCalendar.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;

namespace AgendaCalendar.Application
{
    public sealed record LoginQuery(string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>> { }

    public class LoginQueryHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, UserManager<User> userManager) : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return Errors.User.NotFound;
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Errors.Authentication.InvalidCredentials;
            }
            var token = jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email);
            return new AuthenticationResult(
                user,
                token);
        }
    }
}
