using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Repositories
{
    public class FineRepository : IFineRepository
    {
        private readonly LibraryContext _context;

        public FineRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fine>> GetAllAsync()
        {
            return await _context.Fines.ToListAsync();
        }

        public async Task<Fine?> GetByIdAsync(Guid id)
        {
            return await _context.Fines.FindAsync(id);
        }

        public async Task AddAsync(Fine fine)
        {
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Fine fine)
        {
            _context.Fines.Update(fine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var fine = await _context.Fines.FindAsync(id);
            if (fine != null)
            {
                _context.Fines.Remove(fine);
                await _context.SaveChangesAsync();
            }
        }
    }
}
