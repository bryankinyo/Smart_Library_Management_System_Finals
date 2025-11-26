using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Smart_Library_Management_System.DTOs;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Models;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogRepository _repo;
        private readonly IMapper _mapper;

        public CatalogsController(ICatalogRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var catalog = await _repo.GetByIdAsync(id, includeBooks: true);
            if (catalog == null) return NotFound();
            var dto = _mapper.Map<CatalogDetailsDto>(catalog);
            return Ok(dto);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var results = await _repo.SearchAsync(q);
            var dto = _mapper.Map<System.Collections.Generic.IEnumerable<CatalogDto>>(results);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatalogDto create)
        {
            if (string.IsNullOrWhiteSpace(create.Name)) return BadRequest("Name is required.");

            var catalog = new Catalog
            {
                Name = create.Name,
                Description = create.Description
            };

            await _repo.AddAsync(catalog, create.BookIds);
            var dto = _mapper.Map<CatalogDto>(catalog);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPost("{id:guid}/books")]
        public async Task<IActionResult> AddBook(Guid id, [FromBody] AddBookDto body)
        {
            await _repo.AddBookAsync(id, body.BookId);
            return NoContent();
        }

        [HttpDelete("{id:guid}/books/{bookId:guid}")]
        public async Task<IActionResult> RemoveBook(Guid id, Guid bookId)
        {
            await _repo.RemoveBookAsync(id, bookId);
            return NoContent();
        }
    }
}