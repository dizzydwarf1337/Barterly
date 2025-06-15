namespace Application.DTOs.User
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? token { get; set; }
        public string? Role { get; set; }
    }
}
