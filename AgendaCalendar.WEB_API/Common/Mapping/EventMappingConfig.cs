using AgendaCalendar.Application.Events.Commands;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Events;
using Mapster;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public class EventMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(CreateEventRequest request, int calendarId, int authorId), AddEventCommand>()
                .Map(dest => dest.CalednarId, src => src.calendarId)
                .Map(dest => dest.AuthorId, src => src.authorId)
                .Map(dest => dest.Title, src => src.request.Title)
                .Map(dest => dest.Description, src => src.request.Description)
                .Map(dest => dest.StartTime, src => src.request.StartTime)
                .Map(dest => dest.EndTime, src => src.request.EndTime)
                .Map(dest => dest.RecurrenceRule, src => src.request.RecurrenceRule)
                .Map(dest => dest.Location, src => src.request.Location);

            config.NewConfig<(EditEventRequest request, int eventId, int authorId), UpdateEventCommand>()
                .Map(dest => dest.CalendarId, src => src.request.CalendarId)
                .Map(dest => dest.EventId, src => src.eventId)
                .Map(dest => dest.AuthorId, src => src.authorId)
                .Map(dest => dest.Title, src => src.request.Title)
                .Map(dest => dest.Description, src => src.request.Description)
                .Map(dest => dest.StartTime, src => src.request.StartTime)
                .Map(dest => dest.EndTime, src => src.request.EndTime)
                .Map(dest => dest.RecurrenceRule, src => src.request.RecurrenceRule)
                .Map(dest => dest.Location, src => src.request.Location);

            config.NewConfig<Event, EventResponse>();
        }
    }
}
