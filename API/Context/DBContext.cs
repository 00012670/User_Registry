using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> o)
            : base(o)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
