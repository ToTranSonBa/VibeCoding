using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyApp.Api.Data;
using StudyApp.Api.Dtos.Notes;
using StudyApp.Api.Models;
using System.Text;

namespace StudyApp.Api.Services
{
    public class NotesService : INotesService
    {
        private readonly ApplicationDbContext _db;

        public NotesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> CreateAsync(Guid ownerId, NoteCreateDto dto)
        {
            var note = new KnowledgeNote
            {
                Id = Guid.NewGuid(),
                OwnerId = ownerId,
                Title = dto.Title ?? string.Empty,
                Content = dto.Content ?? string.Empty,
                IsPublic = dto.IsPublic,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.KnowledgeNotes.Add(note);
            await _db.SaveChangesAsync();
            return note.Id;
        }

        public async Task<Guid> CreateFromFileAsync(Guid ownerId, IFormFile file, bool isPublic = false)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            string content;
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            var title = Path.GetFileNameWithoutExtension(file.FileName) ?? "Untitled";

            var note = new KnowledgeNote
            {
                Id = Guid.NewGuid(),
                OwnerId = ownerId,
                Title = title,
                Content = content,
                IsPublic = isPublic,
                OriginalFileName = file.FileName,
                FileSize = file.Length,
                MimeType = file.ContentType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.KnowledgeNotes.Add(note);
            await _db.SaveChangesAsync();
            return note.Id;
        }

        public async Task<NoteDto?> GetAsync(Guid id, Guid? requesterId = null)
        {
            var note = await _db.KnowledgeNotes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) return null;

            if (!note.IsPublic && requesterId != note.OwnerId) return null;

            return new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                IsPublic = note.IsPublic,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public async Task<string?> GetRawAsync(Guid id, Guid? requesterId = null)
        {
            var note = await _db.KnowledgeNotes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) return null;
            if (!note.IsPublic && requesterId != note.OwnerId) return null;
            return note.Content;
        }

        public async Task UpdateAsync(Guid id, Guid requesterId, NoteUpdateDto dto)
        {
            var note = await _db.KnowledgeNotes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) throw new InvalidOperationException("Note not found");
            if (note.OwnerId != requesterId) throw new UnauthorizedAccessException();

            if (dto.Title != null) note.Title = dto.Title;
            if (dto.Content != null) note.Content = dto.Content;
            if (dto.IsPublic.HasValue) note.IsPublic = dto.IsPublic.Value;

            note.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid requesterId)
        {
            var note = await _db.KnowledgeNotes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) throw new InvalidOperationException("Note not found");
            if (note.OwnerId != requesterId) throw new UnauthorizedAccessException();

            _db.KnowledgeNotes.Remove(note);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<NoteDto>> SearchAsync(string? q)
        {
            var query = _db.KnowledgeNotes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim();
                query = query.Where(n => n.Title.Contains(t) || n.Content.Contains(t));
            }

            return await query.OrderByDescending(n => n.CreatedAt)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    IsPublic = n.IsPublic,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
