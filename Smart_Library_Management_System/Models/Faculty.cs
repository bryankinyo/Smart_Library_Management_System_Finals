namespace Smart_Library_Management_System.Models
{
    public class Faculty : User
    {
        public Faculty(string fullName, string email) : base(fullName, email) { }

        public override bool CanBorrow(out int remainingQuota)
        {
            const int limit = 10;
            remainingQuota = Math.Max(0, limit - Loans.Count);
            return remainingQuota > 0;
        }
    }
}
