namespace Smart_Library_Management_System.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public int CopiesAvailable { get; set; } = 1;
    }
}
