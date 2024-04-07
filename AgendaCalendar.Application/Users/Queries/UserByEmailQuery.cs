
namespace AgendaCalendar.Application.Users.Queries
{
    namespace AgendaCalendar.Application.Users.Queries
    {
        public sealed record UserByEmailQuery(string email) : IRequest<User> { }

        public class UserByEmailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserByEmailQuery, User>
        {
            public async Task<User> Handle(UserByEmailQuery request, CancellationToken cancellationToken)
            {
                var users = await unitOfWork.UserRepository.ListAsync( u => u.Email.Equals(request.email));
                return users.FirstOrDefault();
            }
        }
    }
}
