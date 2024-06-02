using AgendaCalendar.Application.Meetings.Commands;
using AgendaCalendar.Application.WorkingHours.Commands;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Meetings;
using AgendaCalendar.WEB_API.Contracts.WorkingHours;
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

            config.NewConfig<(SetWorkingHoursRequest request, int userId), SetWorkingHoursCommand>()
                .Map(dest => dest.dailyHours, src => src.request.DailyHours
                    .Select(dh => new DailyWorkingHours
                    {
                        Day = dh.Day,
                        StartTime = TimeOnly.FromDateTime(dh.StartTime),
                        EndTime = TimeOnly.FromDateTime(dh.EndTime)
                    }).ToList())
                .Map(dest => dest.day, src => src.request.Day)
                .Map(dest => dest.userId, src => src.userId);

            config.NewConfig<Meeting, MeetingResponse>()
                .Map(dest => dest.id, src => src.Id);
        }
    }
}
