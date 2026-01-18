using System;

namespace StudyApp.Api.Models
{
    public class Flashcard
    {
        public Guid Id { get; set; }

        public Guid DeckId { get; set; }

        public string Front { get; set; } = string.Empty;

        public string Back { get; set; } = string.Empty;

        public int OrderIndex { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public FlashcardDeck? Deck { get; set; }
    }
}
