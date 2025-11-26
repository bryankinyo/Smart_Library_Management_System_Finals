using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.Models;
using Smart_Library_Management_System.Interfaces;

namespace Smart_Library_Management_System.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly LibraryContext _db;
        public CatalogRepository(LibraryContext db) { _db = db; }

        public async Task AddAsync(Catalog catalog, IEnumerable<Guid> initialBookIds = null)
        {
            if (initialBookIds != null)
            {
                var books = await _db.Books.Where(b => initialBookIds.Contains(b.Id)).ToListAsync();
                foreach (var book in books) catalog.Books.Add(book);
            }

            _db.Catalogs.Add(catalog);
            await _db.SaveChangesAsync();
        }

        public async Task<Catalog> GetByIdAsync(Guid id, bool includeBooks = false)
        {
            if (includeBooks)
            {
                return await _db.Catalogs
                    .Include(c => c.Books)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            return await _db.Catalogs.FindAsync(id);
        }

        public async Task<IEnumerable<Catalog>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return await _db.Catalogs.ToListAsync();

            return await _db.Catalogs
                .Where(c => EF.Functions.Like(c.Name, $"%{query}%") ||
                            EF.Functions.Like(c.Description, $"%{query}%"))
                .ToListAsync();
        }

        public async Task UpdateAsync(Catalog catalog)
        {
            _db.Catalogs.Update(catalog);
            await _db.SaveChangesAsync();
        }

        public async Task AddBookAsync(Guid catalogId, Guid bookId)
        {
            var catalog = await _db.Catalogs.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == catalogId);
            if (catalog == null) throw new InvalidOperationException("Catalog not found.");

            var book = await _db.Books.FindAsync(bookId);
            if (book == null) throw new InvalidOperationException("Book not found.");

            if (!catalog.Books.Any(b => b.Id == book.Id))
            {
                catalog.Books.Add(book);
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveBookAsync(Guid catalogId, Guid bookId)
        {
            var catalog = await _db.Catalogs.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == catalogId);
            if (catalog == null) throw new InvalidOperationException("Catalog not found.");

            var book = catalog.Books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                catalog.Books.Remove(book);
                await _db.SaveChangesAsync();
            }
        }
    }
}