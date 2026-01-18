using System;

namespace StudyApp.Api.Dtos.Decks
{
    public class DeckSearchDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
