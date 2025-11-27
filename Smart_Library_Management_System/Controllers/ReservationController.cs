using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.DTOs;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly LibraryContext _db;

        public ReservationsController(LibraryContext db)
        {
            _db = db;
        }

        // Create a reservation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            // basic validation
            var book = await _db.Books.FindAsync(dto.BookId);
            if (book == null) return NotFound("Book not found.");

            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null) return NotFound("User not found.");

            var reservation = new Reservation
            {
                BookId = dto.BookId,
                UserId = dto.UserId,
                ReservedAt = DateTime.UtcNow,
                Status = ReservationStatus.Active
            };

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();

            var result = new ReservationDto
            {
                Id = reservation.Id,
                BookId = reservation.BookId,
                UserId = reservation.UserId,
                ReservedAt = reservation.ReservedAt,
                Status = reservation.Status.ToString()
            };

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // Get reservation
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var res = await _db.Reservations
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (res == null) return NotFound();

            var dto = new ReservationDto
            {
                Id = res.Id,
                BookId = res.BookId,
                UserId = res.UserId,
                ReservedAt = res.ReservedAt,
                Status = res.Status.ToString()
            };

            return Ok(dto);
        }

        // List reservations (optionally by user)
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] Guid? userId)
        {
            var q = _db.Reservations.Include(r => r.Book).AsQueryable();
            if (userId.HasValue) q = q.Where(r => r.UserId == userId.Value);
            var items = await q.ToListAsync();

            var dtos = items.ConvertAll(r => new ReservationDto
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                ReservedAt = r.ReservedAt,
                Status = r.Status.ToString()
            });

            return Ok(dtos);
        }

        // Cancel reservation
        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var res = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            if (res == null) return NotFound();

            if (res.Status != ReservationStatus.Active) return BadRequest("Reservation cannot be cancelled.");

            res.Status = ReservationStatus.Cancelled;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // Fulfill reservation (mark fulfilled when book becomes available)
        [HttpPost("{id:guid}/fulfill")]
        public async Task<IActionResult> Fulfill(Guid id)
        {
            var res = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            if (res == null) return NotFound();

            if (res.Status != ReservationStatus.Active) return BadRequest("Reservation cannot be fulfilled.");

            res.Status = ReservationStatus.Fulfilled;
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}