using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        //Used to create the Categories table in db using EF core
        //in the PMC type add-migration {any valide name} in this case we have given AddCategoryTableToDb
        // This then creates a new Folder called Migration in the Solutioin Explore and new file with the given name
        // This files have some code that will be converted into to SQL satements by the EF and again once we give update-database command
        // This then checks for any new migration and that excecutes it.
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id=1, Name="SciFi",DisplayOrder=1},
                new Category { Id = 2, Name = "Kids", DisplayOrder = 3 },
                new Category { Id = 3, Name = "Novel", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Crime", DisplayOrder = 4 }
                );
        }
    }
}
