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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Hangfire;
using Hangfire.PostgreSql;
using AgendaCalendar.Infrastructure.Authorization;

namespace AgendaCalendar.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration
            )
        {
            services.AddPersistence(configuration)
                    .AddIdentity()
                    .AddAuth(configuration)
                    .AddBackgroundJob(configuration)
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
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            var googleOAuthSettings = new GoogleOAuthSettings();
            configuration.Bind(GoogleOAuthSettings.SectionName, googleOAuthSettings);
            
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton(Options.Create(googleOAuthSettings));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>()
                    .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                        opt.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.Secret))
                        });

            services.AddAuthentication().AddGoogle(
                googleOptions =>
                {
                    googleOptions.ClientId = googleOAuthSettings.ClientId;
                    googleOptions.ClientSecret = googleOAuthSettings.Secret;
                });

            return services;
        }

        public static IServiceCollection AddBackgroundJob(this IServiceCollection services, IConfiguration configuration)
        {
           /* services.AddHangfire(conf => conf.UsePostgreSqlStorage(configuration.GetConnectionString("HangfireDb")))
                    .AddSingleton<HangfireBackgroundJobService>()
                    .AddHangfireServer();*/
            return services;
        }

        public static IServiceCollection AddEmail(this IServiceCollection services, IConfigurationManager configuration)
        {
            var emailSettings = new EmailSettings();
            configuration.Bind(EmailSettings.SectionName, emailSettings);

            services.AddSingleton(Options.Create(emailSettings));
            services.AddSingleton<IEmailSender, EmailSender>();
            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddUserManager<UserManager<User>>()
                    .AddDefaultTokenProviders();

            return services;
        }
    }
}
