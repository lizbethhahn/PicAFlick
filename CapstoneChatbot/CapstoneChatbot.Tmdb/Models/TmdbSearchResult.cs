using CapstoneChatbot.Tmdb.Enums;

namespace CapstoneChatbot.Tmdb.Models;

public class TmdbSearchResult
{
    public int TmdbId { get; set; }

    public string Title { get; set; } = string.Empty;

    public MediaType MediaType { get; set; }

    public string? ReleaseDate { get; set; }

    public string? Overview { get; set; }

    public string? PosterPath { get; set; }
}
