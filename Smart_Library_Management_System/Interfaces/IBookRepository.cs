using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(Guid id);
        Task<IEnumerable<Book>> SearchAsync(string titleOrAuthor);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
    }
}
