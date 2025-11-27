using System;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.DTOs;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // Create user (Student or Faculty)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName)) return BadRequest("FullName is required.");
            if (string.IsNullOrWhiteSpace(dto.Email)) return BadRequest("Email is required.");

            var userType = (dto.UserType ?? "Student").Trim().ToLowerInvariant();

            User user = userType switch
            {
                "faculty" => new Faculty(dto.FullName, dto.Email),
                _ => new Student(dto.FullName, dto.Email)
            };

            await _repo.AddAsync(user);
            var result = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // Get user by id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            var dto = _mapper.Map<UserDto>(user);
            return Ok(dto);
        }

        // List users optionally filter by userType ("Student" or "Faculty")
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? userType, [FromQuery] string? q)
        {
            if (!string.IsNullOrWhiteSpace(q))
            {
                var search = await _repo.SearchAsync(q);
                var dtos = search.Select(u => _mapper.Map<UserDto>(u));
                return Ok(dtos);
            }

            var users = await _repo.GetAllAsync(userType);
            var results = users.Select(u => _mapper.Map<UserDto>(u));
            return Ok(results);
        }

        // Update user
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.FullName)) user.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Email)) user.Email = dto.Email;

            await _repo.UpdateAsync(user);
            return NoContent();
        }

        // Delete user
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}