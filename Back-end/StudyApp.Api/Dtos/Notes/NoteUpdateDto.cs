namespace StudyApp.Api.Dtos.Notes
{
    public class NoteUpdateDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool? IsPublic { get; set; }
    }
}
