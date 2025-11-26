using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Smart_Library_Management_System.DTOs;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // List all books or search by title/author (q)
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? q)
        {
            var books = await _repo.SearchAsync(q);
            var dtos = _mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(dtos);
        }

        // Search endpoint kept for compatibility
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? q) => await List(q);

        // Get a single book
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book == null) return NotFound();
            var dto = _mapper.Map<BookDto>(book);
            return Ok(dto);
        }

        // Create a new book
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDto create)
        {
            if (create == null) return BadRequest("Request body is required.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = _mapper.Map<Book>(create);
            await _repo.AddAsync(book);

            var dto = _mapper.Map<BookDto>(book);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // Update an existing book (replace updatable fields from CreateBookDto)
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateBookDto update)
        {
            if (update == null) return BadRequest("Request body is required.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Map incoming fields onto existing entity
            _mapper.Map(update, existing);

            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        // Increment copies available (small convenience action)
        [HttpPost("{id:guid}/copies/increment")]
        public async Task<IActionResult> IncrementCopies(Guid id, [FromQuery] int amount = 1)
        {
            if (amount <= 0) return BadRequest("Amount must be a positive integer.");

            var book = await _repo.GetByIdAsync(id);
            if (book == null) return NotFound();

            book.CopiesAvailable += amount;
            await _repo.UpdateAsync(book);
            return NoContent();
        }
    }
}