using System;
using System.Collections.Generic;

namespace StudyApp.Api.Models
{
    public class FlashcardDeck
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsPublic { get; set; } = false;

        public Guid OwnerId { get; set; }

        public Guid? SourceDeckId { get; set; }

        public int StudyCount { get; set; }

        public int LikeCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser? Owner { get; set; }

        public FlashcardDeck? SourceDeck { get; set; }

        public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

        public ICollection<DeckTag> DeckTags { get; set; } = new List<DeckTag>();
    }
}
