using PicAFlick.Domain.Enums;
using PicAFlick.Shared.Contracts;
using PicAFlick.Infrastructure.Tmdb.Models;
using System.Globalization;

namespace PicAFlick.Domain.Services.Mappers
{
    public static class TmdbMapper
    {
        public static WatchlistCreationDto MapFromTmdbMovie(TmdbMovieDto movie, string userId)
        {
            DateTime? releaseDate = null;

            if (DateTime.TryParseExact(
                movie.ReleaseDate,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
            {
                releaseDate = parsedDate;
            }
            return new WatchlistCreationDto
            {
                Title = movie.Title ?? "Unknown Title",
                MediaType = MediaType.Movie,
                TmdbId = movie.TmdbMovieId,
                PosterPath = movie.PosterPath,
                Overview = movie.Overview,
                ReleaseDate = releaseDate,
                Notes = null
            };
        }

        public static WatchlistCreationDto MapFromTmdbTvShow(TmdbTvShowDto show, string userId)
        {
            DateTime? releaseDate = null;

            if (DateTime.TryParseExact(
                show.ReleaseDate,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
            {
                releaseDate = parsedDate;
            }
            return new WatchlistCreationDto
            {
                Title = show.Name ?? "Unknown Title",
                MediaType = MediaType.TvShow,
                TmdbId = show.TmdbTvShowId,
                PosterPath = show.PosterPath,
                Overview = show.Overview,
                ReleaseDate = releaseDate,
                Notes = null
            };
        }
    }
}

