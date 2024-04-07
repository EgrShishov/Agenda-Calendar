
namespace AgendaCalendar.Application.Users.Commands
{
    public sealed record DeleteUserCommand(int userId) : IRequest<User> { }

    public class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, User>
    {
        public async Task<User> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (user == null) return null;
            //await unitOfWork.UserRepository.DeleteAsync(user.Id);
            await unitOfWork.SaveAllAsync();
            return user;
        }
    }
}
