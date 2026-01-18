namespace StudyApp.Api.Dtos.Notes
{
    public class NoteCreateDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool IsPublic { get; set; }
    }
}
