using StudyApp.Api.Dtos.Notes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyApp.Api.Services
{
    public interface INotesService
    {
        Task<Guid> CreateAsync(Guid ownerId, NoteCreateDto dto);
        Task<Guid> CreateFromFileAsync(Guid ownerId, IFormFile file, bool isPublic = false);
        Task<NoteDto?> GetAsync(Guid id, Guid? requesterId = null);
        Task<string?> GetRawAsync(Guid id, Guid? requesterId = null);
        Task UpdateAsync(Guid id, Guid requesterId, NoteUpdateDto dto);
        Task DeleteAsync(Guid id, Guid requesterId);
        Task<IEnumerable<NoteDto>> SearchAsync(string? q);
    }
}
