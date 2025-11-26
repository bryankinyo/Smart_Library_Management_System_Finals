using System;

namespace Smart_Library_Management_System.DTOs
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ReservedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}