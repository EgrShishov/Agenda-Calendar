using AgendaCalendar.Application.Authentication.Common;
using AgendaCalendar.Application.Common.Interfaces;

namespace AgendaCalendar.Application
{
    public sealed record SignUpCommand(string Username, string Password, string Email, DateTime BirthdayDate) : IRequest<ErrorOr<AuthenticationResult>> { }

    public class SignupCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<SignUpCommand, ErrorOr<AuthenticationResult>>
    {
        public async Task<ErrorOr<AuthenticationResult>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var userWithEmail = await unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (userWithEmail != null)
            {
                return Errors.User.DuplicateEmail;
            }
            var userWithUsername = await unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
            if(userWithUsername != null)
            {
                return Errors.User.DuplicateUsername;
            }
            var newUser = new User
            {
                UserName = request.Username,
                Email = request.Email,
                BirthdayDate = request.BirthdayDate
            };
            var user = await unitOfWork.UserRepository.AddAsync(newUser, request.Password);
            var userCalendar = new Calendar
            {
                Title = request.Username,
                CalendarDescription = $"Basic calendar, which belongs to {request.Username}",
                AuthorId = user.Id
            };
            await unitOfWork.CalendarRepository.AddAsync(userCalendar);
            await unitOfWork.SaveAllAsync();

            var token = jwtTokenGenerator.GenerateToken(newUser.Id, newUser.UserName, newUser.Email);
            return new AuthenticationResult(
                user,
                token);
        }
    }
}
