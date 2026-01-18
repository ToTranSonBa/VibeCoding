namespace StudyApp.Api.Dtos.Auth
{
    public class LoginRequest
    {
        public string? UserNameOrEmail { get; set; }
        public string? Password { get; set; }
    }
}
