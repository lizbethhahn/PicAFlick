using Microsoft.EntityFrameworkCore.ChangeTracking;
using PicAFlick.Domain.Entities;
using PicAFlick.Shared.Contracts;

namespace PicAFlick.Shared.Mappers
{
    public static class WatchlistMapper
    {
        public static WatchlistItem MapFromCreationDto(WatchlistCreationDto dto, string? userId)
        {
            return new WatchlistItem
            {
                UserId = userId,
                Notes = dto.Notes,
                Watched = false,
             // Rating = dto.UserRating
            };
        }

        public static WatchlistDisplayDto MapToDisplayDto(WatchlistItem entity)
        {
            var media = entity.UserMedia ?? throw new InvalidOperationException("User Media was not loaded.");

            return new WatchlistDisplayDto
            {
                Id = entity.Id,
                TmdbId = media.TmdbId,
                Title = media.Title,
                MediaType = media.MediaType,
                Notes = entity.Notes,
                Watched = entity.Watched,
                PosterPath = media.PosterPath,
                Overview = media.Overview,
             // UserRating = entity.Rating
            };
        }
    }
}
