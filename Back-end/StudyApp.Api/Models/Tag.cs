using System;
using System.Collections.Generic;

namespace StudyApp.Api.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? NormalizedName { get; set; }

        public ICollection<DeckTag> DeckTags { get; set; } = new List<DeckTag>();

        public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }
}
