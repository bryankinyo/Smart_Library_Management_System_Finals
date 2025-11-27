using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Interfaces
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<Loan?> GetByIdAsync(Guid id);
        Task AddAsync(Loan loan);
        Task UpdateAsync(Loan loan);
        Task DeleteAsync(Guid id);

        Task<Loan> BorrowAsync(Guid userId, Guid bookId);
        Task<bool> ReturnAsync(Guid loanId);
    }
}
