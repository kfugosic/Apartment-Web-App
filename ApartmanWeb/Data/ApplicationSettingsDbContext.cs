using System.Data.Entity;
using ApartmanWeb.Models;

namespace ApartmanWeb.Data
{
    public class ApplicationSettingsDbContext : DbContext
    {

        public ApplicationSettingsDbContext(string cnnstr) : base(cnnstr)
        {
        }

        public IDbSet<ApplicationSettings> AppSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationSettings>().HasKey(t => t.Id);
            modelBuilder.Entity<ApplicationSettings>().Property(t => t.DirectReservation).IsRequired();
            modelBuilder.Entity<ApplicationSettings>().Property(t => t.Order).IsRequired();

        }
    }
}
