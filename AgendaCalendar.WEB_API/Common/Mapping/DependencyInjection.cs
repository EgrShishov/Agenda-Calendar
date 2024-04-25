using Mapster;
using MapsterMapper;
using System.Reflection;

namespace AgendaCalendar.WEB_API.Common.Mapping
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config)
                .AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
