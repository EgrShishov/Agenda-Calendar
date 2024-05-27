
namespace AgendaCalendar.Application.Users.Queries
{
    namespace AgendaCalendar.Application.Users.Queries
    {
        public sealed record UserByEmailQuery(string email) : IRequest<ErrorOr<User>> { }

        public class UserByEmailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserByEmailQuery,ErrorOr<User>>
        {
            public async Task<ErrorOr<User>> Handle(UserByEmailQuery request, CancellationToken cancellationToken)
            {
                var users = await unitOfWork.UserRepository.ListAsync(u => u.Email.Equals(request.email));
                if (!users.Any())
                {
                    return Errors.User.NotFound;
                }
                return users.FirstOrDefault();
            }
        }
    }
}
