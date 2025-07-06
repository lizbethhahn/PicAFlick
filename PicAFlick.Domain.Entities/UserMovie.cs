namespace PicAFlick.Domain.Entities
{
    public class UserMovie
    {
        public string? BackdropPath { get; set; }
        public DateTime? DateWatched { get; set; }
        public List<int>? GenreIds { get; set; }
        public bool IsWatched { get; set; }        
        public string? OriginalLanguage { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Title { get; set; }
        public int TmdbTvShowId { get; set; }
        public int UserMovieId { get; set; }
        public float? UserRating { get; set; }            
    }
}