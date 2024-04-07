
namespace AgendaCalendar.Application.Users.Queries
{
    public sealed record UserByIdQuery(int userId) : IRequest<User> { }

    public class UserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserByIdQuery, User>
    {
        public async Task<User> Handle(UserByIdQuery request, CancellationToken cancellationToken)
        {
            var users = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            return users;
        }
    }
}
