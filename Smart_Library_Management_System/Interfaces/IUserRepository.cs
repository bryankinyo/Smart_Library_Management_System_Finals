using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync(string? userType = null);
        Task<IEnumerable<User>> SearchAsync(string query);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}