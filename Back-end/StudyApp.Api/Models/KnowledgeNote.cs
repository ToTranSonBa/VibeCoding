using System;
using System.Collections.Generic;

namespace StudyApp.Api.Models
{
    public class KnowledgeNote
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty; // Markdown

        public bool IsPublic { get; set; } = false;

        public string? OriginalFileName { get; set; }

        public long? FileSize { get; set; }

        public string? MimeType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser? Owner { get; set; }

        public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }
}
