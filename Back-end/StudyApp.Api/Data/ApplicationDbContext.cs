using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyApp.Api.Models;

namespace StudyApp.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; } = null!;

        public DbSet<FlashcardDeck> FlashcardDecks { get; set; } = null!;

        public DbSet<Flashcard> Flashcards { get; set; } = null!;

        public DbSet<Tag> Tags { get; set; } = null!;

        public DbSet<DeckTag> DeckTags { get; set; } = null!;

        public DbSet<KnowledgeNote> KnowledgeNotes { get; set; } = null!;

        public DbSet<NoteTag> NoteTags { get; set; } = null!;

        public DbSet<SavedDeck> SavedDecks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // UserProfile one-to-one
            builder.Entity<UserProfile>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            builder.Entity<UserProfile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // FlashcardDeck -> Flashcards (1..*)
            builder.Entity<Flashcard>()
                .HasOne(f => f.Deck)
                .WithMany(d => d.Flashcards)
                .HasForeignKey(f => f.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            // Self-reference for cloned decks
            builder.Entity<FlashcardDeck>()
                .HasOne(d => d.SourceDeck)
                .WithMany()
                .HasForeignKey(d => d.SourceDeckId)
                .OnDelete(DeleteBehavior.Restrict);

            // DeckTag many-to-many
            builder.Entity<DeckTag>()
                .HasKey(dt => new { dt.DeckId, dt.TagId });

            builder.Entity<DeckTag>()
                .HasOne(dt => dt.Deck)
                .WithMany(d => d.DeckTags)
                .HasForeignKey(dt => dt.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DeckTag>()
                .HasOne(dt => dt.Tag)
                .WithMany(t => t.DeckTags)
                .HasForeignKey(dt => dt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            // NoteTag many-to-many
            builder.Entity<NoteTag>()
                .HasKey(nt => new { nt.NoteId, nt.TagId });

            builder.Entity<NoteTag>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NoteTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tag name unique
            builder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // KnowledgeNote owner relation
            builder.Entity<KnowledgeNote>()
                .HasOne(n => n.Owner)
                .WithMany()
                .HasForeignKey(n => n.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // SavedDeck
            builder.Entity<SavedDeck>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SavedDeck>()
                .HasOne(s => s.Deck)
                .WithMany()
                .HasForeignKey(s => s.DeckId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
