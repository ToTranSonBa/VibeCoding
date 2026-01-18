using System;

namespace StudyApp.Api.Models
{
    // Optional: store reference-only saves if you prefer not to clone
    public class SavedDeck
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid DeckId { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser? User { get; set; }

        public FlashcardDeck? Deck { get; set; }
    }
}
