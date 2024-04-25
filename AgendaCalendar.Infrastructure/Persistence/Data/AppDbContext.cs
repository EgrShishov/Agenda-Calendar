﻿using Microsoft.AspNetCore.Identity;
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
                    e.OwnsOne<List<TimePeriod>>(e => e.RecurrenceDates);
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

        }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Reminder>? Reminders { get; set; }
        public DbSet<User>? Users { get; set; }
    }
}