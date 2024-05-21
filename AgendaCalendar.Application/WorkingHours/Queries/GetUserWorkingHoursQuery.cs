
namespace AgendaCalendar.Application.WorkingHours.Queries
{
    public sealed record GetUserWorkingHoursQuery(int userId) : IRequest<ErrorOr<List<Domain.Entities.WorkingHours>>> { }

    public class GetUserWorkingHoursQueryHandler(IUnitOfWork unitOfWork) 
        : IRequestHandler<GetUserWorkingHoursQuery, ErrorOr<List<Domain.Entities.WorkingHours>>>
    {
        public Task<ErrorOr<List<Domain.Entities.WorkingHours>>> Handle(GetUserWorkingHoursQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
