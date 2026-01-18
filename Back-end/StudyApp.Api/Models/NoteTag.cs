using System;

namespace StudyApp.Api.Models
{
    public class NoteTag
    {
        public Guid NoteId { get; set; }

        public int TagId { get; set; }

        public KnowledgeNote? Note { get; set; }

        public Tag? Tag { get; set; }
    }
}
