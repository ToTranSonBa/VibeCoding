namespace StudyApp.Api.Dtos.Auth
{
    public class RegisterRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
