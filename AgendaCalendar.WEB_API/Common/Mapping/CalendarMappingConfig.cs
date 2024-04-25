using AgendaCalendar.Application.Calendars.Commands;
using AgendaCalendar.Application.Events.Commands;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Calendars;
using Mapster;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public class CalendarMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(CreateCalendarRequest Request, int authorId), CreateCalendarCommand>()
                .Map(dest => dest.title, src => src.Request.Title)
                .Map(dest => dest.description, src => src.Request.CalendarDescription)
                .Map(dest => dest.authorId, src => src.authorId);

            config.NewConfig<(EditCalendarRequest Request, int calendarId), EditCalendarCommand>()
                .Map(dest => dest.title, src => src.Request.Title)
                .Map(dest => dest.description, src => src.Request.CalendarDescription)
                .Map(dest => dest.calendarId, src => src.calendarId);

            config.NewConfig<Calendar, CalendarResponse>()
                .Map(dest => dest.AuthorId, src => src.AuthorId)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.CalendarDescription, src => src.CalendarDescription);

        }
    }
}
