using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Interfaces
{
    public interface ICatalogRepository
    {
        Task<Catalog> GetByIdAsync(Guid id, bool includeBooks = false);
        Task<IEnumerable<Catalog>> SearchAsync(string query);
        Task AddAsync(Catalog catalog, IEnumerable<Guid> initialBookIds = null);
        Task UpdateAsync(Catalog catalog);
        Task AddBookAsync(Guid catalogId, Guid bookId);
        Task RemoveBookAsync(Guid catalogId, Guid bookId);
    }
}