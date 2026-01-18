using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyApp.Api.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string? DisplayName { get; set; }

        public string? Bio { get; set; }

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
