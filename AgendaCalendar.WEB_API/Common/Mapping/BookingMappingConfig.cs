using AgendaCalendar.Application.Slots.Common;
using AgendaCalendar.WEB_API.Contracts.Slots;
using Mapster;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public class BookingMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ScheduleResult, ScheduleResponse>()
                .Map(dest => dest.description, src => src.description)
                .Map(dest => dest.title, src => src.title)
                .Map(dest => dest.userName, src => src.userName)
                .Map(dest => dest.slots, src => src.slots
                .Select(slt => new SlotResponse(slt.Id, slt.IsBooked, new List<TimeOnly> { TimeOnly.Parse(slt.Times[0]) }, slt.Date))
                );
        }
    }
}
