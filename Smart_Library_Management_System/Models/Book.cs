namespace Smart_Library_Management_System.Models
{
    public class Book
    {
        private string _title;
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Title is required.");
                _title = value.Trim();
            }
        }

        public string Author { get; set; }
        public string Isbn { get; set; }
        public int CopiesAvailable { get; set; } = 1;
    }
}
