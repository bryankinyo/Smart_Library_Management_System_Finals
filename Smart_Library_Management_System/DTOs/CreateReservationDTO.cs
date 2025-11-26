using System;

namespace Smart_Library_Management_System.DTOs
{
    public class CreateReservationDto
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
    }
}