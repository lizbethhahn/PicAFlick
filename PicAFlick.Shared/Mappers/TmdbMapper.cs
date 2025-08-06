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
                UserId = userId,
                Title = movie.Title ?? "Unknown Title",
                MediaType = MediaType.Movie,
                TmdbId = movie.TmdbMovieId,
                PosterPath = movie.PosterPath,
                ReleaseYear = ParseYearFromDate(movie.ReleaseDate),
                Overview = movie.Overview,
                Notes = null
            };
        }

        public static WatchlistCreationDto MapFromTmdbTvShow(TmdbTvShowDto show, string userId)
        {
            return new WatchlistCreationDto
            {
                UserId = userId,
                Title = show.Name ?? "Unknown Title",
                MediaType = MediaType.TvShow,
                TmdbId = show.TmdbTvShowId,
                PosterPath = show.PosterPath,
                ReleaseYear = ParseYearFromDate(show.FirstAirDate),
                Overview = show.Overview,
                Notes = null
            };
        }

        private static int? ParseYearFromDate(string? date)
            => DateTime.TryParse(date, out var dt) ? dt.Year : null;
    }
}

