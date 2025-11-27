using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryContext _context;

        public LoanRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(Guid id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Loan> BorrowAsync(Guid userId, Guid bookId)
        {
            var loan = new Loan
            {
                UserId = userId,
                BookId = bookId,
                BorrowedAt = DateTime.UtcNow,
                DueAt = DateTime.UtcNow.AddDays(14),
                ReturnedAt = null
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<bool> ReturnAsync(Guid loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
                return false;

            loan.ReturnedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
