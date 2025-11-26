using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Interfaces
{
    public interface ILoanService
    {
        Task<Loan> BorrowAsync(Guid userId, Guid bookId);
        Task ReturnAsync(Guid loanId);
        Task<decimal> CalculateFineAsync(Guid loanId);
    }
}
