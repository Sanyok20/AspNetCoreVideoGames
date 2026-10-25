namespace VideoGames.BLL.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}