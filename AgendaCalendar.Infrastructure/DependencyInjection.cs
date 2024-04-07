using AgendaCalendar.Application.Common.Interfaces;
using AgendaCalendar.Infrastructure.Authentication;
using AgendaCalendar.Infrastructure.Email;
using AgendaCalendar.Infrastructure.Persistence.Data;
using AgendaCalendar.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace AgendaCalendar.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration
            )
        {
            services.AddAuth(configuration)
                    .AddPersistence(configuration)
                    .AddBackgroundJob()
                    .AddEmail(configuration);
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine(configuration.GetConnectionString("AgendaCalendarDb"));
            services.AddPersistence()
                    .AddDbContext<AppDbContext>(
                        options =>
                            options.UseNpgsql(
                                configuration.GetConnectionString("AgendaCalendarDb")));
            return services;
        }

        public static IServiceCollection AddAuth(
            this IServiceCollection services,
            ConfigurationManager configuration) 
        {
            var jwtSettings = new JwtSettings();//for ASP .NET project
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            
            //services.AddOptions<JwtSettings>().Bind(configuration.GetSection(JwtSettings.SectionName));
            //services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>()
                    .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });
            return services;
        }

        public static IServiceCollection AddBackgroundJob(this IServiceCollection services)
        {
            //services.AddHangfire(conf => conf.UsePostgreSqlStorage("User ID=postgres;Password=rootroot;Host=localhost;Port=5432;Database=hangfire;")) for asp.net core
                    //.AddSingleton<HangfireBackgroundJobService>()
                    //.AddHangfireServer();
            return services;
        }

        public static IServiceCollection AddEmail(this IServiceCollection services, IConfigurationManager configuration)
        {
            //var emailSettings = new EmailSettings();
            //configuration.Bind(EmailSettings.SectionName, emailSettings);

            //services.AddSingleton(Options.Create(emailSettings));
            services.AddSingleton<IEmailSender, EmailSender>();
            return services;
        }
    }
}
