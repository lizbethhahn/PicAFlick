using Microsoft.AspNetCore.Mvc;
using PicAFlick.Services.Interfaces;
using PicAFlick.Shared.Contracts;
using System.Security.Claims;

namespace PicAFlick.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController(IWatchlistService watchlistService, IHostEnvironment env) : ControllerBase
    {
        private readonly IWatchlistService _watchlistService = watchlistService;
        private readonly IHostEnvironment _env = env;

        private string? ResolveUserId()
        {
            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(id)) return id;

            // Dev fallback ONLY when not authenticated
            if (_env.IsDevelopment()) return "dev-user";

            return null; // will cause 401 below
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistDisplayDto>>> GetAll(CancellationToken ct)
        {
            var userId = ResolveUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var items = await _watchlistService.GetAllAsync(userId, ct);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] WatchlistCreationDto dto, CancellationToken ct)
        {
            var userId = ResolveUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var created = await _watchlistService.AddAsync(dto, userId, ct);
            if (created != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            else
            {
                return BadRequest("Could not create watchlist item.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var userId = ResolveUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var item = await _watchlistService.GetByIdAsync(id, userId, ct);
            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var userId = ResolveUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _watchlistService.RemoveEntryAsync(id, userId, ct);
            return NoContent();
        }

        [HttpPut("{id:int}/watched")]
        public async Task<IActionResult> MarkAsWatched(int id, CancellationToken ct)
        {
            var userId = ResolveUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _watchlistService.MarkAsWatchedAsync(id, userId, ct);
            return NoContent();
        }
    }
}