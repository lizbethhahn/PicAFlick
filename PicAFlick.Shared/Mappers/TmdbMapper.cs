using PicAFlick.Domain.Enums;
using PicAFlick.Shared.Contracts;
using PicAFlick.Infrastructure.Tmdb.Models;

namespace PicAFlick.Domain.Services.Mappers
{
    public static class TmdbMapper
    {
        public static WatchlistCreationDto MapFromTmdbMovie(TmdbMovieDto movie, string userId)
        {
            return new WatchlistCreationDto
            {
                Title = movie.Title ?? "Unknown Title",
                MediaType = MediaType.Movie,
                TmdbId = movie.TmdbMovieId,
                Notes = null
            };
        }

        public static WatchlistCreationDto MapFromTmdbTvShow(TmdbTvShowDto show, string userId)
        {
            return new WatchlistCreationDto
            {
                Title = show.Name ?? "Unknown Title",
                MediaType = MediaType.TvShow,
                TmdbId = show.TmdbTvShowId,
                Notes = null
            };
        }
    }
}

