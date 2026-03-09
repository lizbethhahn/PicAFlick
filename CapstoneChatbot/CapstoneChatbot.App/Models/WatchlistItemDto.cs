using CapstoneChatbot.Tmdb.Enums;
public class WatchlistItemDto
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string Title { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }

    public string? Notes { get; set; }
    public bool Watched { get; set; }

    public string? PosterPath { get; set; }
    public string? Overview { get; set; }

    public DateTime? ReleaseDate { get; set; }
}