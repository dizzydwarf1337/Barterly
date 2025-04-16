namespace Application.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicturePath { get; set; }
        public string? token { get; set; }
        public string Role { get; set; }
    }
}
