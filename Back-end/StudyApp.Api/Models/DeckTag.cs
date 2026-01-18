using System;

namespace StudyApp.Api.Models
{
    public class DeckTag
    {
        public Guid DeckId { get; set; }

        public int TagId { get; set; }

        public FlashcardDeck? Deck { get; set; }

        public Tag? Tag { get; set; }
    }
}
