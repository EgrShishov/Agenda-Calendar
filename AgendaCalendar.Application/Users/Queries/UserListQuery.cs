
namespace AgendaCalendar.Application.Users.Queries
{
    public sealed record UserListQuery() : IRequest<IReadOnlyList<User>> { }

    public class UserListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserListQuery, IReadOnlyList<User>>
    {
        public async Task<IReadOnlyList<User>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var users = await unitOfWork.UserRepository.GetListAsync();
            return users;
        }
    }
}
