using PersonalCalendar.Domain;
using System.Data.Entity;

namespace PersonalCalendar.Data
{
    public class CalendarDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
    }
}
