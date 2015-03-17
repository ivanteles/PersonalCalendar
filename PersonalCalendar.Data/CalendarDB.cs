using PersonalCalendar.Domain;
using System.Data.Entity;

namespace PersonalCalendar.Data
{
    public class CalendarDB : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Map(map =>
                {
                    map.Properties(p => new { p.Id, p.CalendarId, p.Title, p.Location, p.Description, p.Color });
                    map.ToTable("Events");
                })

                .Map(map =>
                {
                    map.Properties(p => new { p.StartDateTimeUTC, p.EndDateTimeUTC, p.SeriesEndDateUTC, p.FreqType, p.FreqSubtype, p.FreqInterval });
                    map.ToTable("Schedules");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
