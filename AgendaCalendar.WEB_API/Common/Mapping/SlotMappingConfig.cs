using AgendaCalendar.Application.Slots.Commands;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Slots;
using Mapster;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public class SlotMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(BookSlotRequest request, int slotId), BookSlotCommand>()
                .Map(dest => dest.firstName, src => src.request.firstName)
                .Map(dest => dest.lastName, src => src.request.lastName)
                .Map(dest => dest.description, src => src.request.description)
                .Map(dest => dest.email, src => src.request.email)
                .Map(dest => dest.slotId, src => src.slotId);


            config.NewConfig<Slot, SlotResponse>()
                .Map(dest => dest.Times, src => src.Times)
                .Map(dest => dest.Date, src => src.Date)
                .Map(dest => dest.IsBooked, src => src.IsBooked);
        }
    }
}
