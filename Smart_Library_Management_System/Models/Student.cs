namespace Smart_Library_Management_System.Models
{
    public class Student : User
    {
        public Student(string fullName, string email) : base(fullName, email) { }

        public override bool CanBorrow(out int remainingQuota)
        {
            const int limit = 3;
            remainingQuota = Math.Max(0, limit - Loans.Count);
            return remainingQuota > 0;
        }
    }
}
