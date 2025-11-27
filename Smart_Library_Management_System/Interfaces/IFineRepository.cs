using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Interfaces
{
    public interface IFineRepository
    {
        Task<IEnumerable<Fine>> GetAllAsync();
        Task<Fine?> GetByIdAsync(Guid id);
        Task AddAsync(Fine fine);
        Task UpdateAsync(Fine fine);
        Task DeleteAsync(Guid id);
    }
}
