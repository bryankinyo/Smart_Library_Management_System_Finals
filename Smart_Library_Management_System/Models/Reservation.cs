namespace Smart_Library_Management_System.Models
{
    public enum ReservationStatus { Active, Fulfilled, Cancelled }

    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid UserId { get; set; }
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
        public ReservationStatus Status { get; set; } = ReservationStatus.Active;
    }
}
