using Microsoft.AspNetCore.Mvc;
using PicAFlick.Services.Interfaces;
using PicAFlick.Shared.Contracts;
using System.Security.Claims;

namespace PicAFlick.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController(IWatchlistService watchlistService) : ControllerBase
    {
        private readonly IWatchlistService _watchlistService = watchlistService;
        private string? ResolveUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GET /api/watchlist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistDisplayDto>>> GetAll()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = ResolveUserId();
#if DEBUG
            userId ??= "dev-user"; // fallback for local testing
#else
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
#endif
            var items = await _watchlistService.GetAllAsync(userId);
            return Ok(items);
        }

        // GET /api/watchlist/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _watchlistService.GetByIdAsync(id, userId);
            return Ok(item);
        }

        // POST /api/watchist
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] WatchlistCreationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var created = await _watchlistService.AddAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // DELETE /api/watchlist/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _watchlistService.RemoveEntryAsync(id, userId);
            return NoContent();
        }

        // POST /api/watchlist/{id}/watched
        [HttpPut("{id}/watched")]
        public async Task<IActionResult> MarkAsWatched(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _watchlistService.MarkAsWatchedAsync(id, userId);
            return NoContent();
        }
    }
}