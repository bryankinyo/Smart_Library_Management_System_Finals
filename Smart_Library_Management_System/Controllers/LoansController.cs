using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.DTOs;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly LibraryContext _db;
        private readonly IMapper _mapper;

        public LoansController(ILoanService loanService, LibraryContext db, IMapper mapper)
        {
            _loanService = loanService;
            _db = db;
            _mapper = mapper;
        }

        // Borrow a book
        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow([FromBody] BorrowRequestDto request)
        {
            var loan = await _loanService.BorrowAsync(request.UserId, request.BookId);
            var dto = _mapper.Map<LoanDto>(loan);
            return Ok(dto);
        }

        // Return a loan
        [HttpPost("{loanId:guid}/return")]
        public async Task<IActionResult> Return(Guid loanId)
        {
            await _loanService.ReturnAsync(loanId);
            return NoContent();
        }

        // Get loan by id
        [HttpGet("{loanId:guid}")]
        public async Task<IActionResult> Get(Guid loanId)
        {
            var loan = await _db.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null) return NotFound();

            var dto = _mapper.Map<LoanDto>(loan);
            return Ok(dto);
        }

        // List loans (optional query by userId)
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] Guid? userId)
        {
            var query = _db.Loans.Include(l => l.Book).AsQueryable();
            if (userId.HasValue) query = query.Where(l => l.UserId == userId.Value);
            var loans = await query.ToListAsync();
            var dtos = _mapper.Map<System.Collections.Generic.IEnumerable<LoanDto>>(loans);
            return Ok(dtos);
        }
    }
}