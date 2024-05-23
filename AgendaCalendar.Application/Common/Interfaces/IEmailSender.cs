
namespace AgendaCalendar.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendMessageAsync(string email, string subject, string message_body, CancellationToken cancellationToken = default);
    }
}
