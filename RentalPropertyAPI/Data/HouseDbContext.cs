using Microsoft.EntityFrameworkCore;

namespace RentalPropertyAPI.Data
{
    public class HouseDbContext: DbContext
    {
        public DbSet<HouseEntity> Houses => Set<HouseEntity>();

        public HouseDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            optionsBuilder.UseSqlite($"Data Source={Path.Join(path, "houses.db")}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData.Seed(modelBuilder);
        }
    }
}
