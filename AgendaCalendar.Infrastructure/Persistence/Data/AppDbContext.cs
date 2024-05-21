using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgendaCalendar.Infrastructure.Persistence.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {   
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .OwnsOne<RecurrenceRule>(e => e.ReccurenceRules, e =>
                {
                    e.WithOwner();
                });
  
            modelBuilder.Entity<Event>()
                .OwnsOne<List<EventParticipant>>(e => e.EventParticipants);

            modelBuilder.Entity<IdentityUserLogin<int>>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<IdentityUserRole<int>>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<IdentityUserToken<int>>()
                .HasKey(u => u.UserId);
            modelBuilder.ApplyUtcDateTimeConverter();

            modelBuilder.Entity<DailyWorkingHours>()
                .HasKey(dh => new { dh.WorkingHoursId, dh.Day });

            modelBuilder.Entity<DailyWorkingHours>()
               .HasOne(dh => dh.WorkingHours)
               .WithMany(wh => wh.DailyHours)
               .HasForeignKey(dh => dh.WorkingHoursId);

            /* modelBuilder.Entity<Invitation>(e =>
             {
                 e.HasKey(e => e.Id);

                 e.Property(e => e.Status)
                 .HasConversion<string>()
                 .HasDefaultValue(InvitationStatus.Pending);
             });*/
        }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Reminder>? Reminders { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<WorkingHours> WorkingHours { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
    }
}