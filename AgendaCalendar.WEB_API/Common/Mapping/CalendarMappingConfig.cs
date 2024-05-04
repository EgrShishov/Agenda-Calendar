using AgendaCalendar.Application.Calendars.Commands;
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
                .Map(dest => dest.authorId, src => src.authorId)
                .Map(dest => dest.calendarColor, src => src.Request.CalendarColor);

            config.NewConfig<(EditCalendarRequest Request, int calendarId), EditCalendarCommand>()
                .Map(dest => dest.title, src => src.Request.Title)
                .Map(dest => dest.description, src => src.Request.CalendarDescription)
                .Map(dest => dest.calendarId, src => src.calendarId)
                .Map(dest => dest.calendarColor, src => src.Request.CalendarColor);

            config.NewConfig<Calendar, CalendarResponse>()
                .Map(dest => dest.id, src => src.Id)
                .Map(dest => dest.AuthorId, src => src.AuthorId)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.CalendarDescription, src => src.CalendarDescription)
                .Map(dest => dest.CalendarColor, src => src.CalendarColor);
        }
    }
}
