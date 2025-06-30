using DataManagementTranslation.Models;
using Microsoft.EntityFrameworkCore;
using DataManagementTranslation.Configurations;
namespace DataManagementTranslation
{
    public class DataManagerDbContext : DbContext
    {

        public DataManagerDbContext(DbContextOptions<DataManagerDbContext> options):base(options) { }

        public DbSet<Clients> Client { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // вкючаем настройки модели данных 
        {
            modelBuilder.ApplyConfiguration(new СlientsConfiguration());


            base.OnModelCreating(modelBuilder);
        }

    }
}
