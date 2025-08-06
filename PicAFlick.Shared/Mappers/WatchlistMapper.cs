using PicAFlick.Domain.Entities;
using PicAFlick.Shared.Contracts;

namespace PicAFlick.Domain.Services.Mappers
{
    public static class WatchlistMapper
    {
        public static WatchlistItem MapFromCreationDto(WatchlistCreationDto dto, string userId)
        {
            return new WatchlistItem
            {
                UserId = userId,
                Title = dto.Title,
                MediaType = dto.MediaType,
                TmdbId = dto.TmdbId,
                PosterPath = dto.PosterPath,
                ReleaseYear = dto.ReleaseYear,
                Overview = dto.Overview,
                Notes = dto.Notes,
                DateAdded = DateTime.UtcNow
            };
        }

        public static WatchlistDisplayDto MapToDisplayDto(WatchlistItem item)
        {
            return new WatchlistDisplayDto
            {
                Id = item.Id,
                UserId = item.UserId,
                Title = item.Title!,
                MediaType = item.MediaType,
                TmdbId = item.TmdbId,
                PosterPath = item.PosterPath,
                ReleaseYear = item.ReleaseYear,
                Overview = item.Overview,
                Notes = item.Notes,
                Watched = item.Watched,
                // UserRating = item.Rating // when you wire it up
            };
        }
    }
}
