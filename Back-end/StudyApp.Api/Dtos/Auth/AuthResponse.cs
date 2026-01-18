namespace StudyApp.Api.Dtos.Auth
{
    public class AuthResponse
    {
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
        public UserDto? User { get; set; }
    }
}
