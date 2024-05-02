using BulkyWebRazor_temp.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWebRazor_temp.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // Create a Category table with in Database using EF
        // Category Model is used to create the table usinf DbSet EFCore class
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "SciFi", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Kids", DisplayOrder = 3 },
                new Category { Id = 3, Name = "Novel", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Crime", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Check", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Time", DisplayOrder = 7 }

                );
        }
    }
}
