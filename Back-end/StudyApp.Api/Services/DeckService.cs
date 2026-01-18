using Microsoft.EntityFrameworkCore;
using StudyApp.Api.Data;
using StudyApp.Api.Dtos.Decks;
using StudyApp.Api.Models;

namespace StudyApp.Api.Services
{
    public class DeckService : IDeckService
    {
        private readonly ApplicationDbContext _db;

        public DeckService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> CreateDeckAsync(Guid ownerId, DeckCreateDto dto)
        {
            var deck = new FlashcardDeck
            {
                Id = Guid.NewGuid(),
                Title = dto.Title ?? string.Empty,
                Description = dto.Description,
                IsPublic = dto.IsPublic,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Handle tags (create if missing)
            if (dto.Tags != null && dto.Tags.Any())
            {
                foreach (var t in dto.Tags.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)))
                {
                    var tag = await _db.Tags.FirstOrDefaultAsync(x => x.Name == t);
                    if (tag == null)
                    {
                        tag = new Tag { Name = t, NormalizedName = t.ToUpperInvariant() };
                        _db.Tags.Add(tag);
                        await _db.SaveChangesAsync();
                    }
                    deck.DeckTags.Add(new DeckTag { DeckId = deck.Id, TagId = tag.Id });
                }
            }

            // Add flashcards
            if (dto.Cards != null && dto.Cards.Any())
            {
                foreach (var c in dto.Cards)
                {
                    var card = new Flashcard
                    {
                        Id = Guid.NewGuid(),
                        DeckId = deck.Id,
                        Front = c.Front ?? string.Empty,
                        Back = c.Back ?? string.Empty,
                        OrderIndex = c.OrderIndex,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    deck.Flashcards.Add(card);
                }
            }

            _db.FlashcardDecks.Add(deck);
            await _db.SaveChangesAsync();

            return deck.Id;
        }

        public async Task<IEnumerable<DeckSearchDto>> SearchDecksAsync(string? query)
        {
            var q = _db.FlashcardDecks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var normalized = query.Trim();
                q = q.Where(d => d.Title.Contains(normalized));
            }

            var results = await q.OrderByDescending(d => d.CreatedAt)
                .Select(d => new DeckSearchDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    IsPublic = d.IsPublic,
                    OwnerId = d.OwnerId,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();

            return results;
        }

        public async Task<Guid> CloneDeckAsync(Guid sourceDeckId, Guid ownerId)
        {
            // Load source deck with cards and tags
            var source = await _db.FlashcardDecks
                .Include(d => d.Flashcards)
                .Include(d => d.DeckTags).ThenInclude(dt => dt.Tag)
                .FirstOrDefaultAsync(d => d.Id == sourceDeckId);

            if (source == null)
                throw new InvalidOperationException("Source deck not found");

            if (!source.IsPublic)
                throw new InvalidOperationException("Source deck is not public");

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var clonedDeck = new FlashcardDeck
                {
                    Id = Guid.NewGuid(),
                    Title = source.Title,
                    Description = source.Description,
                    IsPublic = false, // cloned decks are private by default
                    OwnerId = ownerId,
                    SourceDeckId = source.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // copy tags (reference existing Tag rows)
                foreach (var dt in source.DeckTags)
                {
                    clonedDeck.DeckTags.Add(new DeckTag { DeckId = clonedDeck.Id, TagId = dt.TagId });
                }

                _db.FlashcardDecks.Add(clonedDeck);
                await _db.SaveChangesAsync();

                // copy flashcards
                foreach (var c in source.Flashcards)
                {
                    var clonedCard = new Flashcard
                    {
                        Id = Guid.NewGuid(),
                        DeckId = clonedDeck.Id,
                        Front = c.Front,
                        Back = c.Back,
                        OrderIndex = c.OrderIndex,
                        IsActive = c.IsActive,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _db.Flashcards.Add(clonedCard);
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return clonedDeck.Id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
