namespace VideoGames.DAL.Entities
{
    public class User : BaseEntity
    {
        public required string UserName { get; set; }
        public required string NormalizedUserName { get; set; }
        public required string Email { get; set; }
        public required string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public bool IsSubscribe { get; set; } = false;
        public DateTime? DateOfBirth { get; set; }

        public List<Role> Roles { get; set; } = [];
        public List<UserToken> Tokens { get; set; } = [];
    }
}