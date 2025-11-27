using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _db;
        public BookRepository(LibraryContext db) { _db = db; }

        public async Task AddAsync(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _db.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> SearchAsync(string titleOrAuthor)
        {
            if (string.IsNullOrWhiteSpace(titleOrAuthor)) return await _db.Books.ToListAsync();

            return await _db.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{titleOrAuthor}%") ||
                            EF.Functions.Like(b.Author, $"%{titleOrAuthor}%"))
                .ToListAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
        }
    }
}