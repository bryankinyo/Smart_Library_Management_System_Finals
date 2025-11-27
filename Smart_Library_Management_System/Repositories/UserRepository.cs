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
    public class UserRepository : IUserRepository
    {
        private readonly LibraryContext _db;

        public UserRepository(LibraryContext db)
        {
            _db = db;
        }

        public async Task<User> AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync(string? userType = null)
        {
            var q = _db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(userType))
            {
                q = q.Where(u => EF.Property<string>(u, "UserType") == userType);
            }
            return await q.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return await _db.Users.ToListAsync();

            return await _db.Users
                .Where(u => EF.Functions.Like(u.FullName, $"%{query}%") ||
                            EF.Functions.Like(u.Email, $"%{query}%"))
                .ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}