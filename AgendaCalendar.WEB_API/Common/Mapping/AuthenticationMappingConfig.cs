using AgendaCalendar.Application;
using AgendaCalendar.Application.Authentication.Common;
using AgendaCalendar.WEB_API.Contracts.Authentication;
using Mapster;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, SignUpCommand>();

            config.NewConfig<LoginRequest, LoginQuery>();

            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(dest => dest.Token, src => src.Token)
                .Map(dest => dest, src => src.user);
        }
    }
}
