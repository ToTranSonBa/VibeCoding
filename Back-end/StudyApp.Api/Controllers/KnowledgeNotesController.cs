using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyApp.Api.Dtos.Notes;
using StudyApp.Api.Services;
using System.Security.Claims;

namespace StudyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KnowledgeNotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public KnowledgeNotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery(Name = "q")] string? q)
        {
            var results = await _notesService.SearchAsync(q);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Guid? requester = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (Guid.TryParse(userIdClaim, out var uid)) requester = uid;

            var note = await _notesService.GetAsync(id, requester);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpGet("{id}/raw")]
        public async Task<IActionResult> GetRaw([FromRoute] Guid id)
        {
            Guid? requester = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (Guid.TryParse(userIdClaim, out var uid)) requester = uid;

            var raw = await _notesService.GetRawAsync(id, requester);
            if (raw == null) return NotFound();
            return Content(raw, "text/markdown");
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] bool isPublic = false)
        {
            if (file == null) return BadRequest("No file provided.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerId))
                return Unauthorized();

            // Validate extension
            var ext = Path.GetExtension(file.FileName) ?? string.Empty;
            if (!ext.Equals(".md", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only .md files are allowed.");

            var id = await _notesService.CreateFromFileAsync(ownerId, file, isPublic);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteCreateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerId))
                return Unauthorized();

            var id = await _notesService.CreateAsync(ownerId, dto);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] NoteUpdateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerId))
                return Unauthorized();

            try
            {
                await _notesService.UpdateAsync(id, ownerId, dto);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerId))
                return Unauthorized();

            try
            {
                await _notesService.DeleteAsync(id, ownerId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
