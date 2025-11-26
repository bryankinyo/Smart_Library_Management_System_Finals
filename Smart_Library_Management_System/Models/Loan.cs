namespace Smart_Library_Management_System.Models
{
    public class Loan
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public Guid UserId { get; set; }
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public bool IsReturned => ReturnedAt.HasValue;
        public bool IsOverdue => !IsReturned && DateTime.UtcNow > DueAt;

        public int DaysOverdue => IsOverdue ? (int)(DateTime.UtcNow - DueAt).TotalDays : 0;
    }
}
