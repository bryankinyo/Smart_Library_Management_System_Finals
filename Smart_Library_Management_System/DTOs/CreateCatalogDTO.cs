namespace Smart_Library_Management_System.DTOs
{
    public class CreateCatalogDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // optional initial books
        public System.Collections.Generic.List<System.Guid> BookIds { get; set; } = new();
    }
}