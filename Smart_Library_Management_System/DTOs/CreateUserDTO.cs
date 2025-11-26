namespace Smart_Library_Management_System.DTOs
{
    public class CreateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // "Student" or "Faculty" (case-insensitive). Default to Student if not provided.
        public string? UserType { get; set; }
    }
}