using Smart_Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Models;
using System.Collections.Generic;

namespace Smart_Library_Management_System.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // TPH mapping for Student/Faculty
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TPH for Users
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Student>("Student")
                .HasValue<Faculty>("Faculty");

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<Loan>().HasKey(l => l.Id);
            modelBuilder.Entity<Fine>().HasKey(f => f.Id);
            modelBuilder.Entity<Reservation>().HasKey(r => r.Id);
            modelBuilder.Entity<Catalog>().HasKey(c => c.Id);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany()
                .HasForeignKey(l => l.BookId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId);

            // Many-to-many Catalog <-> Book (join table CatalogBooks)
            modelBuilder.Entity<Catalog>()
                .HasMany(c => c.Books)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "CatalogBook",
                    j => j.HasOne<Book>().WithMany().HasForeignKey("BookId").HasConstraintName("FK_CatalogBook_Book_BookId"),
                    j => j.HasOne<Catalog>().WithMany().HasForeignKey("CatalogId").HasConstraintName("FK_CatalogBook_Catalog_CatalogId"),
                    j =>
                    {
                        j.HasKey("CatalogId", "BookId");
                        j.ToTable("CatalogBooks");
                    });

            base.OnModelCreating(modelBuilder);
        }
    }
}