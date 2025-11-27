using System;
using System.Threading.Tasks;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Services
{
    // Strategy for fine: simple per-day rate; could be injected as IFineCalculator
    public class LoanService : ILoanService
    {
        private readonly LibraryContext _db;
        private const decimal DailyRate = 0.50m; // example

        public LoanService(LibraryContext db) { _db = db; }

        public async Task<Loan> BorrowAsync(Guid userId, Guid bookId)
        {
            var user = await _db.Users.Include(u => u.Loans).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new InvalidOperationException("User not found.");

            if (!user.CanBorrow(out var remaining)) throw new InvalidOperationException("Borrow limit reached.");

            var book = await _db.Books.FindAsync(bookId);
            if (book == null || book.CopiesAvailable <= 0) throw new InvalidOperationException("Book not available.");

            // Create loan with due date depending on user type (e.g., students 14 days, faculty 30)
            var loan = new Loan
            {
                BookId = book.Id,
                Book = book,
                UserId = user.Id,
                BorrowedAt = DateTime.UtcNow,
                DueAt = DateTime.UtcNow.AddDays(user is Student ? 14 : 30)
            };

            book.CopiesAvailable -= 1;
            user.AddLoan(loan);

            _db.Loans.Add(loan);
            await _db.SaveChangesAsync();

            return loan;
        }

        public async Task ReturnAsync(Guid loanId)
        {
            var loan = await _db.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null) throw new InvalidOperationException("Loan not found.");
            if (loan.IsReturned) throw new InvalidOperationException("Already returned.");

            loan.ReturnedAt = DateTime.UtcNow;
            loan.Book.CopiesAvailable += 1;

            // Optionally create fine if overdue
            var fineAmount = await CalculateFineAsync(loanId);
            if (fineAmount > 0)
            {
                var fine = new Fine { LoanId = loan.Id, Loan = loan, Amount = fineAmount, Paid = false };
                _db.Fines.Add(fine);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<decimal> CalculateFineAsync(Guid loanId)
        {
            var loan = await _db.Loans.FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null) throw new InvalidOperationException("Loan not found.");
            var overdueDays = loan.IsOverdue ? loan.DaysOverdue : 0;
            return overdueDays * DailyRate;
        }
    }
}