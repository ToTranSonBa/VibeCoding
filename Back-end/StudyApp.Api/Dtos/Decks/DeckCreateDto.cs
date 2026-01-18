using System.Collections.Generic;

namespace StudyApp.Api.Dtos.Decks
{
    public class CardCreateDto
    {
        public string? Front { get; set; }
        public string? Back { get; set; }
        public int OrderIndex { get; set; }
    }

    public class DeckCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public List<string>? Tags { get; set; }
        public List<CardCreateDto>? Cards { get; set; }
    }
}
