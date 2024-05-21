
namespace AgendaCalendar.Application.Meetings.Queries
{
    public sealed record UserInvitationsQuery(int userId) : IRequest<ErrorOr<List<Invitation>>> { }

    public class UserInvitationsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserInvitationsQuery, ErrorOr<List<Invitation>>>
    {
        public async Task<ErrorOr<List<Invitation>>> Handle(UserInvitationsQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Errors.User.NotFound;
            }

            var invitations = await unitOfWork.InvitationRepository
                .ListAsync(i => i.UserId == request.userId && i.Status == InvitationStatus.Pending);

            if (invitations == null || !invitations.Any())
            {
                return Errors.Invitations.NotFound;
            }

            return invitations.ToList();
        }
    }
}
