using AgendaCalendar.WEB_API.Common.Errors;
using AgendaCalendar.WEB_API.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AgendaCalendar.WEB_API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ProblemDetailsFactory, AgendaCalendarProblemDetailsFactory>();
            services.AddMappings();
            return services;
        }
    }
}
