using Microsoft.EntityFrameworkCore;

namespace VacationCalendar.Api.EntityModels
{
    public class VacationCalendarContext : DbContext
    {
        public VacationCalendarContext(DbContextOptions<VacationCalendarContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<VacationData> VacationData { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }

    }

}
