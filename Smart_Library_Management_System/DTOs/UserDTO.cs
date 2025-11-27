namespace Smart_Library_Management_System.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; } // "Student" or "Faculty"
    }
}
