using System.Collections.Generic;
using Smart_Library_Management_System.DTOs;

namespace Smart_Library_Management_System.DTOs
{
    public class CatalogDetailsDto
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BookDto> Books { get; set; } = new();
    }
}