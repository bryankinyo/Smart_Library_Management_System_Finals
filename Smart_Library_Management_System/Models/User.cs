namespace Smart_Library_Management_System.Models
{
    public abstract class User
    {
        private string _fullName;
        private readonly List<Loan> _loans = new();

        public Guid Id { get; private set; } = Guid.NewGuid();

        public string FullName
        {
            get => _fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("FullName is required.");
                _fullName = value.Trim();
            }
        }

        public string Email { get; set; }

        // Encapsulated collection
        public IReadOnlyCollection<Loan> Loans => _loans.AsReadOnly();

        protected User(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }

        // Polymorphic behavior: borrow allowances differ by derived type
        public abstract bool CanBorrow(out int remainingQuota);

        // Hook for adding loan
        public void AddLoan(Loan loan)
        {
            if (loan == null) throw new ArgumentNullException(nameof(loan));
            _loans.Add(loan);
        }
    }
}
