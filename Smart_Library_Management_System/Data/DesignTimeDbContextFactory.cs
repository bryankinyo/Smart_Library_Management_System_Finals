using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Smart_Library_Management_System.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();

            var connectionString = "server=localhost;port=3306;database=library_db;user=root;password=bryankinnot;";

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new LibraryContext(optionsBuilder.Options);
        }
    }
}
