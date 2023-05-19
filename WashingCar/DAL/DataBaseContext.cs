using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WashingCar.DAL.Entities;

namespace WashingCar.DAL
{
    public class DataBaseContext : IdentityDbContext<User>
    {
        #region Builder
        public DataBaseContext(DbContextOptions<DataBaseContext> option) : base(option)
        {
        }
        #endregion

        #region Properties
        public DbSet<Service> Services { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Service>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Vehicle>().HasIndex(v => v.NumberPlate).IsUnique();
        }
    }
}
