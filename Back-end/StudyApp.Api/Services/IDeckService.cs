using StudyApp.Api.Dtos.Decks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyApp.Api.Services
{
    public interface IDeckService
    {
        Task<Guid> CreateDeckAsync(Guid ownerId, DeckCreateDto dto);
        Task<IEnumerable<DeckSearchDto>> SearchDecksAsync(string? query);
        Task<Guid> CloneDeckAsync(Guid sourceDeckId, Guid ownerId);
    }
}
