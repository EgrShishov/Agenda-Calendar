using AgendaCalendar.Application.Reminders.Commands;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Reminders;
using Mapster;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public class ReminderMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(CreateReminderRequest request, int eventId), AddReminderCommand>()
                .Map(dest => dest.desc, src => src.request.Description)
                .Map(dest => dest.emailAdress, src => src.request.Email)
                .Map(dest => dest.eventId, src => src.eventId)
                .Map(dest => dest.time, src => src.request.ReminderTime)
                .Map(dest => dest.interval, src => TimeSpan.FromMinutes(src.request.NotificationInterval));

            config.NewConfig<(EditReminderRequest request, int eventId), UpdateReminderCommand>()
                .Map(dest => dest.Description, src => src.request.Description)
                .Map(dest => dest.Email, src => src.request.Email)
                .Map(dest => dest.EventId, src => src.eventId)
                .Map(dest => dest.ReminderTime, src => src.request.ReminderTime)
                .Map(dest => dest.NotificationInterval, src => TimeSpan.FromMinutes(src.request.NotificationInterval));

            config.NewConfig<Reminder, ReminderResponse>();
        }
    }
}
