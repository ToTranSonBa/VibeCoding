using Microsoft.AspNetCore.Identity;
using System;

namespace StudyApp.Api.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Navigation property to profile
        public UserProfile? Profile { get; set; }
    }
}
