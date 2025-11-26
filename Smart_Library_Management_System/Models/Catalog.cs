namespace Smart_Library_Management_System.Models
{
    public class Catalog
    {
        private string _name;

        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Catalog name is required.");
                _name = value.Trim();
            }
        }

        public string Description { get; set; }

        // Many-to-many: a catalog contains many books; a book can be in multiple catalogs
        public ICollection<Book> Books { get; private set; } = new List<Book>();
    }
}
