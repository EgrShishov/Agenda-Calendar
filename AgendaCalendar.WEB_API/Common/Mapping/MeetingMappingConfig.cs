using AgendaCalendar.Application.Meetings.Commands;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Meetings;
using Mapster;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public class MeetingMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(CreateMeetingRequest request, int userId), AddMeetingCommand>()
                .Map(dest => dest.description, src => src.request.description)
                .Map(dest => dest.title, src => src.request.title)
                .Map(dest => dest.startTime, src => src.request.startTime)
                .Map(dest => dest.endTime, src => src.request.endTime)
                .Map(dest => dest.userId, src => src.userId);

            config.NewConfig<(EditMeetingRequest request, int userId), UpdateMeetingCommand>()
                .Map(dest => dest.description, src => src.request.description)
                .Map(dest => dest.title, src => src.request.title)
                .Map(dest => dest.startTime, src => src.request.startTime)
                .Map(dest => dest.endTime, src => src.request.endTime);

            config.NewConfig<Meeting, MeetingResponse>();
        }
    }
}
