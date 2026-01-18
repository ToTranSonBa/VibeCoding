using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyApp.Api.Dtos.Decks;
using StudyApp.Api.Services;
using System.Security.Claims;

namespace StudyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        private readonly IDeckService _deckService;

        public DecksController(IDeckService deckService)
        {
            _deckService = deckService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery(Name = "q")] string? q)
        {
            var results = await _deckService.SearchDecksAsync(q);
            return Ok(results);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeckCreateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerId))
                return Unauthorized();

            var deckId = await _deckService.CreateDeckAsync(ownerId, dto);
            return CreatedAtAction(nameof(GetById), new { deckId }, new { id = deckId });
        }

        [HttpGet("{deckId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid deckId)
        {
            // Minimal detail for created response; implement fuller endpoint separately
            var list = await _deckService.SearchDecksAsync(null);
            var deck = list.FirstOrDefault(d => d.Id == deckId);
            if (deck == null) return NotFound();
            return Ok(deck);
        }

        [Authorize]
        [HttpPost("{deckId}/clone")]
        public async Task<IActionResult> Clone([FromRoute] Guid deckId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerId))
                return Unauthorized();

            try
            {
                var newId = await _deckService.CloneDeckAsync(deckId, ownerId);
                return CreatedAtAction(nameof(GetById), new { deckId = newId }, new { id = newId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
